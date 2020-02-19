using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using Microsoft.Practices.Unity;
using MVCSite.DAC.Interfaces;
using MVCSite.DAC.Instrumentation.Membership;
using System.Web.Security;
using System.Web.Script.Serialization;
using MVCSite.DAC.Instrumentation;
using System.Threading;
using System.Security.Principal;
using System.Globalization;
using MVCSite.Web.Extensions;
using MVCSite.DAC.Common;
using MVCSite.Web.Controllers;
using MVCSite.DAC.Entities;
using MVCSite.ViewResource;
using MVCSite.Common;
using Stripe;
using System.Net;
using MVCSite.Web.RouteHandler;
using MVCSite.Web.ViewModels;

namespace MVCSite.Web
{
    public class MvcApplication : System.Web.HttpApplication
    {
        public static IUnityContainer RootContainer { get; private set; }

// ReSharper disable InconsistentNaming
        protected void Application_Start()
        {
            log4net.Config.XmlConfigurator.Configure();
            ViewEngines.Engines.Clear();
            var engine = new ExtendedRazorViewEngine();
            engine.AddViewLocationFormat("~/Views/{1}/{0}.cshtml");
            engine.AddPartialViewLocationFormat("~/Views/{1}/{0}.cshtml");

            ViewEngines.Engines.Add(engine);

            AreaRegistration.RegisterAllAreas();

            RegisterGlobalFilters(GlobalFilters.Filters);
            RegisterRoutes(RouteTable.Routes);

            CustomMembershipProvider.RootContainer = RootContainer =
                new WebsiteUnityContainer(new ConnectionStrings
                {
                    kootourConnectionString = "name=kootourConnectionString",
                    statConnectionString = "name=statConnectionString",
                });

            var controllerFactory = RootContainer.Resolve<UnityControllerFactory>();
            ControllerBuilder.Current.SetControllerFactory(controllerFactory);
            ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12 | SecurityProtocolType.Tls11 | SecurityProtocolType.Tls;


#if DEBUG
            StripeConfiguration.SetApiKey(TouristController._StripeSecretKeyTEST);
#else
            StripeConfiguration.SetApiKey(TouristController._StripeSecretKey);
#endif

            //ModelBinders.Binders.Add(typeof(DoneRedirect), new DoneRedirectModelBinder());
        }
        void Application_BeginRequest(object sender, EventArgs e)
        {
            //var dest = HttpContext.Current.Request.Url.AbsoluteUri;
            //if (dest.Contains("juan.so"))
            //{
            //    dest = dest.Replace("juan.so", "vjiaoshi.com");
            //    HttpContext.Current.Response.Redirect(dest);
            //}
            //string path = HttpContext.Current.Request.Url.AbsolutePath;

            //if (HttpContext.Current.Request.ServerVariables["HTTPS"] == "on")
            //{
            //    if (SecurePath.IsSecure(path))
            //    {
            //        //do nothing
            //    }
            //    else
            //    {
            //        HttpContext.Current.Response.Redirect(HttpContext.Current.Request.Url.AbsoluteUri.Replace("https://", "http://"));
            //        return;
            //    }
            //}

            //if (HttpContext.Current.Request.ServerVariables["HTTPS"] != "on")
            //{
            //    //if (SecurePath.IsSecure(path))
            //    //{
            //    //    //Redirect to https version
            //    var dest = HttpContext.Current.Request.Url.AbsoluteUri;
            //    if (!dest.StartsWith("http://localhost"))
            //    {
            //        dest = dest.Replace("http://", "https://");
            //        dest = dest.Replace("juan.so", "vjiaoshi.com");
            //        HttpContext.Current.Response.Redirect(dest);
            //    }
            //    //}
            //}

        }
        protected void Application_Error()
        {
            var ctx = HttpContext.Current;
            var logger = RootContainer.Resolve<ILogger>();
            if (HttpContext.Current != null)
            {
                var exception = ctx.Server.GetLastError();
                string errorMsg;
                if (HttpContext.Current.Request != null)
                    errorMsg = string.Format("Unexpected error happened. Request url: '{0}'", HttpContext.Current.Request.RawUrl);
                else
                    errorMsg = "Unexpected error happened:";

                logger.LogError(errorMsg, exception);
            }
            else
                logger.LogError("Unexpected error happened, unable to get last error because HttpContext is null");

        }
        protected void Application_AcquireRequestState(object sender, EventArgs e)
        {
            if (Request.RawUrl == "/Account/Hello" || Request.RawUrl == "/Word/MVersion")
                return;//ignore the ping request.
            //It's important to check whether session object is ready
            if (HttpContext.Current.Session != null)
            {
                CultureInfo ci = (CultureInfo)this.Session["Culture"];
                //Checking first if there is no value in session 
                //and set default language 
                //this can happen for first user's request
                if (ci == null)
                {
                    //Sets default culture to english invariant
                    //string langName = "zh";
                    string langName = "en";
                    ////Try to get values from Accept lang HTTP header
                    //if (HttpContext.Current.Request.UserLanguages != null && HttpContext.Current.Request.UserLanguages.Length != 0)
                    //{
                    //    //Gets accepted list 
                    //    langName = HttpContext.Current.Request.UserLanguages[0].Substring(0, 2);
                    //}
                    ci = new CultureInfo(langName);
                    this.Session["Culture"] = ci;
                }
                //Finally setting culture for each request
                Thread.CurrentThread.CurrentUICulture = ci;
                Thread.CurrentThread.CurrentCulture = CultureInfo.CreateSpecificCulture(ci.Name);
            }
        }

