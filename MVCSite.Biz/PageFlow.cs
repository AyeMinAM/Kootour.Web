using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using MVCSite.DAC.Interfaces;

namespace MVCSite.Biz
{
    public class PageFlow
    {
        public PageFlow()
        {
        }
        public static WebsitePage Home = new WebsitePage { ControllerName = WebsiteControllers.Home, ActionName = "Index", Name = "Home" };
        public static WebsitePage AccountBoards = new WebsitePage { ControllerName = WebsiteControllers.Account, ActionName = "Boards", Name = "AccountBoards" };
        public static WebsitePage AccountDefault = new WebsitePage { ControllerName = WebsiteControllers.Account, ActionName = "Boards", Name = "Account" };
        public static WebsitePage AccountSettings = new WebsitePage { ControllerName = WebsiteControllers.Account, ActionName = "Settings", Name = "Account Settings" };
        public static WebsitePage AccountUnsubscribe = new WebsitePage { ControllerName = WebsiteControllers.Account, ActionName = "Unsubscribe", Name = "Unsubscribe" };
        public static WebsitePage AccountChangePassword = new WebsitePage { ControllerName = WebsiteControllers.Account, ActionName = "ChangePassword", Name = "ChangePassword" };

        public static WebsitePage WordBooks = new WebsitePage { 
            ControllerName = WebsiteControllers.Word, 
            ActionName = "Books", 
            Name = "WordBook",
        };


        public static WebsitePage CompanyIndex = new WebsitePage { ControllerName = WebsiteControllers.Company, ActionName = "Index", Name = "Company" };

        public static WebsitePage EmailActivated = new WebsitePage { ControllerName = WebsiteControllers.Notifications, ActionName = "EmailActivated", Name = "Email Activated" };
        public static WebsitePage ActivationEmailSend = new WebsitePage { ControllerName = WebsiteControllers.Notifications, ActionName = "ActivationEmailSend", Name = "ActivationEmailSend" };
        public static WebsitePage AbuseReportSent = new WebsitePage { ControllerName = WebsiteControllers.Notifications, ActionName = "AbuseReportSent", Name = "Abuse Report Sent" };
        public static WebsitePage ContactInfoSent =
            new WebsitePage { ControllerName = WebsiteControllers.Notifications, ActionName = "ContactInfoSent", Name = "Contact Information Sent" };
        public static WebsitePage ForgotPasswordSent =
            new WebsitePage { ControllerName = WebsiteControllers.Notifications, ActionName = "ForgotPasswordSent", Name = "Reset Password Instructions Sent" };
        public static WebsitePage ResendConfirmationSent =
            new WebsitePage { ControllerName = WebsiteControllers.Notifications, ActionName = "ResendConfirmationSent", Name = "Confirmation Mail Send Again" };
        public static WebsitePage PasswordChanged =
            new WebsitePage { ControllerName = WebsiteControllers.Notifications, ActionName = "PasswordChanged", Name = "Password is changed" };

        public static WebsitePage CancelDeal = new WebsitePage { ControllerName = WebsiteControllers.Services, ActionName = "CancelDeal", Name = "Cancel Deal" };

        public static string GetWebsitePageUrlRelative(WebsitePage page)
        {
            var urlHelper = new UrlHelper(HttpContext.Current.Request.RequestContext);
            if (!string.IsNullOrEmpty(page.PlainUrl))
                return page.PlainUrl;

            if (page.Area != null)
                return urlHelper.Action(page.ActionName, page.ControllerName, new { area = page.Area }); //TODO: implement handling of parameters (merge area parameter and others)

            return urlHelper.Action(page.ActionName, page.ControllerName, page.Parameters);
        }
        public static string GetWebsitePageUrlAbsolute(WebsitePage page,string _serverUrl)
        {
            return _serverUrl + GetWebsitePageUrlRelative(page);
        }
    }

    public class WebsitePage
    {
        public string Name;
        public string ActionName;
        public string ControllerName;
        public string Area;
        public string PlainUrl;
        public object Parameters;

        public WebsitePage Clone()
        {
            return (WebsitePage)base.MemberwiseClone();
        }

        public WebsitePage WithParameters(object parameters)
        {
            var page = Clone();
            page.Parameters = parameters;
            return page;
        }
    }

    public static class WebsiteControllers
    {
        public static string Home = "Home";
        public static string Account = "Account";
        public static string Notifications = "Notifications";
        public static string Services = "Services";
        public static string Static = "Static";

        public static string Company = "Company";
        public static string Board = "Board";
        public static string Card = "Card";

        public static string Word = "Word";
    }
}
