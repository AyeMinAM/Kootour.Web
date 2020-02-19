using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using System.Web.Security;
using MVCSite.Web.ViewModels;
using MVCSite.DAC.Interfaces;
using MVCSite.DAC.Common;
using MVCSite.Biz.Interfaces;
using System.Security;
using MVCSite.Biz;
using MVCSite.DAC.Entities;
using MVCSite.ViewResource;
using Microsoft.Practices.Unity;
using MVCSite.DAC.Instrumentation.Membership;
using System.IO;
using DevTrends.MvcDonutCaching;
using MVCSite.Web.Extensions;
using DotNetOpenAuth.OpenId.RelyingParty;
using MVCSite.DAC.Instrumentation;
using System.Web.Script.Serialization;
using MVCSite.Common;

using System.Text;
using MVCSite.DAC.Extensions;
using DotNetOpenAuth.OAuth;
using DotNetOpenAuth.ApplicationBlock;
using MVCSite.DAC.Services;
using MVCSite.Web.Services;

namespace MVCSite.Web.Controllers
{
    public class AccountController : LayoutBase
    {
        private readonly IPublicCommands _commands;
        private readonly Func<IUserQueries> _queriesFactory;
        private readonly Func<IUserCommands> _commandsFactory;
        protected readonly IRepositoryCities _repositoryCities;

        private static string _SingleSignOnState = string.Empty;
        private static int _helloCountSinceLast = 0;
        private static int _timeSpanToSave = 180;//seconds
        private static DateTime _lastSaveTime = DateTime.UtcNow;


        public AccountController(ISecurity security, IWebApplicationContext webContext,
            ISiteConfiguration configuration,
            IPublicCommands commands,
            Func<IUserQueries> queriesFactory,
            Func<IUserCommands> commandsFactory,
            IRepositoryUsers repositoryUsers,
            IRepositoryCities repositoryCities,
            ILogger logger
            )
            : base(repositoryUsers, security, webContext, configuration, logger)
        {
            _commands = commands;
            _queriesFactory = queriesFactory;
            _commandsFactory = commandsFactory;
            _repositoryCities = repositoryCities;
        }
        public ActionResult ChangeAvatar(int? id)
        {
            try
            {
                if (id == null || IsNotAdmin())
                    id = _security.GetCurrentUserId();
                else
                    ViewBag.id = id;
                var model = new Layout();
                return View("ChangeAvatar", InitLayout(model));
            }
            catch (Exception excp)
            {
                _logger.LogError(excp);
                return RedirectToAction("InternalError", "Static", null);
            }
        }
        public ActionResult EditProfile(int? id)
        {
            try
            {
                if (id == null || IsNotAdmin())
                    id = _security.GetCurrentUserId();
                else
                    ViewBag.id = id;
                var user = _repositoryUsers.GetByIdOrNull(id.Value);
                DateTime now = DateTime.Now;
                int year = now.Year, month = now.Month, day = now.Day;
                if(user!=null && user.Birthday !=null)
                {
                    year=user.Birthday.Value.Year;
                    month = user.Birthday.Value.Month;
                    day = user.Birthday.Value.Day;
                }
                
                var model = new EditProfileModel
                {
                    FirstName = user.FirstName,
                    LastName = user.LastName,                    
                    Location = user.Address,
                    Gender = (byte)user.Sex,
                    BirthDateDay = day,
                    BirthDateMonth = month,
                    BirthDateYear = year,
                    Email = user.Email,
                    MobilePhone = user.Mobile,
                    PhoneNumber = string.IsNullOrEmpty(user.PhoneAreaCode) ? user.Mobile : string.Format("({0}){1}", user.PhoneAreaCode, user.Mobile),
                    YearOptions = DefaultYearOptions.ToSelectList(x => x, x => x),
                    MonthOptions = MonthTranslation.Translations.ToSelectList(x => x.MonthName, x => x.MonthCode.ToString()),
                    DayOptions = DefaultDayOptions.ToSelectList(x => x, x => x),
                    AvatarUrl = user.AvatarUrl,
                    Introduction = user.Bio,
                    VideoURL = user.VideoPath,
                    IsEmailVerified = (user.RealOpenSite > 0 && user.IsEmailVerified4OpenID) || (user.RealOpenSite == 0 && user.IsConfirmed),
                    IsPhoneVerified = user.IsPhoneConfirmed,

                };                
                return View("EditProfile", InitLayout(model));
            }
            catch (Exception excp)
            {
                _logger.LogError(excp);
                return RedirectToAction("InternalError", "Static", null);
            }
        }
        [HttpPost]
        public ActionResult EditProfile(EditProfileModel model)
        {
            try
            {
                
                if (ModelState.IsValid)
                {
                    var user = _repositoryUsers.GetByIdOrNull(_security.GetCurrentUserId());
                    user.FirstName = model.FirstName;
                    user.LastName = model.LastName;                    
                    user.Address = model.Location??string.Empty;
                    user.Sex = model.Gender;
                    //user.Email = model.Email;
                    user.Mobile = model.MobilePhone;
                    user.Birthday = new DateTime(model.BirthDateYear,model.BirthDateMonth,model.BirthDateDay);
                    user.Bio = model.Introduction;
                    user.VideoPath = model.VideoURL;
                    _repositoryUsers.UserUpdate(user);
                    return RedirectToAction("EditProfile");
                }
                model.YearOptions = DefaultYearOptions.ToSelectList(x => x, x => x);
                model.MonthOptions = MonthTranslation.Translations.ToSelectList(x => x.MonthName, x => x.MonthCode.ToString());
                model.DayOptions = DefaultDayOptions.ToSelectList(x => x, x => x);
                return View("EditProfile", InitLayout(model));
            }
            catch (Exception excp)
            {
                _logger.LogError(excp);
                return RedirectToAction("InternalError", "Static", null);
            }
        }
       
