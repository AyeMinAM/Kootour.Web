using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
//using WMath.Facilities;
using MVCSite.DAC.Interfaces;
using MVCSite.DAC.Repositories;
using MVCSite.DAC.Services;
using MVCSite.Web.ViewModels;
using MVCSite.DAC.Common;
using Microsoft.Practices.Unity;
using MVCSite.DAC.Instrumentation;
using MVCSite.Common;
using MVCSite.DAC.Entities;
using System.Web.Script.Serialization;
using System.Drawing;
using System.Web.Routing;
using System.Collections.Specialized;
using System.Text.RegularExpressions;
using System.Globalization;
using System.Threading;
using MVCSite.DAC.Extensions;


namespace MVCSite.Web.Controllers
{

    public class LayoutBase : Base
    {
        protected const int _MaxRowTextLength = 20;
        protected const int _MaxMustConnectChatCount = 0;
        protected const int _ListViewPageSizeMobile = 3;
        protected const int _ListViewPageSize = 100;
        protected const int _ListViewPageSizeBig =500;
        protected const int _ListViewPageSizeImage = 3;
        protected const int _ListViewPageSizePopup = 3;
        protected const int _ListViewPageSizeMsg = 5;

        protected const int _UserUnreadTalkPageSize = 50;
        protected const int _ctrIdXorSeed = 51666;
        public const string _ContentResultSubmitted = "SUBMITTED";
        public const string _ContentResultOK = "OK";
        public const string _ContentResultFailed = "FAILED";
        public const string _ContentResultLogonTimeout = "LOGON_TIMEOUT";

        public const string _SessionConfirmPhone4Digits = "_SessionConfirmPhone4Digits";
        public const string _SessionConfirmingPhoneNumber = "_SessionConfirmingPhoneNumber";
        public const string _SessionConfirmingPhoneAreaCode = "_SessionConfirmingPhoneAreaCode";

        protected readonly ISecurity _security;
        protected readonly IWebApplicationContext _webContext;
        protected readonly ISiteConfiguration _configuration;
        protected readonly ILogger _logger;
        public static IUnityContainer RootContainer { set; protected get; }
        protected readonly IRepositoryUsers _repositoryUsers;
        public static List<User> DefaultRecommendedUsers = null;
        public static List<string> DefaultYearOptions = new List<string>();
        public static List<string> DefaultDayOptions = new List<string>();
        private static bool isStaticDataInited = false;
        public LayoutBase(IRepositoryUsers repositoryUsers, ISecurity security, IWebApplicationContext webContext,
            ISiteConfiguration configuration, ILogger logger)
        {
            _security = security;
            _webContext = webContext;
            _logger = logger;
            _configuration = configuration;
            _repositoryUsers = repositoryUsers;
            if (!isStaticDataInited)
            {
                isStaticDataInited = true;
                for (int i = 1950; i <= 2016; i++)
                {
                    DefaultYearOptions.Add(i.ToString());
                }
                for (int i = 1; i <= 31; i++)
                {
                    DefaultDayOptions.Add(i.ToString());
                }
            }
        }
        protected List<SelectListItem> GetIntArraySelectListItems(int start,int end)
        {
            List<string> options = new List<string>();
            for (int i = start; i <= end; i++)
            {
                options.Add(i.ToString());
            }
            return options.ToSelectList(x => x, x => x);
        }

