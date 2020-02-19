using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.Routing;
using MVCSite.Web.Services;

namespace MVCSite.Web.Extensions
{
    public class AuthenticatedConstraint : IRouteConstraint
    {
        public bool Match(HttpContextBase httpContext, Route route, string parameterName, RouteValueDictionary values, RouteDirection routeDirection)
        {
            return httpContext.Request.IsAuthenticated;
        }
    }

    public class NotEqual : IRouteConstraint
    {
        private string _match = String.Empty;

        public NotEqual(string match)
        {
            _match = match;
        }

        public bool Match(HttpContextBase httpContext, Route route, string parameterName, RouteValueDictionary values, RouteDirection routeDirection)
        {
            return String.Compare(values[parameterName].ToString(), _match, true) != 0;
        }
    }

    public class NotInThese : IRouteConstraint
    {
        private string[] _keywords = null;

        public NotInThese(string[] keywords)
        {
            _keywords = keywords;
        }

        public bool Match(HttpContextBase httpContext, Route route, string parameterName, RouteValueDictionary values, RouteDirection routeDirection)
        {
            return _keywords.Where(x => x.ToLower() == values[parameterName].ToString().ToLower()).SingleOrDefault() == null;
        }
    }

    public class LocalConstraint : IRouteConstraint
    {

        public bool Match(HttpContextBase httpContext, Route route, string parameterName, RouteValueDictionary values, RouteDirection routeDirection)
        {
            return httpContext.Request.IsLocal;
        }
    }

    public class CnameConstraint : IRouteConstraint
    {

        public bool Match(HttpContextBase httpContext, Route route, string parameterName, RouteValueDictionary values, RouteDirection routeDirection)
        {
            var cname = httpContext.Request.RawUrl;
            if (string.IsNullOrEmpty(cname))
                return false;
            cname = cname.Split('/')[1];
            return !Regex.IsMatch(cname, @"^Account|Admin|Guide|Home|Notifications|Tourist$");
        }
    }

    public class FullPathToursConstraint : IRouteConstraint
    {

        public bool Match(HttpContextBase httpContext, Route route, string parameterName, RouteValueDictionary values, RouteDirection routeDirection)
        {
            if (httpContext.Request.Path.StartsWith("/Tourist/Tours"))
                return true;

            return false;
        }
    }
}