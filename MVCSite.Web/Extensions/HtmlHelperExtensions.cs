using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Web;
using System.Web.Mvc;
using System.Web.Mvc.Html;
using System.Web.Routing;
using System.Text;
using MVCSite.Biz;

namespace MVCSite.Web.Extensions
{
    public static class HtmlHelperExtensions
    {
        public static MvcHtmlString ActionLink(this HtmlHelper htmlHelper, WebsitePage page)
        {
            return htmlHelper.ActionLink(page.Name, page);
        }
        public static MvcHtmlString ActionLink(this HtmlHelper htmlHelper, string name, WebsitePage page)
        {
            return htmlHelper.ActionLink(name, page, new {});
        }
        public static MvcHtmlString ActionLink(this HtmlHelper htmlHelper, string name, WebsitePage page, object htmlAttributes)
        {
            return htmlHelper.ActionLink(name, page.ActionName, page.ControllerName, null, htmlAttributes);
        }

        public static MvcHtmlString AuthenticatedActionLink(this HtmlHelper htmlHelper, WebsitePage page)
        {
            return AuthenticatedActionLink(htmlHelper, page.Name, page, new {});
        }

        public static MvcHtmlString AuthenticatedActionLink(this HtmlHelper htmlHelper, WebsitePage page, object htmlAttributes)
        {
            return AuthenticatedActionLink(htmlHelper, page.Name, page, htmlAttributes);
        }
        public static MvcHtmlString AuthenticatedActionLink(this HtmlHelper htmlHelper, string name, WebsitePage page)
        {
            return htmlHelper.AuthenticatedActionLink(name, page, new {});
        }
        public static MvcHtmlString AuthenticatedActionLink(this HtmlHelper htmlHelper, string name, WebsitePage page, object htmlAttributes)
        {
            if (!htmlHelper.ViewContext.RequestContext.HttpContext.User.Identity.IsAuthenticated)
            {
                var url = new UrlHelper(HttpContext.Current.Request.RequestContext).Action(page.ActionName, page.ControllerName) + "#signin";
                var htmlAttributesString = ToHtmlAttributesString(htmlAttributes);

                return new MvcHtmlString(string.Format("<a href='{0}' {2}>{1}</a>", url, name, htmlAttributesString));
            }
            return htmlHelper.ActionLink(name, page, htmlAttributes);
        }

        public static MvcHtmlString AuthenticatedUrl(this UrlHelper urlHelper, object routeValues, WebsitePage page)
        {
            if (!urlHelper.RequestContext.HttpContext.User.Identity.IsAuthenticated)
            {
                var url = urlHelper.Action(page.ActionName, page.ControllerName, routeValues) + "#signin";
                return new MvcHtmlString(url);
            }
            return new MvcHtmlString(urlHelper.Action(page.ActionName, page.ControllerName, routeValues));
        }
        public static MvcHtmlString AuthenticatedUrl(this UrlHelper urlHelper, WebsitePage page)
        {
            return AuthenticatedUrl(urlHelper, new { }, page);
        }

        public static MvcHtmlString SamePageUrl(this UrlHelper urlHelper, object routeValues)
        {
            var request = urlHelper.RequestContext.HttpContext.Request;
            var routeValuesDictionary = new RouteValueDictionary(routeValues);
            foreach (var key in request.QueryString.AllKeys.Where(key => key != null))
            {
                if (!routeValuesDictionary.ContainsKey(key))
                    routeValuesDictionary[key] = request.QueryString[key];
            }
            var url = UrlHelper.GenerateUrl(null, null, null, routeValuesDictionary, RouteTable.Routes, request.RequestContext, true);
            return new MvcHtmlString(url);
        }


        static string ToHtmlAttributesString(object obj)
        {
            return TurnObjectIntoDictionary(obj).Aggregate(new StringBuilder(), (x, y) => x.AppendFormat("{0}='{1}' ", y.Key, y.Value), x => x.ToString());
        }

        static IDictionary<string, object> TurnObjectIntoDictionary(object data)
        {
            var attr = BindingFlags.Public | BindingFlags.Instance;
            var dict = new Dictionary<string, object>();
            foreach (var property in data.GetType().GetProperties(attr))
            {
                if (property.CanRead)
                {
                    dict.Add(property.Name, property.GetValue(data, null));
                }
            }
            return dict;
        }
    }
}