        protected void ReloadDefaultRecommendedUsers()
        {
            if (StaticSiteConfiguration.DefaultRecommendedUserIds!=null)
            {
                LayoutBase.DefaultRecommendedUsers = _repositoryUsers.UserGetAllInUserIds(StaticSiteConfiguration.DefaultRecommendedUserIds).ToList();
            }
        }
        protected List<DashBoardMsg> GetReceivedMsgs(int pageNo,int pageSize,UserMsgStatus status,out int totalCount)
        {
            var requestUserId = _security.GetCurrentUserId();
            var pagedMsgs=_repositoryUsers.UserMsgGetAllByToUserID(requestUserId,pageNo, pageSize,status );
            totalCount=pagedMsgs.TotalCount;
            var msgs = pagedMsgs.ToList();
            if (status == UserMsgStatus.All && totalCount > 0)
            {//Set Read status 
                _repositoryUsers.UserMsgUpdateStatusByIds(msgs.Select(x => x.ID.ToString()).Aggregate((prev, next) => prev+","+next),UserMsgStatus.Read);
            }
            var userIds = msgs.Select(x => x.FromUserID).Distinct().ToArray();
            List<User> userList = _repositoryUsers.UserGetAllInUserIds(userIds).ToList();
            var dashboardMsgs = msgs.OrderByDescending(x => x.EnterTime).Select(x => new DashBoardMsg()
            {
                Msg = x.Message,
                Date = x.EnterTime.ToShortDateString(),
                FromUserId = x.FromUserID == requestUserId ? x.ToUserID : x.FromUserID,
            }).ToList();
            dashboardMsgs.ForEach(msg => {
                var user = userList.Where(x => x.ID == msg.FromUserId).SingleOrDefault();
                if (user == null)
                    return;
                msg.UserAvatarUrl = user.AvatarUrl;
                msg.UserName = user.ShowName;
            });   
            return dashboardMsgs;
        }
        protected T InitLayout<T>(T model, string cityName = null) where T : MVCSite.Web.ViewModels.Layout
        {             
            model.IsMember = _webContext.GetCookie(Constants.CookieIsMember) != null;
            int userId = _security.GetCurrentUserId();
            model.CurrentUser = userId>0? _repositoryUsers.UserGetCurrentUser(userId):null;

            model.UserID = userId;
            int totalCount = 0;
            var dashboardMsgs = GetReceivedMsgs(1, 2,UserMsgStatus.Initial,out totalCount);
            model.MsgList = dashboardMsgs;
            model.MsgCount = dashboardMsgs.Count();

            return model;
        }
        protected MVCSite.Web.ViewModels.Layout GetLayout()
        {
            return InitLayout(new MVCSite.Web.ViewModels.Layout());
        }
        protected MVCSite.Web.ViewModels.Layout GetNotificationLayout()
        {
            var layout = new MVCSite.Web.ViewModels.Layout();
            layout.SelectedPage = LayoutSelectedPage.Account;
            return InitLayout(layout);
        }
        protected RegisterModel GetRegisterModel()
        {
            return new RegisterModel { IsSignedIn=false};
        }
        protected LogOnModel GetLogOnModel()
        {
            return new LogOnModel { };
        }
        protected string GetCurrentUserIdStr()
        {
            var userId = _security.GetCurrentUserId();
            string userIdStr = string.Empty;
            if (userId >0)
                userIdStr = userId.ToString();
            else
                userIdStr = string.Empty;
            return userIdStr;
        }
        protected AccountNavigateModel CreateAccountNavigateModel(User user, AccountSelectSection selected)
        {
            var model= new AccountNavigateModel
            {
                UserInitial = user.Initials,
                UserFullName = user.FullName,
                UserName = user.Email,
                UseAvatar = user.UseAvatar??false,
                AvatarUrl = user.AvatarUrl,
                BoardsCSS="",
                ActivitiesCSS="",
                NotificationsCSS="",
                SettingsCSS="",
                FavoritesCSS="",
                BooksCSS = "",
                UseId = _security.GetCurrentUserId()
            };
            switch (selected)
            { 
                case AccountSelectSection.Boards:
                    model.BoardsCSS = "active";
                    break;
                case AccountSelectSection.Books:
                    model.BooksCSS = "active";
                    break;
                case AccountSelectSection.Settings:
                    model.SettingsCSS = "active";
                    break;
                case AccountSelectSection.Activities:
                    model.ActivitiesCSS = "active";
                    break;
                case AccountSelectSection.Notifications:
                    model.NotificationsCSS = "active";
                    break;
                case AccountSelectSection.Favorites:
                    model.FavoritesCSS = "active";
                    break;
            }
            return model;
        }
        protected ContentResult GetBranchedContentResult(string message)
        {
            if (message.Contains("Exception is: SecurityException"))
                return Content(_ContentResultLogonTimeout);
            return Content(_ContentResultFailed);
        }
        protected string CheckUserAgentLength(string userBrowser)
        {
            if (string.IsNullOrEmpty(userBrowser) || userBrowser.Length <= 150)
                return userBrowser;
            if (userBrowser.Length > 150)
            {
                _logger.LogInfo(string.Format("userBrowser.Length({0}) > 150,contents:{1}", userBrowser.Length, userBrowser));
                userBrowser = userBrowser.Substring(0, 150);
            }
            return userBrowser;
        }
        protected ContentResult CreateTextContentResultResponse(string text, bool encrypt, bool zip)
        {
            var result = text;
            if (!encrypt && !zip)
                return Content(result);
            if (encrypt)
                result = result.Encrypt();
            if (!zip)
                return Content(result);
            var zipBytes = ZipHelper.gzipCompress(result);
            return Content(zipBytes);
        }


        protected string GetAvatarUrl(string url)
        {
            if (!string.IsNullOrEmpty(url))
                return url;
            return  @"/Content/Images/community/noavatar.jpg";        
        }

        protected void DeleteFileByRelativePath(string path)
        {
            if (string.IsNullOrEmpty(path))
            {
                return;
            }
            var oldFile = Path.Combine(StaticSiteConfiguration.ImageFileDirectory, path.Replace(@"/", @"\"));
            if (System.IO.File.Exists(oldFile))
            {
                FileHelper.DeleteFileAndParentDirIfEmpty(oldFile);
            }
            return;
        }
        protected ContentResult CreateJsonResponse(object input)
        {
            var jsonResult = Json(input);
            var serializer = new JavaScriptSerializer();
            serializer.MaxJsonLength = 10240000;
            var json = serializer.Serialize(jsonResult.Data);
            return Content(json);
        }
        protected LanguageCode GetCurrentLanguage()
        {
            return LanguageCode.English;
        }

        protected bool IsNotAdmin()
        {
            int userId = _security.GetCurrentUserId();
            User user = _repositoryUsers.GetByIdOrNull(userId);
            if (userId <= 0 ||
                 (user.Email != "Yhansen@kootour.com"
                && user.Email != "ayemin@kootour.com"
                && user.Email != "ndaniel@kootour.com"
                && user.Email != "pelise@kootour.com"
                && user.Email != "fjaimie@kootour.com"
                && user.Email != "cpardeep@kootour.com"
                && user.Email != "wkevin@kootour.com"))
                return true;
            else
                return false;
        }
    }
}
