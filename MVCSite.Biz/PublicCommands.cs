using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using WMath.Facilities;
using MVCSite.Biz.Interfaces;
using MVCSite.DAC.Instrumentation;
using System.Web.Security;
using System.Web;
using System.Security;
using MVCSite.DAC.Common;
using MVCSite.DAC.Interfaces;
using MVCSite.DAC.Entities;
using MVCSite.DAC.Instrumentation.Membership;
using DevTrends.MvcDonutCaching;
using MVCSite.Common;

namespace MVCSite.Biz
{
    class PublicCommands : CommandsBase, IPublicCommands
    {
        private readonly ISecurity _securityService;
        private readonly IWebApplicationContext _webContext;
        private readonly IEmailer _emailer;
        private readonly EmailGenerator _emailGenerator;
        private readonly IRepositoryUsers _repositoryUsers;
        private readonly ISiteConfiguration _configuration;

        private readonly PageFlow _pageFlow;
        private readonly IRepository _repository;


        public PublicCommands(ISecurity securityService,
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
        public  void LogOnSaveCookies(User user, bool rememberMe)
        {
            var cookie = CreateCookie(user.ID, user.Email, rememberMe, user.Initials, user.FullName);
            HttpContext.Current.Response.SetCookie(cookie);

            var isMemberCookie = new HttpCookie(Constants.CookieIsMember, "1");
            isMemberCookie.Expires = DateTime.UtcNow + TimeSpan.FromDays(99999);
            HttpContext.Current.Response.SetCookie(isMemberCookie);
            return;
        }
        public LogonReturnModel LogOn(string userNameOrEmail, string password, bool rememberMe, string redirectUrl, string userIp, string userBrowser)
        {
            var user = _repositoryUsers.GetByEmailOrNull(userNameOrEmail);
            if (user == null)
                //throw new SecurityException("UserLogonIDWrong");
                throw new SecurityException("Wrong username or password. Please try again.");

            _securityService.ValidateThatUserCanSignIn(user, password);
            LogOnSaveCookies(user, rememberMe);
            var lastLoginTime = user.LastLoginTime;
            bool isFirstTimeToday=false;
            if (lastLoginTime.HasValue&&(DateTime.UtcNow - lastLoginTime.Value).TotalHours >= 24)
            {
                isFirstTimeToday=true;
            }
            user.LastLoginTime = DateTime.UtcNow;
            user.SignUpIP = userIp;
            user.SignUpBrowser = userBrowser;

            _repositoryUsers.UserUpdateLogonInfo(user);
            if (!string.IsNullOrEmpty(redirectUrl))
                return new LogonReturnModel(){ 
                    Page=new WebsitePage { PlainUrl = redirectUrl },
                    IsFirstTimeToday = isFirstTimeToday
                };
            return new LogonReturnModel(){ 
                Page=PageFlow.WordBooks,
                IsFirstTimeToday = isFirstTimeToday
            };
            
        }
        public void CreateInviteeCookie(string email, string token)
        {
            var cookie = new HttpCookie(Constants.CookieInviteeEmail, email);
            HttpContext.Current.Response.Cookies.Add(cookie);
            cookie = new HttpCookie(Constants.CookieInviteeToken, token);
            HttpContext.Current.Response.Cookies.Add(cookie);
        }
        //public WebsitePage Register(string userName, string email,string password, string returnUrl, string userIp, string userBrowser,
        //    UserLanguage lan, bool sendConfirmEmail = true, string nickName = "")
        public WebsitePage Register(UserRegInfo regInfo)
        {
            if (!_configuration.Password.ValidationRegex.IsMatch(regInfo.Password))
                throw new SecurityException(_configuration.Password.ValidationMessage);

            //if (password != confirmPassword)
            //    throw new SecurityException("Passwords should match");

            if (!EmailHelper.IsEmailAddressValid(regInfo.Email))
                throw new SecurityException(string.Format("Email address ({0}) is not valid", regInfo.Email));

            var emailUser = _repositoryUsers.GetByEmailOrNull(regInfo.Email);
            if (emailUser != null)
                throw new SecurityException(string.Format("User with email address ({0}) specified already exists",regInfo.Email));

            //var usernameUser = _repositoryUsers.GetByEmailOrNull(regInfo.FirstName);
            //if (usernameUser != null)
            //    throw new SecurityException("User with user name specified already exists");
            regInfo.Email = FixCommonEmailTypos(regInfo.Email);

            //var regInfo = new UserRegInfo()
            //{
            //    UserName = userName,
            //    Password = password,
            //    Email = email,
            //    Title = string.Empty,
            //    AreaCode = string.Empty,
            //    LocalCode = string.Empty,
            //    RequireConfirmationToken = sendConfirmEmail,
            //    UserIp = userIp,
            //    UserBrowser = userBrowser,
            //    NickName = nickName,
            //    Role = 0,
            //    Language = (byte)lan
            //};
            var user = new CustomMembershipProvider().CreateAccount(regInfo);
            if (!regInfo.RequireConfirmationToken)
            {
                var member = _repositoryUsers.GetByEmailOrNull(regInfo.Email);
                if (member == null)
                    throw new SecurityException("Logon failed:invalid email!");
                LogOnSaveCookies(member, true);
                return new WebsitePage { PlainUrl = regInfo.ReturnUrl, Parameters = member.ID };
            }
#if RELEASE
            SendConfirmationEmail(user, regInfo.ReturnUrl);
#endif
            return new WebsitePage { PlainUrl = regInfo.ReturnUrl, Parameters = user.ID };
        }
        public void SendConfirmationEmail(User user,string url,string pswd="")
        { 
            var emailActivationToken = user.ConfirmationToken;
            var confirmationLink = _webContext.ServerUrl +
                _webContext.UrlHelper.Action("ActivateUser", "Account", new { userId = user.ID, token = emailActivationToken, returnUrl = url });
            EmailConfirmation emailVariables = new EmailConfirmation();
            emailVariables.ConfirmationLink = confirmationLink;
            emailVariables.UserName = user.Email;
            emailVariables.Password = pswd;
            var emailBody = _emailGenerator.GetConfirmationString(emailVariables);
            _emailer.SendHtml(_configuration.DontReplyEmailAddress, user.Email, "Welcome to Kootour", emailBody);
        }
        public void SendUserContactCSREmail(string toEmail, EmailUserContactCSR model)
        {
            var emailBody = _emailGenerator.GetEmailUserContactCSRString(model);
            _emailer.SendHtml(_configuration.DontReplyEmailAddress, toEmail, "Service request from customer", emailBody);
        }
        public bool SendForgotPassword(string username, string email)
        {
            string token = Guid.NewGuid().ToString();
            User currentUser = null;
            if (email != string.Empty)
                currentUser = _repositoryUsers.GetByEmailOrNull(email);
            if (currentUser == null && username != string.Empty)
                currentUser = _repositoryUsers.GetByEmailOrNull(username);
            if (currentUser == null)
                throw new ApplicationException("Can NOT find the user by email or username.");
            _repositoryUsers.SetForgotPasswordToken(currentUser.ID, token);

            var confirmationLink = _webContext.ServerUrl + _webContext.UrlHelper.Action("ResetPassword", "Account",new{ id=token });
            var emailBody = _emailGenerator.GetForgotPasswordString(new EmailForgotPassword
            {
                UserName = currentUser.Email,
                ConfirmationLink = confirmationLink
            });
            _emailer.SendHtml(_configuration.DontReplyEmailAddress, currentUser.Email, "Reset your Kootour password", emailBody);
            return true;
        }
        public bool SendResendConfirmation(string username, string email)
        {
            var token = Guid.NewGuid().ToString();
            User currentUser = null;
            if (email != string.Empty)
                currentUser = _repositoryUsers.GetByEmailOrNull(email);
            if (currentUser == null && username != string.Empty)
                currentUser = _repositoryUsers.GetByEmailOrNull(username);
            if (currentUser == null)
                throw new ApplicationException("无法找到用户，你输入的用户名或邮件地址不正确.");
            if (currentUser.IsConfirmed)
                throw new ApplicationException("抱歉，你的帐号已被激活，我们无法再次给你发送注册确认邮件.");
            _repositoryUsers.SetForgotPasswordToken(currentUser.ID, token);//We share the same token field in user table
            var confirmationLink = _webContext.ServerUrl + _webContext.UrlHelper.Action("ActivateUser", "Account",
                new { userId = currentUser.ID, token = token });
            EmailConfirmation emailVariables = new EmailConfirmation();
            emailVariables.ConfirmationLink = confirmationLink;
            var emailBody = _emailGenerator.GetConfirmationString(emailVariables);
            _emailer.SendHtml(_configuration.DontReplyEmailAddress, currentUser.Email, "欢迎注册微教室-再次发送注册确认邮件", emailBody);
            return true;
        }


        
        public void SendBookingConfirmationEmail(BookingConfirmationModel model)
        {
            var tourShortDescription = string.Format(" -  {0}  -  {1}", model.City, model.TourName);

            //1.Send to Kootour
            var AccountManagerEmailBody = _emailGenerator.GetAccountManagerBookingConfirmationEmailString(model);
            _emailer.SendHtml(_configuration.DontReplyEmailAddress, "contact@kootour.com", "Booking Confirmation" + tourShortDescription, AccountManagerEmailBody);
            _emailer.SendHtml(_configuration.DontReplyEmailAddress, "wkevin@kootour.com", "Booking Confirmation" + tourShortDescription, AccountManagerEmailBody);

            //2.Send to Traveler
            var travelerEmailBody = _emailGenerator.GetTravelerBookingConfirmationEmailString(model);
            _emailer.SendHtml(_configuration.DontReplyEmailAddress, model.TravelerEmail, "Booking Confirmation" + tourShortDescription, travelerEmailBody);

            //3.Send to Guide
            var guideEmailBody = _emailGenerator.GetGuideBookingConfirmationEmailString(model);
            _emailer.SendHtml(_configuration.DontReplyEmailAddress, model.GuideEmail, "Your Tour has been booked" + tourShortDescription, guideEmailBody);
        }

        //For testing confirmation emails only
        //public void SendBookingConfirmationEmail(BookingConfirmationModel model)
        //{
        //    var tourShortDescription = string.Format(" -  {0}  -  {1}", model.City, model.TourName);

        //    //1.Send to Kootour
        //    var AccountManagerEmailBody = _emailGenerator.GetAccountManagerBookingConfirmationEmailString(model);
        //    _emailer.SendHtml(_configuration.DontReplyEmailAddress, "contact@kootour.com", "Booking Confirmation" + tourShortDescription, AccountManagerEmailBody);
        //    _emailer.SendHtml(_configuration.DontReplyEmailAddress, "puyuew@gmail.com", "Booking Confirmation" + tourShortDescription, AccountManagerEmailBody);

        //    //2.Send to Traveler
        //    var travelerEmailBody = _emailGenerator.GetTravelerBookingConfirmationEmailString(model);
        //    _emailer.SendHtml(_configuration.DontReplyEmailAddress, "puyuew@gmail.com", "Booking Confirmation" + tourShortDescription, travelerEmailBody);

        //    //3.Send to Guide
        //    var guideEmailBody = _emailGenerator.GetGuideBookingConfirmationEmailString(model);
        //    _emailer.SendHtml(_configuration.DontReplyEmailAddress, "puyuew@gmail.com", "Your Tour has been booked" + tourShortDescription, guideEmailBody);
        //}
        public bool ResetPasswordWithToken(string passwordToken, string password)
        {
            return _repositoryUsers.ResetPasswordWithToken(passwordToken, password);
        }

        bool IsUrlIsValidWebsiteUrl(string redirectUrl)
        {
            return _webContext.IsLocalUrl(redirectUrl)
                    && redirectUrl.Length > 1
                    && redirectUrl.StartsWith("/")
                    && !redirectUrl.StartsWith("//")
                    && !redirectUrl.StartsWith("/\\");
        }
        static string FixCommonEmailTypos(string email)
        {
            email = email.Replace("yaoo", "yahoo");
            email = email.Replace("yahoomail", "yahoo");
            email = email.Replace("yhaoo", "yahoo");
            email = email.Replace("yeahoo.com", "yahoo.com");
            email = email.Replace(".com.com", ".com");
            email = email.Replace("@rogers.ca", "@rogers.com");
            email = email.Replace("mirosoft.com", "microsoft.com");
            email = email.Replace("@hotmil", "@hotmail");
            email = email.Replace("@hoymail.", "@hotmail.");
            email = email.Replace("@hotmail.uk", "@hotmail.co.uk");
            email = email.Replace("@liva.ca", "@live.ca");
            email = email.Replace("@gmal.com", "@gmail.com");
            email = email.Replace("@gmial.com", "@gmail.com");
            email = email.Replace("@homail.", "@hotmail.");
            email = email.Replace("@hotmall.", "@hotmail.");
            email = email.Replace("@alo.com", "@aol.com");
            email = email.Replace("@a.o.l.com", "@aol.com");
            email = email.Replace("@ol.com", "@aol.com");
            email = email.Replace("@hotmal.com", "@hotmail.com");
            email = email.Replace("@hoymail.com", "@hotmail.com");
            email = email.Replace("@hotmail.com.au", "@hotmail.com");
            email = email.Replace("@homail.com", "@hotmail.com");
            email = email.Replace("@hotmai.com", "@hotmail.com");
            email = email.Replace("yahoo.net", "yahoo.com");
            email = email.Replace("yahou", "yahoo");
            email = email.Replace("yhoo", "yahoo");
            email = email.Replace("yaho.", "yahoo.");
            email = email.Replace("yohoo", "yahoo");
            email = email.Replace("/", ".");
            email = email.Replace("hotmial", "hotmail");
            email = email.Replace("hotamil", "hotmail");
            email = email.Replace("hotail", "hotmail");
            email = email.Replace("hotmale", "hotmail");
            email = email.Replace("hotai.", "hotmail");
            email = email.Replace("hotmmail", "hotmail");
            email = email.Replace("hotamail", "hotmail");
            email = email.Replace("hotmail.net", "hotmail.com");
            email = email.Replace("hotmail.org", "hotmail.com");
            email = email.Replace("*", string.Empty);
            email = email.Replace("www.", string.Empty);

            if (email.EndsWith("@yahoo.co")) email = email.Replace("@yahoo.co", "@yahoo.com");
            if (email.EndsWith(".con")) email = email.Replace(".con", ".com");
            if (email.EndsWith(".com.uk")) email = email.Replace(".com.uk", ".co.uk");
            if (email.EndsWith(".om")) email = email.Replace(".om", ".com");
            if (email.EndsWith("@hotmail.co")) email = email.Replace("@hotmail.co", "@hotmail.com");

            if (!EmailHelper.IsEmailAddressValid(email))
                throw new SecurityException("Email address is not valid");
            return email;
        }
        static string ErrorCodeToString(MembershipCreateStatus createStatus)
        {
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
        public decimal TryParseDecimalOrMinusOne(string value)
        {
            if (string.IsNullOrEmpty(value))
                return -1;

            var valueReg = new Regex(@"(\d+)", RegexOptions.IgnoreCase | RegexOptions.Singleline);
            var valueMatch = valueReg.Match(value);
            decimal doubleValue = -1;
            if (valueMatch.Success)
            {
                doubleValue = MVCSite.Common.SafeConvert.ToDecimal(valueMatch.Value, -1);
            }
            return doubleValue;
        }


    }

    public class EntityValidationError
    {
        public string Message;
        public string Field;
    }
}
