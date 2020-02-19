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
    public class AjaxDonutOutputCacheAttribute : DonutOutputCacheAttribute
    {

        public AjaxDonutOutputCacheAttribute()
            : base()
        {

        }

        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            if (!filterContext.RequestContext.HttpContext.Request.IsAjaxRequest())
            {
                _cacheSettings = BuildCacheSettings();//We need to create this to avoid breaking the bottom logic.
                return;
            }
            base.OnActionExecuting(filterContext);
            return;
        }


    }
}