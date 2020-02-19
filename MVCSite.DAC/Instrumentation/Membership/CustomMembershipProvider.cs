using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Security;
using Microsoft.Practices.Unity;
using MVCSite.DAC.Entities;
using MVCSite.DAC.Repositories;
using MVCSite.DAC.Common;
using MVCSite.Common;
using System.Data.Entity.Validation;

namespace MVCSite.DAC.Instrumentation.Membership
{
    public class CustomMembershipProvider : MembershipProvider
    {
        public static IUnityContainer RootContainer { set; get; }

        private const int OneDayInMinutes = 24 * 60;

        public virtual string CreateAccount(string userName, string password, string email)
        {
            return CreateAccount(userName, password, email, requireConfirmationToken: false, nickName: "");
        }
        public virtual string GeneratePasswordResetToken(string userName)
        {
            return GeneratePasswordResetToken(userName, tokenExpirationInMinutesFromNow: OneDayInMinutes);
        }

        private EFDataContext getRepository()
        {
            return RootContainer.Resolve<EFDataContext>();
        }
        private EFDataContext createRepository()
        {
            //return getRepository();
            return RootContainer.Resolve<EFDataContext>("perResolveRepository");
        }
        private string _applicationName;

        public override void Initialize(string name, System.Collections.Specialized.NameValueCollection config)
        {
            if (config == null)
            {
                throw new ArgumentNullException("config");
            }
            if (string.IsNullOrEmpty(name))
            {
                name = "CodeFirstMembershipProvider";
            }
            if (string.IsNullOrEmpty(config["description"]))
            {
                config.Remove("description");
                config.Add("description", "Code-First Membership Provider");
            }

            base.Initialize(name, config);

            _applicationName = config["applicationName"];
        }

