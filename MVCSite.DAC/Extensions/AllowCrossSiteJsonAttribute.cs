using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Mvc.Html;
using System.Linq.Expressions;
using System.Text;

namespace MVCSite.Web.Extensions
{
    public class AllowCrossSiteJsonAttribute : ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            HttpContext.Current.Response.Cache.SetCacheability(HttpCacheability.NoCache);
            HttpContext.Current.Response.Cache.SetNoStore();

            filterContext.RequestContext.HttpContext.Response.AppendHeader("Access-Control-Allow-Origin", "*");
            string rqstMethod = HttpContext.Current.Request.Headers["Access-Control-Request-Method"];
            if (rqstMethod == "OPTIONS" || rqstMethod == "POST" || rqstMethod == "GET")
            {
                filterContext.RequestContext.HttpContext.Response.AppendHeader("Access-Control-Allow-Methods", "GET, POST, OPTIONS");
                filterContext.RequestContext.HttpContext.Response.AppendHeader("Access-Control-Allow-Headers", "X-Requested-With, Accept, Access-Control-Allow-Origin, Content-Type");
            }
            base.OnActionExecuting(filterContext);
        }
    }

}