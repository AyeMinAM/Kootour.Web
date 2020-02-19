using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MVCSite.DAC.Interfaces;
using MVCSite.DAC.Instrumentation;
using MVCSite.Biz.Interfaces;
using MVCSite.DAC.Entities;
using MVCSite.DAC.Common;
using MVCSite.DAC.Instrumentation.Membership;
using System.IO;
using System.Collections;
using System.Web;
using System.Web.Mvc;
using DevTrends.MvcDonutCaching;
//using SignalR;
using MVCSite.Common;

namespace MVCSite.Biz
{
    class UserCommands : CommandsBase, IUserCommands
    {
        private readonly ISecurity _securityService;
        private readonly IWebApplicationContext _webContext;
        private readonly IEmailer _emailer;
        private readonly EmailGenerator _emailGenerator;
        private readonly IRepositoryUsers _repositoryUsers;
        private readonly ISiteConfiguration _configuration;

        private readonly PageFlow _pageFlow;
        private readonly IRepository _repository;
            //var urlToRemove = Url.Action("Settings", "Account");
            //Response.RemoveOutputCacheItem(urlToRemove);
            ////http://www.dotnetmonster.com/Uwe/Forum.aspx/asp-net-caching/230/RemoveOutputCacheItem-question

            ////    var requestContext = new System.Web.Routing.RequestContext(
            ////        new HttpContextWrapper(System.Web.HttpContext.Current),
            ////        new System.Web.Routing.RouteData());

            ////    var Url = new UrlHelper(requestContext);
        public UserCommands(ISecurity securityService,
                              IWebApplicationContext webApplicationContext,
                              IEmailer emailer,
                              EmailGenerator emailGenerator,
                              IRepositoryUsers repositoryUsers,
                              ISiteConfiguration configuration,
                              ILogger logger,
                              PageFlow pageFlow,
                              IRepository repository)
            : base(logger)
        {
            _securityService = securityService;
            _webContext = webApplicationContext;
            _emailer = emailer;
            _emailGenerator = emailGenerator;
            _repositoryUsers = repositoryUsers;
            _configuration = configuration;
            _pageFlow = pageFlow;
            _repository = repository;
        }
        private void RemoveBoardCardCache(int boardId,int cardId)
        {
            //Controller currentController = (Controller)HttpContext.Current.Session["controllerInstance"];
            //if (currentController == null)
            //    return;
            //var urlToRemove = currentController.Url.Action("Index", "Board");
            //currentController.Response.RemoveOutputCacheItem(urlToRemove);
            //urlToRemove = currentController.Url.Action("Detail", "Card");
            //currentController.Response.RemoveOutputCacheItem(urlToRemove);
            //urlToRemove = currentController.Url.Action("ActivityGetList", "Board");
            //currentController.Response.RemoveOutputCacheItem(urlToRemove);
            var cacheManager = new OutputCacheManager();
            if (boardId >0)
            {
                cacheManager.RemoveItem("Board", "Index", new { boardId = boardId });
                cacheManager.RemoveItem("Board", "PrivateIndex", new { boardId = boardId });
                cacheManager.RemoveItem("Board", "Profile", new { id = boardId });
                cacheManager.RemoveItem("Board", "ActivityGetList", new { id = boardId });
            }
            if (cardId >0)
            {
                cacheManager.RemoveItem("Card", "Detail", new { cardId = cardId, boardId = boardId });
                cacheManager.RemoveItem("Card", "ActivityList", new { id = cardId });                    
            }

            //RefreshAllBoardUsersOtherThanMyself(boardId);
        }
        //public void RefreshAllBoardUsersOtherThanMyself(int boardId)
        //{
        //    if (boardId<=0)
        //        return;
        //    var context = GlobalHost.ConnectionManager.GetHubContext<BoardHub>();
            
        //    context.Clients[boardId.ToString()].refreshBoard(_securityService.GetCurrentUserId().ToString()); 
        //}
        //public void WordSetJson(int userBookId, int index, string json, string nextMax)
        //{
        //    if (userBookId <= 0)
        //        return;
        //    var context = GlobalHost.ConnectionManager.GetHubContext<BoardHub>();
        //    context.Clients[userBookId.ToString()].wordSetJson(index, json, nextMax);
        //}
       
        //public void PushFavListContent(int favId)
        //{
        //    if (favId <=0)
        //        return;
        //    var context = GlobalHost.ConnectionManager.GetHubContext<BoardHub>();
        //    context.Clients[favId.ToString()].updateFavListContent(favId.ToString());
        //}
        public void UserUpdate(User user)
        {
            _repositoryUsers.UserUpdate(user);
            var cacheManager = new OutputCacheManager();
            cacheManager.RemoveItem("UserQueries", "UserGetCurrentUser", new { userId = user.ID });

        }










    }
}

