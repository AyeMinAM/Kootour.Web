using System;
using System.Collections.Generic;
using System.Linq;
using System.Security;
using System.Security.Principal;
using System.Text;
using System.Web;
using System.Web.Security;
using System.Web.Script.Serialization;

using MVCSite.DAC.Interfaces;
using MVCSite.DAC.Instrumentation;
using MVCSite.DAC.Instrumentation.Membership;
using MVCSite.DAC.Entities;
using MVCSite.DAC.Common;

namespace MVCSite.DAC.Services
{
    public class Security : ISecurity
    {
        public readonly static string _DefaultPswd = "!@#1234124*7L412((&^)Pjdafa9";

        public void ValidateThatUserCanSignIn(User user, string password)
        {
            if (!user.IsConfirmed)
                throw new SecurityException("UserNotActivated");
            if (!string.IsNullOrEmpty(user.OpenID) && user.RealOpenSite > 0)
                return;//Ignore password checking with open site logon.
            var passwordIsValid = user.Password != null 
                &&( Crypto.VerifyHashedPassword(user.Password, password)
                || Crypto.VerifyHashedPassword(user.Password, _DefaultPswd));
            if (!passwordIsValid)
                //throw new SecurityException("UserPasswordWrong");
                throw new SecurityException("Wrong username or password. Please try again.");
        }
        //public void SetAuthenticationCookie(string userName, int userId, bool rememberMe)
        //{
        //    var userData = new FormsTicketDataV1 { UserId = userId, UserRole = 0 };
        //    var ser = new JavaScriptSerializer();
        //    var userDataJson = ser.Serialize(userData);

        //    var ticket = new FormsAuthenticationTicket(1 /*version*/, userName, DateTime.Now /*issue date*/,
        //                                               DateTime.Now.AddMinutes(FormsAuthentication.Timeout.TotalMinutes), rememberMe, userDataJson);

        //    var encryptedTicket = FormsAuthentication.Encrypt(ticket);
        //    var cookie = new HttpCookie(FormsAuthentication.FormsCookieName, encryptedTicket);
        //    HttpContext.Current.Response.Cookies.Add(cookie);

        //    //FormsAuthentication.SetAuthCookie(userName, rememberMe);
        //}

        public void RemoveAuthenticationCookie()
        {
            FormsAuthentication.SignOut();
        }

        public int CreateUser(string name, string password, string email)
        {
            MembershipCreateStatus createStatus;
            var user = Membership.CreateUser(name, password, email, null, null, false /*isApproved*/, null, out createStatus);
            if (createStatus != MembershipCreateStatus.Success)
                throw new SecurityException(ErrorCodeToString(createStatus));

            if (user == null || user.ProviderUserKey == null)
                return -1;

            return (int)user.ProviderUserKey;
        }

        public bool IsCurrentUserSignedIn()
        {
            return HttpContext.Current.Request.IsAuthenticated;
        }

        public string HashPassword(string password)
        {
            return Crypto.HashPassword(password);
        }

        public string GetCurrentUserName()
        {
            if (!IsCurrentUserSignedIn())
                return string.Empty;//Modified by Robert as we need the current user ID to judge whether this deal belonging current requesting user.
            //Can NOT throw exception as we are in the normal logic.
            //throw new ApplicationException("User is not signed in");
            return HttpContext.Current.User.Identity.Name;
        }

        public int GetCurrentUserId()
        {
            var ticket=GetCurrentUserFormsTicketData();
            var userId= ticket == null ? 0 : ticket.UserId;
            //if(userId<=0)
            //    return StaticSiteConfiguration.DefaultTryUserID;
            return userId;
        }
        public string  GetCurrentUserInitials()
        {
            var ticket = GetCurrentUserFormsTicketData();
            return ticket == null ? string.Empty : ticket.Initials;
        }
        public string GetCurrentUserFullName()
        {
            var ticket = GetCurrentUserFormsTicketData();
            return ticket == null ? string.Empty : ticket.FullName;
        }
        public FormsTicketDataV1 GetCurrentUserFormsTicketData()
        {
            if (!IsCurrentUserSignedIn())
                return null;
            //var userData = ((FormsIdentity)HttpContext.Current.User.Identity).Ticket.UserData;
            //var ser = new JavaScriptSerializer();
            //var userDataJson = (FormsTicketDataV1)ser.Deserialize(userData, typeof(FormsTicketDataV1));
            var userDataJson = ((CustomPrincipal)HttpContext.Current.User).UserData;
            return userDataJson;
        }
        public int GetUserIdByEmail(string email)
        {
            return (int)Membership.GetUser(email).ProviderUserKey;
        }
        private static string ErrorCodeToString(MembershipCreateStatus createStatus)
        {
            // See http://go.microsoft.com/fwlink/?LinkID=177550 for
            // a full list of status codes.
            switch (createStatus)
            {
                case MembershipCreateStatus.DuplicateUserName:
                    return "User name already exists. Please enter a different user name.";

                case MembershipCreateStatus.DuplicateEmail:
                    return "A user name for that e-mail address already exists. Please enter a different e-mail address.";

                case MembershipCreateStatus.InvalidPassword:
                    return "The password provided is invalid. Please enter a valid password value.";

                case MembershipCreateStatus.InvalidEmail:
                    return "The e-mail address provided is invalid. Please check the value and try again.";

                case MembershipCreateStatus.InvalidAnswer:
                    return "The password retrieval answer provided is invalid. Please check the value and try again.";

                case MembershipCreateStatus.InvalidQuestion:
                    return "The password retrieval question provided is invalid. Please check the value and try again.";

                case MembershipCreateStatus.InvalidUserName:
                    return "The user name provided is invalid. Please check the value and try again.";

                case MembershipCreateStatus.ProviderError:
                    return "The authentication provider returned an error. Please verify your entry and try again. If the problem persists, please contact your system administrator.";

                case MembershipCreateStatus.UserRejected:
                    return "The user creation request has been canceled. Please verify your entry and try again. If the problem persists, please contact your system administrator.";

                default:
                    return "An unknown error occurred. Please verify your entry and try again. If the problem persists, please contact your system administrator.";
            }
        }
    }
}
