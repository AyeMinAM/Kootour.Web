


//using WMath.Engine.DomainEntities;

namespace MVCSite.Biz.Interfaces {

 
		public partial interface ICommunityCommands {
					void UpdateChatContent(int chatId);
		}
 
		public partial interface ICommunityQueries {
		}
 
		public partial interface IEmailer {
					void SendHtml(string from, string to, string subject, string body);
					void SendUserEmail(MVCSite.DAC.Entities.User userProfile, string to, string subject, string bodyHtml);
		}
 
		public partial interface IPublicCommands {
					void LogOnSaveCookies(MVCSite.DAC.Entities.User user, bool rememberMe);
					MVCSite.Biz.LogonReturnModel LogOn(string userNameOrEmail, string password, bool rememberMe, string redirectUrl, string userIp, string userBrowser);
					void CreateInviteeCookie(string email, string token);
					MVCSite.Biz.WebsitePage Register(MVCSite.DAC.Entities.UserRegInfo regInfo);
					void SendConfirmationEmail(MVCSite.DAC.Entities.User user, string url, string pswd);
					void SendUserContactCSREmail(string toEmail, MVCSite.Biz.EmailUserContactCSR model);
					bool SendForgotPassword(string username, string email);
					bool SendResendConfirmation(string username, string email);
					void SendBookingConfirmationEmail(MVCSite.Biz.BookingConfirmationModel model);
					bool ResetPasswordWithToken(string passwordToken, string password);
					decimal TryParseDecimalOrMinusOne(string value);
		}
 
		public partial interface IUserCommands {
					void UserUpdate(MVCSite.DAC.Entities.User user);
		}
 
		public partial interface IUserQueries {
					System.Collections.Generic.IEnumerable<MVCSite.DAC.Entities.User> UserGetGlobalUsers();
					MVCSite.DAC.Entities.User GetByMobileOrNull(string mobile);
					MVCSite.DAC.Entities.User GetByPhoneOrNull(string area, string local);
					MVCSite.DAC.Entities.User UserGetById(int userId);
					MVCSite.DAC.Entities.User UserGetCurrentUser();
		}
}