        protected void Application_PostAuthenticateRequest(object sender, EventArgs e)
        {
            var formsCookie = Request.Cookies[FormsAuthentication.FormsCookieName];

            if (formsCookie == null)
                return;

            var auth = FormsAuthentication.Decrypt(formsCookie.Value);

            var ser = new JavaScriptSerializer();
            var userData = ser.Deserialize<FormsTicketDataV1>(auth.UserData);

            var principal = new CustomPrincipal(new GenericIdentity(auth.Name), userData);

            Context.User = Thread.CurrentPrincipal = principal;
        }
        protected void Session_Start(Object sender, EventArgs e)
        {
            var tracker = RootContainer.Resolve<Tracker>();
            tracker.OnSessionStart();
        }

        static void RegisterGlobalFilters(GlobalFilterCollection filters)
        {
            filters.Add(new HandleErrorAttribute());
        }
        static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");
            //routes.IgnoreRoute("{resource}.css/{*pathInfo}");
            //routes.IgnoreRoute("{resource}.js/{*pathInfo}");
            //routes.IgnoreRoute("{resource}.png/{*pathInfo}");
            //routes.IgnoreRoute("{resource}.jpg/{*pathInfo}");
            //routes.IgnoreRoute("{resource}.gif/{*pathInfo}");

            routes.IgnoreRoute("images/{*pathInfo}");
            routes.IgnoreRoute("Content/{*pathInfo}");
            routes.IgnoreRoute("Content/font/{*pathInfo}");
            routes.IgnoreRoute("assets/{*pathInfo}");
            routes.IgnoreRoute("assets/css/{*pathInfo}");
            routes.IgnoreRoute("node_modules/{*pathInfo}");

            routes.IgnoreRoute("Scripts/{*pathInfo}");
            routes.IgnoreRoute("ClientBin/{*pathInfo}");
            routes.IgnoreRoute("js/{*pathInfo}");
            routes.IgnoreRoute("kootour.ico");
            routes.IgnoreRoute("AdditionalFiles/{*pathInfo}");

            routes.MapMvcAttributeRoutes();

