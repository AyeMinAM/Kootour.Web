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
    public class JsonpResult : JsonResult
    {
        public override void ExecuteResult(ControllerContext context)
        {
            if (context == null)
            {
                throw new ArgumentNullException("context");
            }
            var request = context.HttpContext.Request;
            var response = context.HttpContext.Response;
            string callback = (context.RouteData.Values["callback"] as string) ?? request["callback"];
            if (!string.IsNullOrEmpty(callback))
            {
                if (string.IsNullOrEmpty(base.ContentType))
                {
                    base.ContentType = "application/x-javascript";
                }
                response.Write(string.Format("{0}(", callback));
            }
            base.ExecuteResult(context);
            if (!string.IsNullOrEmpty(callback))
            {
                response.Write(")");
            }
        }
    }

}