using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MVCSite.DAC.Instrumentation;
using System.Web;
using System.Web.Script.Serialization;
using System.Web.Security;
using System.Threading.Tasks;
using MVCSite.Common;

namespace MVCSite.Biz
{
    public class CommandsBase
    {
        private readonly ILogger _logger;

        public CommandsBase(ILogger logger)
        {
            _logger = logger;
        }

        protected static HttpCookie CreateCookie(int userId, string userName, bool rememberMe, string intials, string fullName)
        {
            var userData = new FormsTicketDataV1 { UserId = userId, UserRole = 0, Initials = intials, FullName = fullName };
            //var userData = new FormsTicketData { UserId = Guid.NewGuid(), UserRole = 0, Initials = intials, FullName = fullName };

            var ser = new JavaScriptSerializer();
            var userDataJson = ser.Serialize(userData);

            var ticket = new FormsAuthenticationTicket(2 /*version*/, userName, DateTime.UtcNow /*issue date*/,
                                                       DateTime.UtcNow.AddMinutes(FormsAuthentication.Timeout.TotalMinutes), rememberMe, userDataJson);

            var encryptedTicket = FormsAuthentication.Encrypt(ticket);
            var authCookie= new HttpCookie(FormsAuthentication.FormsCookieName, encryptedTicket);// { Domain ="vjiaoshi.com"};
            authCookie.Path = FormsAuthentication.FormsCookiePath;
            //authCookie.Domain = "wellyours.com";//NEVER ASSIGN DOMAIN HERE AS THIS IS USED BY MULTIPLE WEBSITE.
            if (rememberMe)
                authCookie.Expires = DateTime.UtcNow.AddYears(1);
            else
                authCookie.Expires = DateTime.UtcNow.AddDays(1);
            return authCookie;
        }

        protected void ExecuteActionAsync(Action action)
        {
            Task.Factory.StartNew(() =>
            {
                try
                {
                    action();
                }
                catch (Exception e)
                {
                    _logger.LogError(e);
                }
            });
        }
        protected void ExecuteActionsAsync(IEnumerable<Action> actions)
        {
            Task.Factory.StartNew(() =>
            {
                try
                {
                    foreach (var action in actions)
                        action();
                }
                catch (Exception e)
                {
                    _logger.LogError(e);
                }
            });
        }
    }
}