        #region "Main Functions"
        public string CreateAccount(string name, string pswd, string mail, bool requireConfirmationToken, string nickName)
        {
            //UserRole role = StaticSiteConfiguration.IsGuiderSite ? UserRole.Guider : UserRole.Tourist;
            return CreateAccount(new UserRegInfo()
            {
                FirstName = name,
                Password = pswd,
                Email = mail,
                RequireConfirmationToken = requireConfirmationToken,
                NickName = nickName,
                Role = (short)UserRole.Tourist,
            }).ConfirmationToken;
        }
        public User CreateAccount(UserRegInfo regInfo)
        {
            //string userName, string password, string email,
            //string title, string areaCode, string localCode, bool requireConfirmationToken, string userIp, string userBrowser, 
            //string nickName = "", short role = 0;

            if(regInfo == null)
               throw new MembershipCreateUserException(MembershipCreateStatus.ProviderError);
            if (string.IsNullOrEmpty(regInfo.Password))
            {
                throw new MembershipCreateUserException(MembershipCreateStatus.InvalidPassword);
            }
            string hashedPassword = Crypto.HashPassword(regInfo.Password);
            if (hashedPassword.Length > 128)
            {
                throw new MembershipCreateUserException(MembershipCreateStatus.InvalidPassword);
            }
            if (string.IsNullOrEmpty(regInfo.FirstName))
            {
                throw new MembershipCreateUserException(MembershipCreateStatus.InvalidUserName);
            }
            if (string.IsNullOrEmpty(regInfo.Email))
            {
                throw new MembershipCreateUserException(MembershipCreateStatus.InvalidEmail);
            }
            //var fullNamePinyin = ChineseHelper.ConvertHanziToPinyin(regInfo.NickName);
            var context = createRepository();
            //dynamic user = context.Users.FirstOrDefault(Usr => Usr.Name == regInfo.UserName);
            dynamic emailuser = context.Users.FirstOrDefault(Usr => Usr.Email == regInfo.Email);
            //if (user != null)
            //{
            //    throw new MembershipCreateUserException(MembershipCreateStatus.DuplicateUserName);
            //}
            if (emailuser != null)
            {
                throw new MembershipCreateUserException(MembershipCreateStatus.DuplicateEmail);
            }
            //var userLocation = context.UserLocationGetCityByIP(regInfo.UserIp).SingleOrDefault();
            //if (userLocation == null)
            //{
            //    userLocation = context.UserLocations.Where(x => x.STRING_ID == "beijing").FirstOrDefault(); 
            //}
            var newUser = new User
            {
                FirstName=regInfo.FirstName,
                LastName=regInfo.LastName,
                Password=hashedPassword,
                Email=regInfo.Email,
                EnterTime=DateTime.UtcNow,
                ModifyTime=DateTime.UtcNow,
                IsConfirmed = !regInfo.RequireConfirmationToken,
                CanChangeName=true,
                LastLoginTime=DateTime.UtcNow,
                SignUpIP=regInfo.UserIp,
                SignUpBrowser = regInfo.UserBrowser,
                ConfirmationToken = Guid.NewGuid().ToString(),
                TokenExpireTime=DateTime.UtcNow.AddDays(15),
                Initials = !string.IsNullOrEmpty(regInfo.NickName) ? regInfo.NickName.GetFirstN(3):(!string.IsNullOrEmpty(regInfo.FirstName) ? regInfo.FirstName.GetFirstN(3):string.Empty),
                FullName = regInfo.NickName,
                NotificationEmailType = (byte)NotificationEmailType.Periodically,
                Title=regInfo.Title,
                PhoneAreaCode=regInfo.AreaCode,
                PhoneLocalCode=regInfo.LocalCode,
                Role=regInfo.Role,
                IsOnline=true,
                DefaultPhoneType=(byte)ConnectPhoneType.Mobile,
                UseAvatar=false,
                GeoX = 0,
                GeoY =0,
                LocationId =0,
                Sex=1,
                VideoPath=string.Empty,
                Birthday=null,
                IsPhoneConfirmed=false,
                OpenID=regInfo.OpenID,
                OpenSite=regInfo.SiteID,
                Address=regInfo.Location,
                AvatarPath=regInfo.Avatar??string.Empty,
                IsEmailVerified4OpenID=regInfo.WithOpenSiteEmail,
            };
            //if (userCompany != null)
            //    newUser.UserCompanies.Add(userCompany);
            //if (userBoard != null)
            //    newUser.UserBoards.Add(userBoard);
            context.Users.Add(newUser);
            try
            {
                context.SaveChanges();
            }
            catch (DbEntityValidationException dbEx)
            {
                StringBuilder sb = new StringBuilder(1024);
                foreach (var validationErrors in dbEx.EntityValidationErrors)
                {
                    foreach (var validationError in validationErrors.ValidationErrors)
                    {
                        var errorMsg = string.Format("ValidationError--Property: {0} Error: {1}", validationError.PropertyName, validationError.ErrorMessage);
                        sb.Append(errorMsg);
                        sb.AppendLine();
                    }
                }
                throw new Exception(string.Format("CreateAccount() got exception messsage:{0}.",sb.ToString()));
            }
            catch (Exception e)
            {
                //this.ToString();
                throw;
            }
            return newUser;
        }

        public bool ConfirmAccount(string accountConfirmationToken)
        {
            if (string.IsNullOrEmpty(accountConfirmationToken))
            {
                throw CreateArgumentNullOrEmptyException("accountConfirmationToken");
            }
            var context = createRepository();
            {
                //var row = context.User.FirstOrDefault(x => x.ConfirmationToken == accountConfirmationToken);
                //if (row != null)
                //{
                //    row.IsConfirmed = true;
                //    context.SaveChanges();
                //    return true;
                //}
                return false;
            }
        }

        public override bool ChangePassword(string email, string oldPassword, string newPassword)
        {
            if (string.IsNullOrEmpty(email))
            {
                throw CreateArgumentNullOrEmptyException("email");
            }
            if (string.IsNullOrEmpty(oldPassword))
            {
                throw CreateArgumentNullOrEmptyException("oldPassword");
            }
            if (string.IsNullOrEmpty(newPassword))
            {
                throw CreateArgumentNullOrEmptyException("newPassword");
            }
            var context = createRepository();
            {
                User user = context.Users.FirstOrDefault(x => x.Email == email);
                if (user == null)
                {
                    return false;
                }
                dynamic hashedPassword = user.Password;
                bool verificationSucceeded = IsVerificationSucceed(hashedPassword, oldPassword);
                if (verificationSucceeded)
                {
                    user.PasswordFailedCount = 0;
                }
                else
                {
                    byte failures = (byte)user.PasswordFailedCount;
                    if ((int)failures != -1)
                    {
                        user.PasswordFailedCount += 1;
                        user.PasswordFailedTime = DateTime.UtcNow;
                    }
                    context.SaveChanges();
                    return false;
                }
                dynamic newhashedPassword = Crypto.HashPassword(newPassword);
                if (newhashedPassword.Length > 128)
                {
                    throw new ArgumentException("Password too long");
                }
                user.Password = newhashedPassword;
                user.PasswordFailedTime = DateTime.UtcNow;
                context.SaveChanges();
                return true;
            }
        }