        public ContentResult Hello()
        {
            if ((DateTime.UtcNow - _lastSaveTime).TotalSeconds > _timeSpanToSave)
            {
                //_repositoryUsers.UserHelloCreate(new UserHello() {
                //    Count = _helloCountSinceLast,
                //    StartTime = _lastSaveTime,
                //    EndTime = DateTime.UtcNow,
                //    SiteID = (byte)WebSiteID.VJiaoshi,
                //});                
                _helloCountSinceLast = 0;
                _lastSaveTime = DateTime.UtcNow;
            }
            _helloCountSinceLast++;
            return Content(_ContentResultOK);
        }
       
        #region For mobile
        //[HttpPost]
        //public ActionResult MLogOnQQ(QQUserInfo model)
        //{
        //    try
        //    {
        //        return LonOnQQDoRegisterOrLogon(model, null, LogonMode.AndroidMobileLogon);
        //    }
        //    catch (Exception excp)
        //    {
        //        _logger.LogError(excp);
        //        return Content(_ContentResultFailed);
        //    }
        //}

        /// <summary>
        /// Mobile phone loging on with QQ.
        /// </summary>
        /// <param name="state"></param>
        /// <returns></returns>
        [HttpPost]
        public ContentResult MLogOnQQStart(string state)
        {
            try
            {
                Session.Remove(ConstantDefs.SessionStateQQ);
                if (string.IsNullOrEmpty(_SingleSignOnState))
                    _SingleSignOnState = state;
                else
                {
                    if (!_SingleSignOnState.Contains(state))
                        _SingleSignOnState += state;
                }
                return Content(_ContentResultOK);
            }
            catch (Exception excp)
            {
                _logger.LogError(excp);
                return Content(_ContentResultFailed);
            }
        }

        #endregion
        public ContentResult InitialContext(string id)
        {
            var model = new MemberModel {
                ID = _security.GetCurrentUserId().ToString(),
                Name=_security.GetCurrentUserName(),
                FullName = _security.GetCurrentUserFullName(),
                Initials = _security.GetCurrentUserInitials(),
            };
            var html = RenderRazorViewToString("InitialContext", model);
            return Content(html);
        }

        [ChildActionOnly]
        [DonutOutputCache(Duration = 3000, VaryByParam = "userId")]
        public ActionResult MemberHeader(int userId)
        {
            var model = _queriesFactory().UserGetById(userId);
            return View("_MemberHeader", model);
        }
        public ContentResult UserNameContext()
        {
            var userName = _security.GetCurrentUserName();
            var model = new UserNameContextModel
            {
                Name = userName
            };
            var html = RenderRazorViewToString("UserNameContext", model);
            return Content(html);
        }

        public ActionResult Settings()
        {
            var queries = _queriesFactory();
            var user = queries.UserGetCurrentUser();
            var model = new UserSettingsModel
            {
                SelectedPage = LayoutSelectedPage.AccountAfterLogOn,
                CurrentUser = user,
                Navigate = CreateAccountNavigateModel(user, AccountSelectSection.Settings),
                IsSignedIn=true
            };
            return View(InitLayout(model));
        }

        //
        // GET: /Account/LogOn
        [DonutOutputCache(CacheProfile = "OneDay")]
        public ActionResult LogOn(string returnUrl, Nullable<int> role)
        {
            if (_security.GetCurrentUserId()>0)
            {
                return RedirectToAction("TourProducts", "Guide");
            }
            var model = new LogOnModel {  
                SelectedPage = LayoutSelectedPage.Account,
                ReturnUrl = returnUrl,
                Lan = GetCurrentLanguage(),
                WeChatUrl = new WeChatHelper().GetLoginUrl(role??1),
                Role = role ?? 1,
            };
            return View(InitLayout(model));
        }

