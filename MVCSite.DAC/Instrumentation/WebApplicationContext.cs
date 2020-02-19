using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Mvc;
using MVCSite.DAC.Interfaces;
using System.Web;
using MVCSite.DAC.Instrumentation.Membership;
using System.Security;
using MVCSite.DAC.Entities;

namespace MVCSite.DAC.Instrumentation
{
    public class WebApplicationContext : IWebApplicationContext
    {
        public Uri RequestUrl
        {
            get { return HttpContext.Current.Request.Url; }
        }
        public bool IsLocalUrl(string url)
        {
            return new UrlHelper(HttpContext.Current.Request.RequestContext).IsLocalUrl(url);
        }


        /// <summary>
        /// don't use that property ! it will be removed after refactoring
        /// </summary>
        public UrlHelper UrlHelper
        {
            get { return new UrlHelper(HttpContext.Current.Request.RequestContext); }
        }
        public string ServerUrl
        {
            get { return HttpContext.Current.Request.Url.GetLeftPart(UriPartial.Authority); }
        }
        public string ServerUrlOrNull
        {
            get { return HttpContext.Current == null ? null : HttpContext.Current.Request.Url.GetLeftPart(UriPartial.Authority); }
        }

        public string UserIpAddress
        {
            get
            {
                var remoteAddress = HttpContext.Current.Request.ServerVariables["REMOTE_ADDR"];
                return remoteAddress;
            }

        }

        public string GetCookie(string name)
        {
            var cookie = HttpContext.Current.Request.Cookies[name];
            if (cookie == null)
                return null;
            return cookie.Value;
        }

        public void SetCookie(string name, string value)
        {
            var old = HttpContext.Current.Response.Cookies[name];
            if (old != null)
            {
                HttpContext.Current.Response.Cookies.Remove(name);
            }
            var cookie = new HttpCookie(name, value);
            HttpContext.Current.Response.Cookies.Add(cookie);
        }

        public void ValidateThatUserCanSignIn(User user, string password)
        {
            if (!user.IsConfirmed)
                throw new SecurityException("User is not activated");

            var passwordIsValid = user.Password != null && ((Crypto.VerifyHashedPassword(user.Password, password)
                                                             || Crypto.VerifyHashedPasswordOld(user.Password, password)));
            if (!passwordIsValid)
                throw new SecurityException("Not valid username or password");
        }

        public bool IsCurrentUserSignedIn()
        {
            return HttpContext.Current.Request.IsAuthenticated;
        }
        public string GetCurrentUserNameOrNull()
        {
            if (!IsCurrentUserSignedIn())
                return null;

            return HttpContext.Current.User.Identity.Name;
        }
        public string GetCurrentUserName()
        {
            if (!IsCurrentUserSignedIn())
                throw new ApplicationException("User is not signed in");

            return HttpContext.Current.User.Identity.Name;
        }
        public int? GetCurrentUserIdOrNull()
        {
            if (!IsCurrentUserSignedIn())
                return null;
            var principal = (CustomPrincipal)HttpContext.Current.User;
            return principal.UserData.UserId;
        }
        public int GetCurrentUserId()
        {
            if (!IsCurrentUserSignedIn())
                throw new ApplicationException("User is not signed in");
            var principal = (CustomPrincipal)HttpContext.Current.User;
            return principal.UserData.UserId;
        }
    }
}