            //routes.MapRoute(
            //    "BoardMember",                       // Route name
            //    "Board/{title}/{boardId}/members",   // URL with parameters
            //    new { controller = "Board", action = "Members", boardId = "" },  // Parameter defaults
            //    new { title = new NotInThese(new string[] { "GetList" }), boardId = @"\d+" }
            //);
            //routes.MapRoute(
            //    "SingleBoard",                      // Route name
            //    "Board/{title}/{boardId}/Index/",   // URL with parameters
            //    new { controller = "Board", action = "Index", boardId = "" },  // Parameter defaults
            //    new { title = new NotInThese(new string[] { "GetList", "Create", "CreateList", "DeleteInvite" }), boardId = @"\d+" }
            //);
            //routes.MapRoute(
            //    "PrivateSingleBoard",                      // Route name
            //    "Board/{title}/{boardId}/PrivateIndex/",   // URL with parameters
            //    new { controller = "Board", action = "PrivateIndex", boardId = "" },  // Parameter defaults
            //    new { title = new NotInThese(new string[] { "GetList", "Create", "CreateList", "DeleteInvite" }), boardId = @"\d+" }
            //);

            //routes.MapRoute(
            //    "CardDetail",                        // Route name
            //    "Card/{title}/{cardId}/{boardId}",   // URL with parameters
            //    new { controller = "Card", action = "Detail", cardId = "", boardId="" },  // Parameter defaults
            //    new { title = new NotInThese(new string[] { "Get", "Move", "Delete", "Actions" }), 
            //        cardId = @".{8}-.{4}-.{4}-.{4}-.{12}",
            //        boardId = @".{8}-.{4}-.{4}-.{4}-.{12}",
            //    }
            //);

            //routes.MapRoute(
            //    "PrivateSingleFavorite",            // Route name
            //    "Favorite/{title}/{id}/Private/",   // URL with parameters
            //    new { controller = "Favorite", action = "Private", id = "" },  // Parameter defaults
            //    new { title = new NotInThese(new string[] { "GetList", "Create", "CreateList" }), id = @"\d+" }
            //);
            //routes.MapRoute(
            //    "PublicSingleFavorite",            // Route name
            //    "Favorite/{title}/{id}/Public/",   // URL with parameters
            //    new { controller = "Favorite", action = "Public", id = "" },  // Parameter defaults
            //    null
            //);
            //routes.MapRoute(
            //    "PublicListSingleFavorite",            // Route name
            //    "Favorite/{title}/{id}/ListPublic/",   // URL with parameters
            //    new { controller = "Favorite", action = "ListPublic", id = "" },  // Parameter defaults
            //    null
            //);
            //routes.MapRoute(
            //    "SingleBiz",   // Route name
            //    "biz/{id}/",   // URL with parameters
            //    new { controller = "Home", action = "Biz", id = "" },  // Parameter defaults
            //    new { id = @".{8}-.{4}-.{4}-.{4}-.{12}" }
            //);


            //if (StaticSiteConfiguration.IsGuiderSite)
            //    routes.MapRoute(
            //        "Default",
            //        "{controller}/{action}/{id}",
            //        new { controller = "Home", action = "Index", id = UrlParameter.Optional },
            //        null
            //    );
            //else


            routes.MapRoute(
                "tourname",
                "Tour/{tourname}/{id}",
                new { controller = "Tourist", action = "Tour", tourname = UrlParameter.Optional },
                null
                );

            routes.MapRoute(
                "FullPathTours",
                "Tourist/Tours/{cname}/{cat}",
                new { controller = "Tourist", action = "Tours", cat = UrlParameter.Optional },
                new { cname = new FullPathToursConstraint() }
                );

            routes.MapRoute(
                "cname",
                "{cname}/{cat}",
                new { controller = "Tourist", action = "Tours", cat = UrlParameter.Optional },
                new { cname = new CnameConstraint() }
                ).RouteHandler = new CnameRouteHandler();

            routes.MapRoute(
                "Default",                      
                "{controller}/{action}/{id}",
                new { controller = "Tourist", action = "Index", id = UrlParameter.Optional },
                null
                );
            
            routes.MapLowercaseRoute("City", "{id}", new { controller = "Home", action = "BizCategory", id = "" });
            
        }
        public override string GetVaryByCustomString(HttpContext context, string custom)
        {
            switch (custom.ToUpper())
            {
                case "RAWURL":
                    return context.Request.RawUrl;
                default:
                    return "";
            }
        }
    }
}