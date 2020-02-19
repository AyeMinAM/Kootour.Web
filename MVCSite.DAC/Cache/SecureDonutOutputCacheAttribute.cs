using System;
using System.Globalization;
using System.IO;
using System.Web;
using System.Web.Mvc;
using System.Web.UI;

using Microsoft.Practices.Unity;
using MVCSite.DAC.Interfaces;
using MVCSite.DAC.Instrumentation.Membership;

namespace DevTrends.MvcDonutCaching
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, Inherited = true, AllowMultiple = false)]
    public class SecureDonutOutputCacheAttribute : DonutOutputCacheAttribute
    {

        public SecureDonutOutputCacheAttribute()
            : base()
        {

        }

        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            _cacheSettings = BuildCacheSettings();

            var cacheKey = _keyGenerator.GenerateKey(filterContext, _cacheSettings);

            if (_cacheSettings.IsServerCachingEnabled)
            {
                var cachedItem = _outputCacheManager.GetItem(cacheKey);

                if (cachedItem != null)
                {
                    //var userId = HttpContext.Current.Session["CurrentUserID"];
                    //if (userId == null)
                    //    return;
                    //var applicatoin = HttpContext.Current.ApplicationInstance as MvcApplication;
                    var container = CustomMembershipProvider.RootContainer;
                    var security = container.Resolve<ISecurity>();
                    if (!security.IsCurrentUserSignedIn())
                        return;
                    var userId = security.GetCurrentUserId();
                    if (userId <= 0)
                        return;
                    if (!cachedItem.Content.Contains(string.Format("__{0}__", userId)))
                    {
                        //_outputCacheManager.RemoveItem(,cacheKey);
                        return;
                    }
                    filterContext.Result = new ContentResult
                    {
                        Content = _donutHoleFiller.ReplaceDonutHoleContent(cachedItem.Content, filterContext),
                        ContentType = cachedItem.ContentType
                    };
                }
            }

            if (filterContext.Result == null)
            {
                var cachingWriter = new StringWriter(CultureInfo.InvariantCulture);

                var originalWriter = filterContext.HttpContext.Response.Output;

                filterContext.HttpContext.Response.Output = cachingWriter;

                filterContext.HttpContext.Items[cacheKey] = new Action<bool>(hasErrors =>
                {
                    filterContext.HttpContext.Items.Remove(cacheKey);

                    filterContext.HttpContext.Response.Output = originalWriter;

                    if (!hasErrors)
                    {
                        var cacheItem = new CacheItem
                        {
                            Content = cachingWriter.ToString(),
                            ContentType = filterContext.HttpContext.Response.ContentType
                        };

                        filterContext.HttpContext.Response.Write(_donutHoleFiller.RemoveDonutHoleWrappers(cacheItem.Content, filterContext));

                        if (_cacheSettings.IsServerCachingEnabled && filterContext.HttpContext.Response.StatusCode == 200)
                        {
                            _outputCacheManager.AddItem(cacheKey, cacheItem, DateTime.Now.AddSeconds(_cacheSettings.Duration));
                        }
                    }
                });
            }
        }


    }
}