using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using MVCSite.DAC.Interfaces;
using MVCSite.DAC.Repositories;
using MVCSite.DAC.Entities;
using MVCSite.DAC.Services;
using MVCSite.Web.ViewModels;
using MVCSite.Biz;
using MVCSite.DAC.Common;
using MVCSite.Biz.Interfaces;
using Microsoft.Practices.Unity;
using DevTrends.MvcDonutCaching;
using MVCSite.DAC.Instrumentation;
using MVCSite.Common;
using System.Web.Script.Serialization;
using MVCSite.Web.Extensions;


namespace MVCSite.Web.Controllers
{
    public class HomeController : LayoutBase
    {
        private readonly IPublicCommands _commands;

        public HomeController(ISecurity security, IWebApplicationContext webContext,
            IRepositoryUsers repositoryUsers,
            IPublicCommands commands,
            ISiteConfiguration configuration, ILogger logger)
            : base(repositoryUsers,security, webContext, configuration, logger)
        {
            _commands = commands;
        }



        //[DonutOutputCache(CacheProfile = "OneDay")]
        //public ActionResult Index() 
        //{
        //    //if (_security.IsCurrentUserSignedIn())
        //    //{
        //    //    //var userId=_security.GetCurrentUserId();
        //    //    //var recentUBID = _commands.UserBookTimeGetRecentUserBookID(userId);
        //    //    //if (recentUBID > 0)
        //    //    //    return RedirectToAction("OneBook", "Word", new { id = recentUBID });
        //    //    //else
        //    //        return RedirectToAction("Books", "Word", new { id = 0 });
        //    //}
        //    var model = new ViewModels.Layout
        //    {
        //        IsMember=false,
        //        IsSignedIn=false,
        //        SelectedPage = LayoutSelectedPage.Account,
        //    };
        //    return RedirectToAction("LogOn", "Account", null);
        //    //return View("Index", InitLayout(model));
            
        //}


        [Route("About-Us", Order = 1)]
        //[Route("Home/About", Order = 2)]
        public ActionResult AboutUs()
        {
            return View("AboutUs", InitLayout(new Layout()));
        }
        [Route("Terms-and-Conditions", Order = 1)]
        //[Route("Home/Terms", Order = 2)]
        public ActionResult Terms()
        {
            return View("Terms", InitLayout(new Layout()));
        }
        [Route("Payform", Order = 1)]
        //[Route("Home/Terms", Order = 2)]
        public ActionResult Payform()
        {
            return View("Payform", InitLayout(new Layout()));
        }
        [Route("Help-Home", Order = 1)]
        //[Route("Home/HelpHome", Order = 2)]
        public ActionResult HelpHome()
        {
            return View("HelpHome", InitLayout(new Layout()));
        }

        [Route("Help-Guides", Order = 1)]
        //[Route("Home/HelpGuides", Order = 2)]
        public ActionResult HelpGuides()
        {
            return View("HelpGuides", InitLayout(new Layout()));
        }
        [Route("Help-Travellers", Order = 1)]
        //[Route("Home/HelpTravellers", Order = 2)]
        public ActionResult HelpTravellers()
        {
            return View("HelpTravellers", InitLayout(new Layout()));
        }
        [Route("Help-Account-Related", Order = 1)]
        //[Route("Home/HelpAccountRelated", Order = 2)]
        public ActionResult HelpAccountRelated()
        {
            return View("HelpAccountRelated", InitLayout(new Layout()));
        }
        [Route("Help-Corporate-and-Affiliate", Order = 1)]
        //[Route("Home/HelpCorporate", Order = 2)]
        public ActionResult HelpCorporate()
        {
            return View("HelpCorporate", InitLayout(new Layout()));
        }

        [Route("Why-Travel-with-Kootour", Order = 1)]
        public ActionResult WhyKootour()
        {
            return View("WhyKootour", InitLayout(new Layout()));
        }
        //We can NOT use cache as every user is different.No parameter can pass in as the main page was cached.
        [ChildActionOnly]
        public ActionResult Header(string id,int role,int page)
        {
            var model = new ViewModels.Layout
            {
                IsMember = false,
                IsSignedIn = _security.IsCurrentUserSignedIn(),
                UserID = _security.GetCurrentUserId(),
                Role=role,
                IsIndex = (page == 1)?true:false,
            };
            InitLayout(model, id);//Pass in the city name with null to get from cockie or ip.
            return View("_Header", model);
        }
        [ChildActionOnly]
        public ActionResult HeaderGuide(string id)
        {
            var model = new ViewModels.Layout
            {
                IsMember = false,
                IsSignedIn = _security.IsCurrentUserSignedIn(),
                UserID = _security.GetCurrentUserId()
            };
            InitLayout(model, id);//Pass in the city name with null to get from cockie or ip.
            return View("_HeaderGuide", model);
        }

        [ChildActionOnly]
        public ActionResult HeaderAdmin(string id, int role, int page)
        {
            var model = new ViewModels.Layout
            {
                IsMember = false,
                IsSignedIn = _security.IsCurrentUserSignedIn(),
                UserID = _security.GetCurrentUserId(),
                Role = role,
                IsIndex = (page == 1) ? true : false,
            };
            InitLayout(model, id);//Pass in the city name with null to get from cockie or ip.
            return View("_HeaderAdmin", model);
        }

        [ChildActionOnly]
        public ActionResult HeaderAdminTour(string id)
        {
            var model = new ViewModels.Layout
            {
                IsMember = false,
                IsSignedIn = _security.IsCurrentUserSignedIn(),
                UserID = _security.GetCurrentUserId()
            };
            InitLayout(model, id);//Pass in the city name with null to get from cockie or ip.
            return View("_HeaderAdminTour", model);
        }
    }
}