        public bool DeleteAccount(string email)
        {
            if (string.IsNullOrEmpty(email))
            {
                throw CreateArgumentNullOrEmptyException("userName");
            }
            var context = createRepository();
            {
                dynamic user = context.Users.FirstOrDefault(x => x.Email == email);
                if (user == null)
                {
                    return false;
                }
                context.Users.Remove(user);
                context.SaveChanges();
                return true;
            }
        }

        public bool IsConfirmed(string email)
        {
            if (string.IsNullOrEmpty(email))
            {
                throw CreateArgumentNullOrEmptyException("userName");
            }
            var context = getRepository();
            {
                dynamic user = context.Users.FirstOrDefault(x => x.Email == email);
                if (user == null)
                {
                    return false;
                }
                if (user.IsConfirmed)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
        }

        public string GeneratePasswordResetToken(string email, int tokenExpirationInMinutesFromNow)
        {
            if (string.IsNullOrEmpty(email))
            {
                throw CreateArgumentNullOrEmptyException("userName");
            }
            var context = createRepository();
            {
                dynamic user = context.Users.FirstOrDefault(x => x.Email == email);
                if (user == null)
                {
                    throw new InvalidOperationException(string.Format("User not found: {0}", email));
                }
                if (!user.IsConfirmed)
                {
                    throw new InvalidOperationException(string.Format("User not found: {0}", email));
                }
                string token = null;
                if (user.PasswordVerificationTokenExpirationDate > DateTime.UtcNow)
                {
                    token = user.PasswordVerificationToken;
                }
                else
                {
                    token = Crypto.GenerateToken();
                }
                user.PasswordVerificationToken = token;
                user.PasswordVerificationTokenExpirationDate = DateTime.UtcNow.AddMinutes(tokenExpirationInMinutesFromNow);
                context.SaveChanges();
                return token;
            }
        }

        public bool ResetPasswordWithToken(string token, string newPassword)
        {
            throw new NotImplementedException();
            /*
            if (string.IsNullOrEmpty(newPassword))
            {
                throw CreateArgumentNullOrEmptyException("newPassword");
            }
            var context = createRepository();
            {
                dynamic user = context.User.FirstOrDefault(x => x.PasswordVerificationToken == token && x.PasswordVerificationTokenExpirationDate > DateTime.UtcNow);
                if (user != null)
                {
                    dynamic newhashedPassword = CodeFirstCrypto.HashPassword(newPassword);
                    if (newhashedPassword.Length > 128)
                    {
                        throw new ArgumentException("Password too long");
                    }
                    user.Password = newhashedPassword;
                    user.PasswordChangedDate = DateTime.UtcNow;
                    user.PasswordVerificationToken = null;
                    user.PasswordVerificationTokenExpirationDate = null;
                    context.SaveChanges();
                    return true;
                }
                else
                {
                    return false;
                }
            }
            */
        }

        bool IsVerificationSucceed(string hashedPassword, string password)
        {
            return hashedPassword != null && ((Crypto.VerifyHashedPassword(hashedPassword, password)
                    || Crypto.VerifyHashedPasswordOld(hashedPassword, password)));
        }

        public string ExtendedValidateUser(string userNameOrEmail, string password)
        {
            if (string.IsNullOrEmpty(userNameOrEmail))
            {
                throw CreateArgumentNullOrEmptyException("userNameOrEmail");
            }
            if (string.IsNullOrEmpty(password))
            {
                throw CreateArgumentNullOrEmptyException("password");
            }
            var context = createRepository();
            {
                User user = null;
                IList<User> userList = null;
                userList = context.Users.Where(usr => usr.Email == userNameOrEmail).ToList();
                if (userList == null || userList.Count<1)
                {
                    return string.Empty;
                }
                user = userList[0];
                //if (!user.IsConfirmed)
                //{
                //    return string.Empty;
                //}
                var hashedPassword = user.Password;

                bool verificationSucceeded = IsVerificationSucceed(hashedPassword, password);
                if (verificationSucceeded)
                {
                    user.PasswordFailedCount = 0;
                }
                else
                {
                    int failures = user.PasswordFailedCount == null ? 0 : (int)user.PasswordFailedCount;
                    if (failures != -1)
                    {
                        user.PasswordFailedCount += 1;
                        user.PasswordFailedTime = DateTime.UtcNow;
                    }
                }
                context.SaveChanges();

                if (verificationSucceeded)
                {
                    return user.Email;
                }
                else
                {
                    return string.Empty;
                }
            }
        }
        public override bool ValidateUser(string username, string password)
        {
            if (string.IsNullOrEmpty(password))
                return false;
            return ExtendedValidateUser(username, password) != string.Empty;
        }

        public MembershipUser CreateUser(string username, string password, string email,
            string passwordQuestion, string passwordAnswer, bool isApproved, object providerUserKey, string nickName, out MembershipCreateStatus status)
        {
            CreateAccount(username, password, email, !isApproved, nickName);
            status = MembershipCreateStatus.Success;
            return GetUser(email, true);
        }
        public override MembershipUser CreateUser(string username, string password, string email,
            string passwordQuestion, string passwordAnswer, bool isApproved, object providerUserKey, out MembershipCreateStatus status)
        {
            return CreateUser(username, password, email, passwordQuestion, passwordAnswer, isApproved, providerUserKey, "", out status);
        }
        private ArgumentException CreateArgumentNullOrEmptyException(string paramName)
        {
            return new ArgumentException(string.Format("Argument cannot be null or empty: {0}", paramName));
        }



        #endregion

        #region "Get Functions"

        public System.DateTime GetPasswordChangedDate(string email)
        {
            if (string.IsNullOrEmpty(email))
            {
                throw CreateArgumentNullOrEmptyException("userName");
            }
            var context = getRepository();
            {
                dynamic user = context.Users.FirstOrDefault(Usr => Usr.Email == email);
                if (user == null)
                {
                    throw new InvalidOperationException(string.Format("User not found: {0}", email));
                }
                return user.PasswordChangedDate;
            }
        }

        public System.DateTime GetCreateDate(string email)
        {
            if (string.IsNullOrEmpty(email))
            {
                throw CreateArgumentNullOrEmptyException("userName");
            }
            var context = getRepository();
            {
                dynamic user = context.Users.FirstOrDefault(Usr => Usr.Email == email);
                if (user == null)
                {
                    throw new InvalidOperationException(string.Format("User not found: {0}", email));
                }
                return user.CreateDate;
            }
        }

        public int GetPasswordFailedCount(string userName)
        {
            if (string.IsNullOrEmpty(userName))
            {
                throw CreateArgumentNullOrEmptyException("userName");
            }
            var context = getRepository();
            {
                dynamic user = context.Users.FirstOrDefault(usr => usr.Email == userName);
                if (user == null)
                {
                    throw new InvalidOperationException(string.Format("User not found: {0}", userName));
                }
                return user.PasswordFailedCount;
            }
        }

        public override System.Web.Security.MembershipUser GetUser(string email, bool userIsOnline)
        {
            if (string.IsNullOrEmpty(email))
            {
                throw CreateArgumentNullOrEmptyException("email");
            }
            var context = getRepository();
            {
                User user = null;
                IList<User> userList = null;
                userList = context.Users.Where(usr => usr.Email == email).ToList();
                if (userList == null || userList.Count < 1)
                {
                    return null;
                }
                user = userList[0];

                if (user == null)
                {
                    return null;
                }
                return new MembershipUser(System.Web.Security.Membership.Provider.Name, email, user.ID, user.Email, null, user.ConfirmationToken, true, false, user.EnterTime, DateTime.MinValue,
                DateTime.MinValue, DateTime.MinValue, DateTime.MinValue);
            }
        }

        public System.Guid GetUserIdFromPasswordResetToken(string token)
        {
            throw new NotImplementedException();
            /*
            if (string.IsNullOrEmpty(token))
            {
                throw CreateArgumentNullOrEmptyException("token");
            }
            var context = getRepository();
            {
                dynamic result = context.User.FirstOrDefault(usr => usr.PasswordVerificationToken == token);
                if (result != null)
                {
                    return result.UserId;
                }
                return Guid.Empty;
            }*/
        }

        public System.DateTime GetLastPasswordFailureDate(string email)
        {
            if (string.IsNullOrEmpty(email))
            {
                throw CreateArgumentNullOrEmptyException("userName");
            }
            var context = getRepository();
            {
                dynamic user = context.Users.FirstOrDefault(usr => usr.Email == email);
                if (user == null)
                {
                    throw new InvalidOperationException(string.Format("User not found: {0}", email));
                }
                return user.LastPasswordFailureDate;
            }
        }

        public override MembershipUser GetUser(object providerUserKey, bool userIsOnline)
        {
            var userId = (int)providerUserKey;
            var context = getRepository();
            {
                var user = context.Users.FirstOrDefault(usr => usr.ID == userId);
                if (user == null)
                {
                    return null;
                }
                return new MembershipUser(System.Web.Security.Membership.Provider.Name, user.Email, user.ID, user.Email, null, null, true, false, user.EnterTime, DateTime.MinValue,
                DateTime.MinValue, DateTime.MinValue, DateTime.MinValue);
            }
        }

        public override string GetUserNameByEmail(string email)
        {
            if (string.IsNullOrEmpty(email))
            {
                throw CreateArgumentNullOrEmptyException("email");
            }
            var context = getRepository();
            {
                var user = context.Users.FirstOrDefault(usr => usr.Email == email);
                if (user == null)
                {
                    return null;
                }
                return user.Email;
            }
        }

        #endregion

        #region "Properties"
        public override string ApplicationName
        {
            get { return _applicationName; }
            set { _applicationName = value; }
        }
        public override bool EnablePasswordReset
        {
            get { return false; }
        }
        public override bool EnablePasswordRetrieval
        {
            get { return false; }
        }
        public override int MaxInvalidPasswordAttempts
        {
            get { return int.MaxValue; }
        }
        public override int MinRequiredNonAlphanumericCharacters
        {
            get { return 0; }
        }
        public override int MinRequiredPasswordLength
        {
            get { return 6; }
        }
        public override int PasswordAttemptWindow
        {
            get { return int.MaxValue; }
        }
        public override System.Web.Security.MembershipPasswordFormat PasswordFormat
        {
            get { return MembershipPasswordFormat.Hashed; }
        }
        public override string PasswordStrengthRegularExpression
        {
            get { return string.Empty; }
        }
        public override bool RequiresQuestionAndAnswer
        {
            get { return false; }
        }
        public override bool RequiresUniqueEmail
        {
            get { return true; }
        }
        #endregion

        #region "Not Needed"

        public override bool ChangePasswordQuestionAndAnswer(string username, string password, string newPasswordQuestion, string newPasswordAnswer)
        {
            throw new NotSupportedException();
        }
        public override bool DeleteUser(string username, bool deleteAllRelatedData)
        {
            throw new NotImplementedException();
        }
        public override MembershipUserCollection FindUsersByEmail(string emailToMatch, int pageIndex, int pageSize, out int totalRecords)
        {
            throw new NotSupportedException();
        }
        public override MembershipUserCollection FindUsersByName(string usernameToMatch, int pageIndex, int pageSize, out int totalRecords)
        {
            throw new NotSupportedException();
        }
        public override MembershipUserCollection GetAllUsers(int pageIndex, int pageSize, out int totalRecords)
        {
            throw new NotSupportedException();
        }
        public override int GetNumberOfUsersOnline()
        {
            throw new NotSupportedException();
        }
        public override string GetPassword(string username, string answer)
        {
            throw new NotSupportedException();
        }
        public override string ResetPassword(string username, string answer)
        {
            throw new NotSupportedException();
        }
        public override bool UnlockUser(string userName)
        {
            throw new NotSupportedException();
        }
        public override void UpdateUser(MembershipUser user)
        {
            throw new NotSupportedException();
        }

        #endregion
    }
}
