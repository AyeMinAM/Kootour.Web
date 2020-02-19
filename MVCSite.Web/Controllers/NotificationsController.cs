using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using MVCSite.DAC.Interfaces;
using MVCSite.Web.ViewModels;
using MVCSite.DAC.Instrumentation;
using MVCSite.Common;
namespace MVCSite.Web.Controllers
{
    public class NotificationsController : LayoutBase
    {

        public NotificationsController(ISecurity security, 
            IWebApplicationContext webContext,
            ISiteConfiguration configuration,
            IRepositoryUsers repositoryUsers,
            ILogger logger)
            : base(repositoryUsers, security, webContext, configuration, logger)
        {
        }
        public ActionResult ChangePasswordSuccess()
        {
            return View(GetNotificationLayout());
        }

        public ActionResult ForgotPasswordSent()
        {
            return View(GetNotificationLayout());
        }

        public ActionResult ResendConfirmationSent()
        {
            return View(GetNotificationLayout());
        }
        public ActionResult PasswordChanged()
        {
            return View(GetNotificationLayout());
        }

        public ActionResult CantBidTwice()
        {
            return View(GetNotificationLayout());
        }

        public ActionResult Message(string title = "Message", string text = null)
        {
            string referrer = string.Empty;
            if (Request.UrlReferrer != null)
                referrer = Request.UrlReferrer.ToString();
            else
                referrer = Url.Action("Index", "Tourist");
            var model = new Message { 
                Title = title, 
                Text = text,
                Referrer = referrer
            };
            return View(InitLayout(model));
        }
        public ActionResult ActivationEmailSend()
        {
            var model = new ViewModels.Layout
            {
                SelectedPage = LayoutSelectedPage.Account,
                IsSignedIn = false,
                CurrentUser = null
            };
            return View(InitLayout(model));
        }
        public ActionResult EmailActivated(string requestUrl)
        {
            var model = new ViewModels.Layout
            {
                SelectedPage = LayoutSelectedPage.Account,
                IsSignedIn = false,
                CurrentUser = null,
                RequestUrl = requestUrl,
            };
            return View(InitLayout(model));
        }
        public ActionResult SingleSignOnResult(string text,bool result)
        {
            var model = new Message
            {
                SelectedPage = LayoutSelectedPage.Account,
                IsSignedIn = false,
                CurrentUser = null,
                Title = "", 
                Text = text,
                Result=result
            };
            return View(InitLayout(model));
        }
        public ActionResult BrowserEnd(string id)
        {
            var model = new Message
            {
                SelectedPage = LayoutSelectedPage.Account,
                IsSignedIn = false,
                CurrentUser = null,
                Title = "",
                Text = "",
                Result = true
            };
            return View(InitLayout(model));
        }
    }
}