        [HttpPost]
        public JsonResult JSCheckUser(JSLogOnModel model)
        {
            try
            {
                var user = _repositoryUsers.GetByOpenIDOrNull(model.loginId,(OpenSiteType)model.loginType);
                if (user == null)
                {
                    return Json(new
                    {
                        result = false,
                        message="User not found",
                    }, JsonRequestBehavior.DenyGet);                
                }
                return Json(new
                {
                    result = true,
                }, JsonRequestBehavior.DenyGet);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex);
                return Json(new
                {
                    result = false,
                    message = "Check user failed with exception!",
                }, JsonRequestBehavior.DenyGet);
            }
        }
        [HttpPost]
        public JsonResult JSLogon(JSLogOnModel model)
        {
            try
            {
                var userBrowser = CheckUserAgentLength(Request.UserAgent);
                var userIp = RequestHelper.GetClientIpAddress(Request);
                var user = _repositoryUsers.GetByOpenIDOrNull(model.loginId,(OpenSiteType)model.loginType);
                var returnModel = _commands.LogOn(user.Email, Security._DefaultPswd, false, string.Empty, userIp, userBrowser);
                return Json(new
                {
                    result = true,
                    url = Url.Action("TourProducts","Guide"),
                }, JsonRequestBehavior.DenyGet);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex);
                return Json(new
                {
                    result = false,
                    message = "Logon failed with exception!"
                }, JsonRequestBehavior.DenyGet);
            }
        }
        [HttpPost]
        public JsonResult JSRegister(JSRegisterModel model)
        {
            try
            {
                var userBrowser = CheckUserAgentLength(Request.UserAgent);
                var userIp = RequestHelper.GetClientIpAddress(Request);

                User oldEmailUser = null;
                bool withOpenSiteEmail = false;
                if (!string.IsNullOrEmpty(model.email))
                {
                    oldEmailUser = _repositoryUsers.GetByEmailOrNull(model.email);
                    withOpenSiteEmail = true;
                }
                var userEmail = model.email;
                if (oldEmailUser != null || string.IsNullOrEmpty(userEmail))
                {
                    userEmail = GetOpenSiteEmail(model.loginId, model.loginType);
                    model.email = userEmail;
                    withOpenSiteEmail = false;
                }
                UserRole role = (UserRole)model.Role;

                var page = _commands.Register(new UserRegInfo()
                {
                    FirstName = model.firstName,
                    LastName = model.lastName,
                    Email = userEmail,
                    Password = Security._DefaultPswd,
                    Title = string.Empty,
                    AreaCode = string.Empty,
                    LocalCode = string.Empty,
                    RequireConfirmationToken = false,
                    UserIp = userIp,
                    UserBrowser = userBrowser,
                    NickName = model.firstName,
                    Role = (short)role,
                    Language = (byte)GetCurrentLanguage(),
                    ReturnUrl = string.Empty,
                    OpenID=model.loginId,
                    SiteID=model.loginType,
                    WithOpenSiteEmail=withOpenSiteEmail,
                    Location=model.location,
                    Avatar=model.avatar,

                });
                return Json(new
                {
                    result = true,
                    url = Url.Action("TourProducts", "Guide"),
                }, JsonRequestBehavior.DenyGet);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex);
                return Json(new
                {
                    result = false,
                    message = "Register failed with exception!",
                }, JsonRequestBehavior.DenyGet);
            }
        }
        private string GetOpenSiteEmail(string openId, int type)
        {
            string domain = "kootour.com";
            switch ((OpenSiteType)type)
            { 
                //case OpenSiteType.Facebook:
                //    domain = "facebook.com";
                //    break;
                //case OpenSiteType.WeChat:
                //    domain = "wechat.com";
                //    break;
                default:
                    domain = "kootour.com";
                    break;
            }
            return "noemail"+openId + "@" + domain;
        }
        private ActionResult GetLogonOKResult(UserRole userRole)
        {
            return RedirectToAction("Index", "Tourist");   
            //if (userRole == UserRole.Guider)
            //    return RedirectToAction("TourProducts", "Guide", new { page = 1 });
            //else
            //    return RedirectToAction("Dashboard", "Tourist");          
        }
        [HttpPost]
        public ActionResult LogOn(LogOnModel model)
        {
            try
            {
                LogonMode from = (LogonMode)model.From;
                _logger.LogInfo(string.Format("LogOn() entered with LogonMode:{0}", from));
                var userBrowser = string.Empty;
                userBrowser = CheckUserAgentLength(Request.UserAgent);
                Validators.ValidEmail(_repositoryUsers,ModelState,model.Email, "Email");
                Validators.ValidRegisterPassword(ModelState, _configuration, model.Password, "Password");
                if (ModelState.IsValid)
                {
                    try
                    {
                        //model.RememberMe = true;
                        var userIp = RequestHelper.GetClientIpAddress(Request);
                        var returnModel = _commands.LogOn(model.Email, model.Password, model.RememberMe, model.ReturnUrl, userIp, userBrowser);
                        var page = returnModel.Page;
                        var redirectUrl = page.PlainUrl;
                        if (model.ReturnUrl == "Tour")
                            return Json(new
                            {
                                Result = true,
                                html = redirectUrl
                            });
                        if (!string.IsNullOrEmpty(model.ReturnUrl))
                        {
                            return Redirect(model.ReturnUrl);
                        }
                        var user = _repositoryUsers.GetByEmailOrNull(model.Email);
                       
                        
                        return GetLogonOKResult(user.RealRole);
                   }
                    catch (SecurityException e)
                    {
                        ModelState.AddModelError("Password", e.Message);
                    }
                    catch (ApplicationException e)
                    {
                        ModelState.AddModelError("", e.Message);
                    }
                }
                _logger.LogError(string.Format("LogOn() GOT INVALID ModelState:{0}", ModelState.ToString()));
                if (model.ReturnUrl == "Tour")
                {

                  
                    return Json(new
                    {
                        result = false,
                        modelErrors = ModelState.AllErrors()
                });
                }
                model.SelectedPage = LayoutSelectedPage.Account;

                return View("LogOn", InitLayout(model));
            }
            catch (Exception excp)
            {
                _logger.LogError(excp);
                return RedirectToAction("InternalError","Static");
            }
        }
        public ActionResult ActivateUser(int userId, string token, string returnUrl)
        {
            var user=_repositoryUsers.ActivateUser(userId, token);
            if (user!=null)
            {
                if (user.TokenExpireTime < DateTime.UtcNow)
                {
                    return RedirectToAction("LogOn");
                }
                _commands.LogOnSaveCookies(user, true);
                if (user.OpenSite>0 && !user.IsEmailVerified4OpenID)
                {//OpenSite to change email
                    var password = token.Substring(0, 8);
                    var email = HttpUtility.UrlDecode(returnUrl);
                    var oldUser = _repositoryUsers.GetByEmailOrNull(email);
                    if (oldUser != null)
                    {//Before user activate,somebody else has registerd with this email
                        return RedirectToAction("Message", "Notifications", new { title = "Email Occupied", text = ValidationStrings.EmailAlreadyUsed });
                    }
                    _repositoryUsers.UserUpdatePasswordFromOpenSite(user.ID, email, password);
                    return GetLogonOKResult(user.RealRole);
                    //DoLogOut();
                    //return RedirectToAction("RememberNewPassword", "Account", new { email = user.Email, pswd = password });
                }
                if (!string.IsNullOrEmpty(returnUrl))
                {
                    return Redirect(returnUrl);                    
                }
                return GetLogonOKResult(user.RealRole);
            }
            else
                return RedirectToAction("LogOn");
        }
        [HttpPost]
        public ActionResult LogOnGoogleContact(LogOnModel model)
        {
            var tokenMgr = new InMemoryTokenManager(StaticSiteConfiguration.GoogleConsumerKey,StaticSiteConfiguration.GoogleConsumerSecret);
            var google = new WebConsumer(GoogleConsumer.ServiceDescription, tokenMgr);

            string AccessToken = string.Empty;
            // Is Google calling back with authorization?
            var accessTokenResponse = google.ProcessUserAuthorization();
            if (accessTokenResponse != null)
            {
                AccessToken = accessTokenResponse.AccessToken;
            }
            else if ( string.IsNullOrEmpty(AccessToken))
            {
                // If we don't yet have access, immediately request it.
                GoogleConsumer.RequestAuthorization(google, GoogleConsumer.Applications.Contacts);
            }
            return RedirectToAction("Boards");
        }
        public ActionResult LogOnWeChat(string code, string state)
        {
            bool isFromBrowswer = true;
            string hintText = "Use WeChat account";
            try
            {
                _logger.LogInfo(string.Format("LogOnWeChat() ENTERED WITH code:{0},state:{1}.", code, state));
                string[] stateParts = state.Split(new string[]{WeChatHelper.stateSeparator},StringSplitOptions.RemoveEmptyEntries);
                if (stateParts == null || stateParts.Length != 2 || stateParts[0] != WeChatHelper.WeChatState)
                {
                    _logger.LogInfo(string.Format("LogOnWeChat() FAILED as WeChatHelper.WeChatState:{0},state:{1}.", 
                        WeChatHelper.WeChatState, state));
                    return RedirectToAction("LogOn");//We pass stateInSessionStr=null as it sometimes happen 
                }
                var helper =new WeChatHelper();
                var token = helper.Get_token(code);
                var userInfo = helper.Get_UserInfo(token.access_token,token.openid);

                if (userInfo == null)
                {
                    _logger.LogInfo(string.Format("LogOnWeChat() CANCELED as userInfo == null."));
                    return RedirectToAction("LogOn");
                }
                int userRole = SafeConvert.ToInt32(stateParts[1]);
                return LonOnWeChatDoRegisterOrLogon(userInfo, userRole);
            }
            catch (Exception exp)
            {
                _logger.LogError(exp.Message + "  Call Stack:" + exp.StackTrace);
            }
            if (isFromBrowswer)
                return RedirectToAction("LogOn", "Account");
            else
                return RedirectToAction("SingleSignOnResult", "Notifications", new { text = hintText, result = false });
        }
        private ActionResult LonOnWeChatDoRegisterOrLogon(MVCSite.Web.Controllers.WeChatHelper.OAuthUser wechatUser,int role)
        {
            var userFromWeChat = _repositoryUsers.GetByOpenIDOrNull(wechatUser.openid, OpenSiteType.WeChat);
            var address=string.Format("{0} {1} {2}", wechatUser.city, wechatUser.privilege, wechatUser.country);
            var userEmail=string.Empty;
            if (userFromWeChat != null)
            {
                userFromWeChat.FirstName = wechatUser.nickname;
                userFromWeChat.Address = address;
                userFromWeChat.Sex = wechatUser.sex.ToLower() == "female" ? (byte)SexType.Female : (byte)SexType.Male;
                userFromWeChat.AvatarPath = wechatUser.headimgurl;
                userFromWeChat.ModifyTime = DateTime.UtcNow;
                _repositoryUsers.UserUpdate(userFromWeChat);
                userEmail = userFromWeChat.Email;
            }
            else
            {
                JSRegisterModel model = new JSRegisterModel()
                {
                    loginId = wechatUser.openid,
                    loginType = (byte)OpenSiteType.WeChat,
                    firstName = wechatUser.nickname,
                    lastName = string.Empty,
                    email = string.Empty,
                    password = string.Empty,
                    location = address,
                    avatar = wechatUser.headimgurl,
                    Role=role,
                };
                JSRegister(model);
                userEmail = model.email;
            }
            var logonModel = new LogOnModel
            {
                Email = userEmail,
                Password = Security._DefaultPswd,
                SelectedPage = LayoutSelectedPage.Account,
                ReturnUrl = string.Empty,
                RememberMe = true,
                From = (int)LogonMode.NormalLogon,
                Lan = GetCurrentLanguage(),
            };
            return LogOn(logonModel);
        }
        public ActionResult LogOnGoogle()
        {
            OpenIdAjaxRelyingParty rp = new OpenIdAjaxRelyingParty();
            var r = rp.GetResponse();
            if (r != null)
            {
                switch (r.Status)
                {
                    case AuthenticationStatus.Authenticated:
                        Session["GoogleIdentifier"] = r.ClaimedIdentifier.ToString();

                        break;
                    case AuthenticationStatus.Canceled:
                        return RedirectToAction("LogOn");
                }
            }
            return RedirectToAction("Boards");
        }

        [HttpPost]
        public void LogOnGoogle(LogOnModel model)
        {
            string discoveryUri = "https://www.google.com/accounts/o8/id";
            OpenIdRelyingParty openid = new OpenIdRelyingParty();
            var b = new UriBuilder(Url.Action("Boards")) { Query = "" };
            var req = openid.CreateRequest(discoveryUri, b.Uri, b.Uri);
            req.RedirectToProvider();
            return;
        }


        private void DoLogOut()
        { 
            var cacheManager = new OutputCacheManager();
            var userId = _security.GetCurrentUserId();
            cacheManager.RemoveItem("Home", "Header", new { id = userId.ToString() });
            FormsAuthentication.SignOut();
            _repositoryUsers.UserRemoveCachedUser(userId);
        }
        //
        // GET: /Account/LogOff

        public ActionResult LogOut()
        {
            DoLogOut();
            return RedirectToAction("LogOn", "Account");
        }
        public ActionResult MLogOut()
        {
            FormsAuthentication.SignOut();
            return RedirectToAction("MLogon", "Account");
        }
        public ContentResult LogOnPop(string id,int from)
        {
            var qscoll = HttpUtility.ParseQueryString(Request.UrlReferrer.Query);
            var model = new LogOnModel
            {
                SelectedPage = LayoutSelectedPage.Account,
                Email = id,
                ReturnUrl = Request.UrlReferrer.AbsoluteUri,
                InviteToken = qscoll["token"] ?? string.Empty,
                From=from,
                HoldId = id,
                Lan = GetCurrentLanguage(),

            };
            if (from == (int)LogonMode.JoinFavoriteLogon)
            {
                model.Email = string.Empty;
            }
            var html = RenderRazorViewToString("LogOnPop", model);
            return Content(html);
        }
        public ContentResult RegisterPop(string id, int from)
        {
            // Parse the query string variables into a NameValueCollection.
            var qscoll = HttpUtility.ParseQueryString(Request.UrlReferrer.Query);
            var model = new RegisterModel
            {
                SelectedPage = LayoutSelectedPage.Account,
                Email=id,
                ReturnUrl = Request.UrlReferrer.AbsoluteUri,
                InviteToken = qscoll["token"]??string.Empty,
                SendConfirmEmail = qscoll["token"]==null,
                From = from,
                HoldId=id,
                Language = GetCurrentLanguage(),

            };
            if (from == (int)LogonMode.JoinFavoriteLogon)
            {
                model.Email = string.Empty;
            }
            var html = RenderRazorViewToString("RegisterPop", model);
            return Content(html);
        }
      
        //
        // GET: /Account/Register
        [DonutOutputCache(CacheProfile = "OneDay")]
        public ActionResult Register(string returnUrl, Nullable<int> role)
        {
            if (_security.GetCurrentUserId() > 0)
            {
                return RedirectToAction("TourProducts", "Guide");
            }
            var model = new RegisterModel
            {
                IfWantNewsletter = true,
                SelectedPage = LayoutSelectedPage.Account,
                ReturnUrl = returnUrl,
                SendConfirmEmail = true,
                FirstName = "",
                Language = GetCurrentLanguage(),
                WeChatUrl = new WeChatHelper().GetLoginUrl(role ?? 1),
                Role=(role??1)
            };
            return View(InitLayout(model));
        }
        [Authorize]
        public ActionResult BecomeGuider()
        {
            try
            {
                int userId=_security.GetCurrentUserId();
                if (userId<=0)
                {
                    return RedirectToAction("InternalError", "Static", null);
                }
                _repositoryUsers.UserUpdateRole(userId,UserRole.Guider);
                return RedirectToAction("TourProducts", "Guide");
            }
            catch (Exception excp)
            {
                _logger.LogError(excp);
                return RedirectToAction("InternalError", "Static", null);
            }

        }
        [HttpPost]
        public ActionResult Register(RegisterModel model)
        {
            //bool isCommunityLogon = false;// IsCommunityClient(); 
            try
            {
             
                LogonMode from = (LogonMode)model.From;
                var userBrowser = string.Empty;
                bool validateEmail = true;
                var userAgent = Request.UserAgent;
                string originalEmail = model.Email;
                //if (isCommunityLogon)
                //{
                //    model.SendConfirmEmail = false;
                //}
                //else
                //{
                    model.SendConfirmEmail = true;
                //}
                if (userAgent.Contains("iPhone"))
                {
                    from = LogonMode.iPhoneLogon;
                }
                else if (userAgent.Contains("Android"))
                {
                    from = LogonMode.AndroidMobileLogon;
                }

                userBrowser = CheckUserAgentLength(Request.UserAgent);
                validateEmail = true;
                Validators.ValidRegisterEmailUserNamePassword(_repositoryUsers, ModelState, _configuration,model.Email, model.FirstName, model.Password, model.NickName, validateEmail);
                if (model.Password != model.ConfirmPassword)
                {
                    ModelState.AddModelError("Password", ValidationStrings.PasswordsMustMatch);                
                }
                if (ModelState.IsValid)
                {
                    try
                    {
                        var userIp = RequestHelper.GetClientIpAddress(Request);
                        UserRole role = (UserRole)model.Role;
                      
                        var page = _commands.Register(new UserRegInfo()
                        {
                            FirstName = model.FirstName,
                            LastName = model.LastName,
                            Email = model.Email,
                            Password = model.Password,
                            Title = string.Empty,
                            AreaCode = string.Empty,
                            LocalCode = string.Empty,
                            RequireConfirmationToken = model.SendConfirmEmail,
                            UserIp = userIp,
                            UserBrowser = userBrowser,
                            NickName = model.NickName,
                            Role = (short)role,
                            Language = (byte)GetCurrentLanguage(),
                            ReturnUrl = model.ReturnUrl
                        });

                        if (model.IfWantNewsletter)
                            MailChimpServices.SubscribeToMailChimpList(model.Email, string.Format("{0} {1}", model.FirstName, model.LastName));

                        var userGuid = SafeConvert.ToInt32(page.Parameters);
                        if (Request.IsAjaxRequest())
                            return Json(new
                            {
                                result = true,
                                html = page.PlainUrl.Trim()
                            });


                        //if (string.IsNullOrEmpty(page.PlainUrl))
                        //    return RedirectToPage(PageFlow.ActivationEmailSend);

                        //Activate User
                        _repositoryUsers.ActivateUserByPassConfirmationToken(model.Email);
                        //Autotically logon
                        var returnModel = _commands.LogOn(model.Email, model.Password, true, model.ReturnUrl, userIp, userBrowser);
                        if (!string.IsNullOrEmpty(model.ReturnUrl))
                        {
                            return Redirect(model.ReturnUrl);//Redirect to previous page
                        }

                        return RedirectToPage(page);
                    }
                    catch (SecurityException e)
                    {
                        ModelState.AddModelError(string.Empty, e.Message);
                    }
                }
                //if (!isCommunityLogon && (from == LogonMode.AndroidMobileLogon || from == LogonMode.iPhoneLogon || (int)from >= 100))
                //    return Json(new
                //    {
                //        Result = _ContentResultFailed,
                //        Message = string.Join(";", ModelState.Values
                //                    .SelectMany(x => x.Errors)
                //                    .Select(x => x.ErrorMessage)),
                //    });
                // If we got this far, something failed, redisplay form
                if (Request.IsAjaxRequest())
                    return Json(new
                    {
                        result = false,
                        html = RenderRazorViewToString("RegisterPop", model)
                    });
                //Show validation error message to user.
                model.Email = originalEmail;
                //if(!isCommunityLogon)
                    return View("Register", InitLayout(model));
                //else
                //    return View("MRegister", InitLayout(model));
 
            }
            catch (Exception excp)
            {
                _logger.LogError(excp);
                //if (!isCommunityLogon)
                    return RedirectToAction("LogOn");
                //else
                //    return RedirectToAction("MLogOn");
            }
        }

        private void SetChangeNameModelAdditional(User model)
        {
      
        }
        //
        // GET: /Account/ChangePassword

        [Authorize]
        public ActionResult ChangePassword()
        {
            var model = new ChangePasswordModel
            {
                OldPassword=string.Empty,
                NewPassword=string.Empty,
                ConfirmPassword=string.Empty
            };
            return View("ChangePassword", InitLayout(model));
        }

        //
        // POST: /Account/ChangePassword

        [Authorize]
        [HttpPost]
        public ActionResult ChangePassword(ChangePasswordModel model)
        {
            try
            {
                User user = null;
                if (string.IsNullOrEmpty(model.NewPassword) || !_configuration.Password.ValidationRegex.IsMatch(model.NewPassword))
                    ModelState.AddModelError("NewPassword", _configuration.Password.ValidationMessage);
                if (model.NewPassword != model.ConfirmPassword)
                    ModelState.AddModelError("ConfirmPassword", "Passwords should match");
                if (ModelState.IsValid)
                {
                    user = _repositoryUsers.GetByIdOrNull(_security.GetCurrentUserId());
                    //_security.ValidateThatUserCanSignIn(user, model.OldPassword);

                    var commands = _commandsFactory();
                    var hashedPassword = Crypto.HashPassword(model.NewPassword);
                    user.Password = hashedPassword;
                    commands.UserUpdate(user);
                    return RedirectToAction("ChangePasswordSuccess");
                }
                return View("ChangePassword", InitLayout(model));
            }
            catch (Exception excp)
            {
                _logger.LogError(excp);
                return RedirectToAction("InternalError", "Static", null);
            }

        }

        //
        // GET: /Account/ChangePasswordSuccess

        public ActionResult ChangePasswordSuccess()
        {
            var model = new ChangePasswordSuccessModel
            {

            };
            return View(InitLayout(model));
        }

        //[Authorize]
        [HttpPost]
        public ContentResult SaveUserInfo(SaveUserInfoModel model)
        {
            try
            {
                var userId = _security.GetCurrentUserId();
                var user = _repositoryUsers.GetByIdOrNull(userId);
                if (user == null)
                    return Content(_ContentResultFailed);

                return Content(_ContentResultOK);
            }
            catch (Exception ex)
            {
                _logger.LogError(string.Format("SaveUserInfo() GOT exception message:{0}.",ex.Message));
            }
            return Content(_ContentResultFailed);        
        }
        public ContentResult GlobalUsers(int id)
        {
            var model = new GlobalUsersModel
            {
                CurrentUsers = _repositoryUsers.GetGlobalUsers(),
                NewUser = new User { 
                    RealRole=UserRole.Tourist,
                    RoleOptions=UserGetRoleOptions(),
                    Email="@prophecycoal.com"
                },
                IsCreate=true
            };
            if (id>0)
            {
                var editUser=_repositoryUsers.GetByIdOrNull(id);
                if (editUser != null)
                {
                    model.NewUser = editUser;
                    model.IsCreate = false;
                    model.NewUser.RoleOptions = UserGetRoleOptions();
                }
            }
            var html = RenderRazorViewToString("GlobalUsers", model);
            return Content(html);
        }
        [HttpPost]
        public ContentResult GlobalUsers(GlobalUsersModel model)
        {
            if (string.IsNullOrWhiteSpace(model.NewUser.Email))
                ModelState.AddModelError("NewUser.Name", "User name cannot be empty");

            if (string.IsNullOrWhiteSpace(model.NewUser.Email))
                ModelState.AddModelError("NewUser.Email", "User email address cannot be empty");

            if (model.IsCreate)
            {
                if (string.IsNullOrWhiteSpace(model.NewUser.Password))
                    ModelState.AddModelError("NewUser.Password", "User password cannot be empty");
                else if (!_configuration.Password.ValidationRegex.IsMatch(model.NewUser.Password))
                    ModelState.AddModelError("NewUser.Password", _configuration.Password.ValidationMessage);
            }
            if (!EmailHelper.IsEmailAddressValid(model.NewUser.Email))
                ModelState.AddModelError("NewUser.Email", "Email is not valid");
            var queries = _queriesFactory();
            if (ModelState.IsValid)
            {
                try
                {

                    if (!model.IsCreate)
                    {
                        var oldUser = _repositoryUsers.GetByEmailOrNull(model.NewUser.Email);
                        if (oldUser != null)
                        {
                            oldUser.FirstName = model.NewUser.FirstName;
                            oldUser.LastName = model.NewUser.LastName;
                            oldUser.Email = model.NewUser.Email;
                            oldUser.Title = model.NewUser.Title ?? string.Empty;
                            oldUser.PhoneLocalCode = model.NewUser.PhoneLocalCode;
                            if (!string.IsNullOrEmpty(model.NewUser.Password))
                                oldUser.Password = Crypto.HashPassword(model.NewUser.Password);
                            oldUser.Role = model.NewUser.Role;
                            _commandsFactory().UserUpdate(oldUser);
                        }
                        else
                            throw new Exception("The user does NOT exist!");
                    }
                    else
                    {
                        var user = new User
                        {

                            FirstName = model.NewUser.FirstName,
                            LastName = model.NewUser.LastName,
                            Email = model.NewUser.Email,
                            Title = model.NewUser.Title ?? string.Empty,
                            PhoneLocalCode = model.NewUser.PhoneLocalCode,
                            Password = model.NewUser.Password,
                            Role = model.NewUser.Role

                        };
                        //_commandsFactory().CreateMember(user, null);
                    }
                    //var newModel = new GlobalUsersModel
                    //{
                    //    CurrentUsers = _repositoryUsers.GetGlobalUsers(),
                    //    NewUser = new User
                    //    {
                    //        
                    //        RealRole = UserRole.Member,
                    //        RoleOptions = UserGetRoleOptions(),
                    //        Email = "@prophecycoal.com",
                    //        Password=string.Empty,
                    //        Name=string.Empty,
                    //        Title=string.Empty,
                    //        PhoneLocalCode=string.Empty
                    //    },
                    //    IsCreate=true,
                    //};
                    //var html = RenderRazorViewToString("GlobalUsers", newModel);
                    return Content("OK");
                }
                catch (Exception e)
                {
                    ModelState.AddModelError(string.Empty, e.Message);
                }
            }
            model.CurrentUsers = _repositoryUsers.GetGlobalUsers();
            model.NewUser.RealRole = UserRole.Tourist;
            model.NewUser.RoleOptions = UserGetRoleOptions();
            model.NewUser.Email = "@prophecycoal.com";
            var errorHtml = RenderRazorViewToString("GlobalUsers", model);
            return Content(errorHtml);
        }

        [HttpPost]
        public ContentResult DestroyUser(int id)
        {
            //_repositoryUsers.UserDestroyByID(id);
            var newModel = new GlobalUsersModel
            {
                CurrentUsers = _repositoryUsers.GetGlobalUsers(),
                NewUser = new User
                {
                    RealRole = UserRole.Tourist,
                    RoleOptions = UserGetRoleOptions(),
                    Email = "@prophecycoal.com"
                },
                IsCreate=true
            };
            var html = RenderRazorViewToString("GlobalUsers", newModel);
            return Content(html);
        }
        public ActionResult ForgotPassword()
        {
            var model = new ForgotPassword
            {
                Username = "",
                Email = "",
                SelectedPage = LayoutSelectedPage.Account,
            };
            return View(InitLayout(model));
        }
        [HttpPost]
        public ActionResult ForgotPassword(ForgotPassword model)
        {
            if (!EmailHelper.IsEmailAddressValid(model.Email))
                ModelState.AddModelError("Email", "Please input valid Email.");
            if (ModelState.IsValid)
            {
                try
                {
                    if (!_commands.SendForgotPassword(model.Username, model.Email))
                        throw new ApplicationException("Password is not valid，Please input valid Email.");
                    return RedirectToPage(PageFlow.ForgotPasswordSent);
                }
                catch (ApplicationException e)
                {
                    ModelState.AddModelError(string.Empty, e.Message);
                }
            }
            model.SelectedPage = LayoutSelectedPage.Account;
            return View(InitLayout(model));
        }
        public ActionResult ResetPassword(string id)
        {
            var model = new ResetPassword
            {
                NewPassword = "",
                ConfirmNewPassword = "",
                Token = id,
                SelectedPage = LayoutSelectedPage.Account,
            };
            return View(InitLayout(model));
        }
        [HttpPost]
        public ActionResult ResetPassword(ResetPassword model)
        {
            if (string.IsNullOrEmpty(model.NewPassword) || string.IsNullOrEmpty(model.ConfirmNewPassword)
                || model.NewPassword != model.ConfirmNewPassword)
                ModelState.AddModelError("ConfirmNewPassword", " Password and Re-entered Password do not match. Please try again.");
            else
            {
                if (!_configuration.Password.ValidationRegex.IsMatch(model.ConfirmNewPassword))
                    ModelState.AddModelError("ConfirmNewPassword", _configuration.Password.ValidationMessage);
                else
                {
                    if (string.IsNullOrEmpty(model.Token))
                        ModelState.AddModelError("ConfirmNewPassword", "Your reset password token is incorrect");
                }
            }
            if (ModelState.IsValid)
            {
                try
                {
                    if (_commands.ResetPasswordWithToken(model.Token, model.NewPassword))
                        return RedirectToPage(PageFlow.PasswordChanged);
                    else
                        //ModelState.AddModelError("ConfirmNewPassword", "抱歉,密码重置失败.你的密码修改必须在申请后24小时内完成,本次邮件只能修改一次密码.");
                        ModelState.AddModelError("ConfirmNewPassword", "Sorry. Password reset failed. Your password changes must be completed within 24 hours after the application.");
                }
                catch (ApplicationException e)
                {
                    ModelState.AddModelError("ConfirmNewPassword", e.Message);
                }
            }
            model.SelectedPage = LayoutSelectedPage.Account;
            return View(InitLayout(model));
        }
        public ActionResult ResendConfirmation()
        {
            var model = new ResendConfirmation
            {
                Username = "",
                Email = "",
                SelectedPage = LayoutSelectedPage.Account
            };
            return View(InitLayout(model));
        }
        [HttpPost]
        public ActionResult ResendConfirmation(ResendConfirmation model)
        {
            if (!EmailHelper.IsEmailAddressValid(model.Email) && (string.IsNullOrEmpty(model.Username) || model.Username.Length > 50))
                ModelState.AddModelError("Email", "邮件地址和用户名必须至少有一个是正确的.");
            if (ModelState.IsValid)
            {
                try
                {
                    _commands.SendResendConfirmation(model.Username, model.Email);
                    return RedirectToPage(PageFlow.ResendConfirmationSent);
                }
                catch (ApplicationException e)
                {
                    ModelState.AddModelError(string.Empty, e.Message);
                }
            }
            model.SelectedPage = LayoutSelectedPage.Account;
            return View(InitLayout(model));
        }
        #region Status Codes
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
        #endregion
        [Authorize]
        [HttpPost]
        public JsonResult CMyInfo()
        {
            try
            {
                var userId = _security.GetCurrentUserId();
                if (userId <= 0)
                    return Json(new
                    {
                        Result = false
                    });
                var user = _repositoryUsers.UserGetCurrentUser(userId);
                if (user == null)
                    return Json(new
                    {
                        Result = false
                    });
                LanguageCode lan = GetCurrentLanguage();
                user.SexOptions = SelectOptionsHelper.CreateOptions((int)SexType.Male,
                    (int)SexType.Female,
                    lan == LanguageCode.All ? EnumTranslation.SexTypeCn : EnumTranslation.SexTypeEn);
                var html = RenderRazorViewToString("CMyInfo", user);
                return Json(new
                {
                    Html = html,
                    Prev = "",
                    Next = "",
                    Result = true
                });
            }
            catch (Exception excp)
            {
                _logger.LogError(excp);
            }
            return Json(new
            {
                Result = false
            });
        }
        [Authorize]
        [HttpPost]
        public JsonResult CChangePassword()
        {
            var model = new ChangePasswordModel
            {
                OldPassword = string.Empty,
                NewPassword = string.Empty,
                ConfirmPassword = string.Empty
            };
            var html = RenderRazorViewToString("CChangePassword", model);
            return Json(new
            {
                Html = html,
                Prev = "",
                Next = "",
                Result = true
            });
        }

        public ActionResult ConfirmEmail(DoneRedirect redirect)
        {
            var model = new ConfirmEmailModel
            {
                Email = string.Empty,
                DoneControllerName = redirect.DoneControllerName,
                DoneActionName = redirect.DoneActionName
            };
            
            return View(InitLayout(model));
        }
        [HttpPost]
        public ActionResult ConfirmEmail(ConfirmEmailModel model)
        {
            try
            {
                bool isValid = Validators.ValidEmailAndCheckDB(_repositoryUsers,ModelState,model.Email, "Email");
                if (isValid && ModelState.IsValid)
                {
                    var user = _repositoryUsers.UserUpdateComfirmationToken(_security.GetCurrentUserId());
                    user.Email = model.Email;
                    _commands.SendConfirmationEmail(user, model.Email, user.ConfirmationToken.Substring(0,8));

                    return RedirectToAction("ConfirmEmailSend", new { DoneActionName = model.DoneActionName, DoneControllerName = model.DoneControllerName });
                }
                return View("ConfirmEmail", InitLayout(model));
            }
            catch (Exception excp)
            {
                _logger.LogError(excp);
                return RedirectToAction("InternalError", "Static", null);
            }

        }
        public ActionResult ConfirmEmailSend(DoneRedirect redirect)
        {
            var model = new ConfirmEmailSendModel
            {
                DoneRedirect = redirect
            };

            return View(InitLayout(model));
        }
        public ActionResult RememberNewPassword(string email,string pswd)
        {
            try
            {
                RememberNewPasswordModel model = new RememberNewPasswordModel()
                {
                    Email = email,
                    NewPassword=pswd,
                };
                return View("RememberNewPassword", InitLayout(model));
            }
            catch (Exception excp)
            {
                _logger.LogError(excp);
                return RedirectToAction("InternalError", "Static", null);
            }

        }
        public ActionResult ConfirmPhone(DoneRedirect redirect)
        {
            try
            {
                var user=_repositoryUsers.GetByIdOrNull(_security.GetCurrentUserId());
                if (user == null)
                {
                    return RedirectToAction("LogOn", new {returnUrl=string.Empty,role=0});
                }
                var model = new ConfirmPhoneModel
                {
                    PhoneNumber = user.Mobile,
                    AreaCode = string.IsNullOrEmpty(user.PhoneAreaCode)?"1":user.PhoneAreaCode,
                    AreaCodeOptions = _repositoryCities.GetPhoneCodesSelectListItemCached(),
                    DoneControllerName = redirect.DoneControllerName,
                    DoneActionName = redirect.DoneActionName
                };
                return View("ConfirmPhone", InitLayout(model));
            }
            catch (Exception excp)
            {
                _logger.LogError(excp);
                return RedirectToAction("InternalError", "Static", null);
            }
        }
        [HttpPost]
        public ActionResult ConfirmPhone(ConfirmPhoneModel model)
        {
            try
            {
                Validators.ValidPhoneNumber(_repositoryUsers, ModelState, model.PhoneNumber, "PhoneNumber");
                if (ModelState.IsValid)
                {
                    var digits=TourHelper.GenerateRandom4DigitString();
                    Session.Add(_SessionConfirmPhone4Digits,digits);
                    Session.Add(_SessionConfirmingPhoneNumber, model.PhoneNumber);
                    Session.Add(_SessionConfirmingPhoneAreaCode, model.AreaCode);
                    TourHelper.SendSMS(model.AreaCode,model.PhoneNumber, string.Format("Your verification number is {0},please input these 4 digits in the next step.  Sincerely Kootour", digits));
                    return RedirectToAction("ConfirmPhoneSendDigit", new { DoneActionName = model.DoneActionName, DoneControllerName = model.DoneControllerName });
                }
                model.AreaCodeOptions = _repositoryCities.GetPhoneCodesSelectListItemCached();
                return View("ConfirmPhone", InitLayout(model));
            }
            catch (Exception excp)
            {
                _logger.LogError(excp);
                return RedirectToAction("InternalError", "Static", null);
            }

        }
        public ActionResult ConfirmPhoneSendDigit(DoneRedirect redirect)
        {
            try
            {
                var model = new ConfirmPhoneSendDigitModel
                {
                    DoneControllerName = redirect.DoneControllerName,
                    DoneActionName = redirect.DoneActionName
                };
                return View(InitLayout(model));
            }
            catch (Exception excp)
            {
                _logger.LogError(excp);
                return RedirectToAction("InternalError", "Static", null);
            }

        }
        [HttpPost]
        public ActionResult ConfirmPhoneSendDigit(ConfirmPhoneSendDigitModel model)
        {
            try
            {
                var digits=Session[_SessionConfirmPhone4Digits];
                if (digits == null || digits.ToString() != model.DigitCode)
                {
                    ModelState.AddModelError("DigitCode", "Wrong number!Please check the text message on your mobile phone and make sure your phone number is right.");
                }
                if (ModelState.IsValid)
                {
                    var areaCode = Session[_SessionConfirmingPhoneAreaCode];
                    var phoneNumber = Session[_SessionConfirmingPhoneNumber];
                    var user = _repositoryUsers.UserGetCurrentUser(_security.GetCurrentUserId());
                    user.IsPhoneConfirmed = true;
                    user.Mobile = phoneNumber.ToString();
                    user.PhoneAreaCode = areaCode.ToString();
                    user.ModifyTime = DateTime.UtcNow;
                    _repositoryUsers.UserUpdate(user);
                    Session.Remove(_SessionConfirmingPhoneNumber);
                    Session.Remove(_SessionConfirmPhone4Digits);
                    Session.Remove(_SessionConfirmingPhoneAreaCode);
                    return RedirectToAction("ConfirmPhoneDone", new { DoneActionName = model.DoneActionName, DoneControllerName = model.DoneControllerName });
                }
                return View(InitLayout(model));
            }
            catch (Exception excp)
            {
                _logger.LogError(excp);
                return RedirectToAction("InternalError", "Static", null);
            }
        }
        public ActionResult ConfirmPhoneDone(DoneRedirect redirect)
        {
            var model = new ConfirmPhoneDoneModel
            {
                DoneRedirect = redirect
            };

            return View(InitLayout(model));
        }
    }
}
