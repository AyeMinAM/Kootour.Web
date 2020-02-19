using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using MVCSite.Web.ViewModels;
using MVCSite.DAC.Entities;
using MVCSite.DAC.Interfaces;
using MVCSite.Biz.Interfaces;
using MVCSite.DAC.Common;
using System.Security;
using System.IO;
using MVCSite.DAC.Instrumentation;
using MVCSite.Common;
namespace MVCSite.Web.Controllers
{

    public class StaticController : LayoutBase
    {

        private readonly Func<IUserQueries> _queriesFactory;
        private readonly Func<IUserCommands> _commandsFactory;
        public StaticController(Func<IUserQueries> queriesFactory,
            Func<IUserCommands> commandsFactory, 
            ISecurity security, 
            IWebApplicationContext webContext,
            ISiteConfiguration configuration,
            IRepositoryUsers repositoryUsers,
            ILogger logger)
            : base(repositoryUsers, security, webContext, configuration, logger)
        {
            _commandsFactory = commandsFactory;
            _queriesFactory = queriesFactory;
        }
        [Route("Static/NotFound")]
        public ActionResult NotFound()
        {
            string referrer = string.Empty;
            if (Request.UrlReferrer != null)
                referrer = Request.UrlReferrer.ToString();
            else
                referrer = Url.Action("Index", "Tourist");
            var model = new NotFoundModel
            {
                SelectedPage = LayoutSelectedPage.Account,
                IsSignedIn = _security.IsCurrentUserSignedIn(),
                Referrer = referrer
            };
            if (model.IsSignedIn)
                model.CurrentUser = _queriesFactory().UserGetCurrentUser();
            return View(InitLayout(model));
        }
        [Route("Static/BrowserWarning")]
        public ActionResult BrowserWarning()
        {
            var model = new ViewModels.Layout
            {
                SelectedPage = LayoutSelectedPage.Account,
                IsSignedIn = _security.IsCurrentUserSignedIn(),
            };
            if (model.IsSignedIn)
                model.CurrentUser = _queriesFactory().UserGetCurrentUser();
            return View(InitLayout(model));
        }
        [Route("Static/InternalError")]
        public ActionResult InternalError()
        {
            string referrer = string.Empty;
            if (Request.UrlReferrer != null)
                referrer = Request.UrlReferrer.ToString();
            else
                referrer = Url.Action("Index", "Tourist");
            var model = new InternalErrorModel
            {
                SelectedPage = LayoutSelectedPage.Account,
                IsSignedIn = _security.IsCurrentUserSignedIn(),
                Referrer = referrer
            };
            if (model.IsSignedIn)
                model.CurrentUser = _queriesFactory().UserGetCurrentUser();
            return View(InitLayout(model));
        }
        [Route("Static/MInternalError")]
        public ActionResult MInternalError()
        {
            string referrer = string.Empty;
            if (Request.UrlReferrer != null)
                referrer = Request.UrlReferrer.ToString();
            else
                referrer = Url.Action("Index", "Home");
            _logger.LogError(string.Format("MInternalError() ENTERED with:{0}.", referrer));
            var model = new MInternalErrorModel
            {
                Referrer = referrer
            };
            return View(model);
        }
    }



}