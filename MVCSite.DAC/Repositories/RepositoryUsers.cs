using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MVCSite.DAC.Interfaces;
using MVCSite.DAC.Instrumentation;
using MVCSite.DAC.Entities;
using System.Data.Objects;
using WMath.Facilities;
using System.Data.Entity.Validation;
using System.Data;
using System.Data.Entity;
using DevTrends.MvcDonutCaching;
using MVCSite.DAC.Instrumentation.Membership;
using MVCSite.Common;
using MVCSite.DAC.Common;
using System.Web.Security;

namespace MVCSite.DAC.Repositories
{
    public class RepositoryUsers : RepositoryBase, IRepositoryUsers
    {
        //protected readonly EFDataContext _dataContext;


        public RepositoryUsers(ILogger logger, EFDataContext dataContext, ICacheProvider cacheProvider)
            : base(dataContext, cacheProvider, logger)
        {
            //_dataContext = dataContext;
        }
        public bool ResetPasswordWithToken(string passwordToken, string password)
        {
            var user = _dataContext.Users.Where(x => x.ConfirmationToken == passwordToken).SingleOrDefault();
            if (user == null)
                return false;
            if (user.TokenExpireTime < DateTime.UtcNow)
                return false;//The reseting password token is expired
            user.ConfirmationToken = string.Empty;//Remove the reset password token
            user.TokenExpireTime = null;
            user.Password = Crypto.HashPassword(password);
            user.PasswordChangeTime = DateTime.UtcNow;
            _dataContext.SaveChanges();
            return true;
        }
        public void SetForgotPasswordToken(int userId, string passwordToken)
        {
            var user = _dataContext.Users.Where(x => x.ID == userId).SingleOrDefault();
            if (user == null)
                return;
            user.ConfirmationToken = passwordToken;
            user.TokenExpireTime = DateTime.UtcNow.AddDays(1);
            _dataContext.SaveChanges();
        }
        public User GetByIdOrNull(int id)
        {
            return _dataContext.Users.Where(x => x.ID == id).SingleOrDefault();
        }
        public User GetByEmailOrNull(string emailAddress)
        {
            return _dataContext.Users.Where(x => x.Email.ToLower().Trim() == emailAddress.ToLower().Trim()).SingleOrDefault();
        }
        public User GetByOpenIDOrNull(string openId,OpenSiteType site)
        {
            return _dataContext.Users.Where(x => x.OpenID == openId && x.OpenSite == (byte)site).SingleOrDefault();
        }
        //public User GetByUsernameOrNull(string username)
        //{
        //    return _dataContext.Users.Where(x => x.Name.ToLower().Trim() == username.ToLower().Trim()).SingleOrDefault();
        //}

        //public User GetByUsernameOrEmailOrNull(string usernameOrEmail)
        //{
        //    return _dataContext.Users.Where(x => x.Name.ToLower().Trim() == usernameOrEmail.ToLower().Trim()
        //        || x.Email.ToLower().Trim() == usernameOrEmail.ToLower().Trim()).SingleOrDefault();
        //}
        public User GetByMobileOrNull(string mobile)
        {
            return _dataContext.Users.Where(x => x.Mobile.Trim() == mobile.Trim()).SingleOrDefault();
        }
        public User GetByPhoneOrNull(string area,string local)
        {
            return _dataContext.Users.Where(x => x.PhoneAreaCode.Trim() == area.Trim() && x.PhoneLocalCode.Trim() == local.Trim()).SingleOrDefault();
        }
        public IEnumerable<User>  GetGlobalUsers()
        {
            return _dataContext.Users;
        }
        public IEnumerable<User> UserGetAllInUserIds(int [] userIds)
        {
            if (userIds == null || userIds.Length <= 0)
                return new List<User>();
            var users= from u in _dataContext.Users
                       join userId in userIds
                       on u.ID equals userId
                       select u;
            return users;
        }
        public IQueryable<User> UserGetAll()
        {
            return _dataContext.Users.OrderByDescending(x => x.EnterTime);//.ToPageable(page, pageSize);
        }
        public IQueryable<User> GuideGetAll()
        {
            return _dataContext.Users.Where(x=>x.Role==0).OrderByDescending(x => x.EnterTime);//.ToPageable(page, pageSize);
        }
        public IQueryable<User> TravellerGetAll()
        {
            return _dataContext.Users.Where(x=>x.Role==1).OrderByDescending(x => x.EnterTime);//.ToPageable(page, pageSize);
        }
        public IPageable<User> UserGetAllByPage(int page, int pageSize)
        {
            return _dataContext.Users.OrderByDescending(x => x.ID).ToPageable(page, pageSize);
        }
        public IPageable<User> UserGetAllByPageExceptAfter(byte lan,int userId,int page, int pageSize,DateTime after)
        {
            return _dataContext.Users.Where(x=>x.ID != userId && x.EnterTime > after).OrderByDescending(x => x.ID).ToPageable(page, pageSize);
        }
        public IPageable<User> UserGetAllLogonedBetweenByPage(int page, int pageSize,DateTime start,DateTime end)
        {
            var users = from u in _dataContext.Users
                        where u.LastLoginTime >= start && u.LastLoginTime <= end
                        select u ;
            return users.OrderByDescending(x => x.LastLoginTime).ToPageable(page, pageSize);
        }
        public void UserRemoveCachedUser(int id)
        {
            ObjectCacheManager cm = new ObjectCacheManager();
            cm.RemoveItem("UserQueries", "UserGetCurrentUser", new { userId = id });
            return;
        }
        public void UserUpdate(User user)
        {
            try
            {
                var curUser = _dataContext.Users.Find(user.ID);
                if (curUser == null)
                {
                    curUser = _dataContext.Users.Where(x => x.ID == user.ID).Single();
                }
                curUser.CopyFields(user);
                EnsureAttachedAndModified("Users", curUser);
                _dataContext.SaveChanges();
                ObjectCacheManager cm = new ObjectCacheManager();
                cm.RemoveItem("UserQueries", "UserGetCurrentUser", new { userId = user.ID });
            }
            catch (DbEntityValidationException dbEx)
            {
                this.HandleDbEntityValidationException(dbEx, "UserUpdate()");
            }
            return;
        }

        public User ActivateUser(int userId, string token)
        {
            var user = _dataContext.Users.Where(x => x.ID == userId).SingleOrDefault();
            var isValid = user != null && user.ConfirmationToken == token;
            if (isValid)
            {
                if (!string.IsNullOrEmpty(user.ChangeToEmail))
                { //User are changing their email
                    user.Email = user.ChangeToEmail;
                    user.ChangeToEmail = string.Empty;
                }
                user.IsConfirmed = true;
                user.TokenExpireTime = null;
                user.ConfirmationToken = string.Empty;
                _dataContext.SaveChanges();
                return user;
            }
            else
                return null;
        }
        public User ActivateUserByPassConfirmationToken(string email)
        {
            var user = _dataContext.Users.Where(x => x.Email == email).SingleOrDefault();
            var isValid = user != null;
            if (isValid)
            {
                if (!string.IsNullOrEmpty(user.ChangeToEmail))
                { //User are changing their email
                    user.Email = user.ChangeToEmail;
                    user.ChangeToEmail = string.Empty;
                }
                user.IsConfirmed = true;
                user.TokenExpireTime = null;
                user.ConfirmationToken = string.Empty;
                _dataContext.SaveChanges();
                return user;
            }
            else
                return null;
        }

        public User UserGetCurrentUser(int userId)
        {
            //var userId = _security.GetCurrentUserId();
            ObjectCacheManager cm = new ObjectCacheManager();
            User oldUser = (User)cm.GetItem("UserQueries", "UserGetCurrentUser", new { userId = userId });
            if (oldUser == null)
            {
                var newUser = GetByIdOrNull(userId);
                if (newUser == null)
                    return null;
                cm.AddItem("UserQueries", "UserGetCurrentUser", new { userId = userId }, newUser, 3600);
                return newUser;
            }
            else
                return oldUser;

        }
        public void UserUpdateGeoInfo(User current)
        {
             try
            {
                var dbOne = _dataContext.Users.Where(x=>x.ID==current.ID).SingleOrDefault();
                dbOne.GeoX = current.GeoX;
                dbOne.GeoY = current.GeoY;
                dbOne.Address = current.Address;
                dbOne.LocationId = current.LocationId;
                dbOne.ModifyTime = DateTime.UtcNow;
                _dataContext.Entry(dbOne).Property(x => x.GeoX).IsModified = true;
                _dataContext.Entry(dbOne).Property(x => x.GeoY).IsModified = true;
                _dataContext.Entry(dbOne).Property(x => x.ModifyTime).IsModified = true;
                _dataContext.Entry(dbOne).Property(x => x.LocationId).IsModified = true;
                _dataContext.SaveChanges();
                ObjectCacheManager cm = new ObjectCacheManager();
                cm.RemoveItem("UserQueries", "UserGetCurrentUser", new { userId = current.ID });
            }
            catch (DbEntityValidationException dbEx)
            {
                throw;
            }
        }
        public void UserUpdateLogonInfo(User current)
        {
            try
            {
                _dataContext.Entry(current).Property(x => x.LastLoginTime).IsModified = true;
                _dataContext.Entry(current).Property(x => x.SignUpBrowser).IsModified = true;
                _dataContext.Entry(current).Property(x => x.SignUpIP).IsModified = true;
                _dataContext.Entry(current).Property(x => x.ModifyTime).IsModified = true;
                _dataContext.SaveChanges();
                ObjectCacheManager cm = new ObjectCacheManager();
                cm.RemoveItem("UserQueries", "UserGetCurrentUser", new { userId = current.ID });
                ////_objectContext.Refresh(RefreshMode.StoreWins, _dataContext.Users);
            }
            catch (DbEntityValidationException dbEx)
            {
                StringBuilder sb = new StringBuilder(1024);
                foreach (var validationErrors in dbEx.EntityValidationErrors)
                {
                    foreach (var validationError in validationErrors.ValidationErrors)
                    {
                        var error = string.Format("ValidationError--Property: {0} Error: {1}",
                            validationError.PropertyName, validationError.ErrorMessage);
                        sb.Append(error);
                    }
                }
                throw new Exception(sb.ToString());
            }
        }
        public void UserUpdateAvatar(User current)
        {
            try
            {
                var dbOne = _dataContext.Users.Where(x => x.ID == current.ID).SingleOrDefault();
                if (dbOne == null)
                    throw new Exception(string.Format("UserUpdateAvatar() can NOT find user with id:{0}.", current.ID));
                if (!string.IsNullOrEmpty(dbOne.AvatarPath))
                {
                    FileSystemHelper.CheckAndDeleteOldFile(StaticSiteConfiguration.ImageFileDirectory, dbOne.AvatarPath, _logger);
                }
                dbOne.AvatarPath = current.AvatarPath;
                _dataContext.Entry(dbOne).Property(x => x.AvatarPath).IsModified = true;
                _dataContext.Entry(dbOne).Property(x => x.ModifyTime).IsModified = true;
                _dataContext.SaveChanges();
                UserRemoveCachedUser(current.ID);
            }
            catch (DbEntityValidationException dbEx)
            {
                HandleDbEntityValidationException(dbEx, "UserUpdateAvatar");
            }
        }
        public void UserUpdateAvatar(int id,string avatarPath)
        {
            try
            {
                var dbOne = _dataContext.Users.Where(x => x.ID == id).SingleOrDefault();
                if (dbOne == null)
                    throw new Exception(string.Format("UserUpdateAvatar() can NOT find user with id:{0}.", id));
                if (!string.IsNullOrEmpty(dbOne.AvatarPath))
                {
                    FileSystemHelper.CheckAndDeleteOldFile(StaticSiteConfiguration.ImageFileDirectory, dbOne.AvatarPath, _logger);
                }
                dbOne.AvatarPath = avatarPath;
                _dataContext.Entry(dbOne).Property(x => x.AvatarPath).IsModified = true;
                _dataContext.Entry(dbOne).Property(x => x.ModifyTime).IsModified = true;
                _dataContext.SaveChanges();
                UserRemoveCachedUser(id);

            }
            catch (DbEntityValidationException dbEx)
            {
                HandleDbEntityValidationException(dbEx, "UserUpdateAvatar");
            }
        }
        public void UserUpdateVideo(int userId, string videoPath)
        {
            try
            {
                var dbOne = _dataContext.Users.Where(x => x.ID == userId).SingleOrDefault();
                if (dbOne == null)
                    throw new Exception(string.Format("UserUpdateAvatar() can NOT find user with id:{0}.", userId));
                if (!string.IsNullOrEmpty(dbOne.VideoPath))
                {
                    FileSystemHelper.CheckAndDeleteOldFile(StaticSiteConfiguration.ImageFileDirectory, dbOne.VideoPath, _logger);
                }
                dbOne.VideoPath = videoPath;
                _dataContext.Entry(dbOne).Property(x => x.VideoPath).IsModified = true;
                _dataContext.Entry(dbOne).Property(x => x.ModifyTime).IsModified = true;
                _dataContext.SaveChanges();
                UserRemoveCachedUser(userId);

            }
            catch (DbEntityValidationException dbEx)
            {
                HandleDbEntityValidationException(dbEx, "UserUpdateVideo");
            }
        }
        public User UserUpdateComfirmationToken(int userId)
        {
            try
            {
                var dbOne = _dataContext.Users.Where(x => x.ID == userId).SingleOrDefault();
                if (dbOne == null)
                    throw new Exception(string.Format("UserUpdateAvatar() can NOT find user with id:{0}.", userId));
                dbOne.ConfirmationToken = Guid.NewGuid().ToString();
                dbOne.TokenExpireTime = DateTime.UtcNow.AddDays(3);
                dbOne.ModifyTime = DateTime.UtcNow;
                _dataContext.Entry(dbOne).Property(x => x.ConfirmationToken).IsModified = true;
                _dataContext.Entry(dbOne).Property(x => x.TokenExpireTime).IsModified = true;
                _dataContext.Entry(dbOne).Property(x => x.ModifyTime).IsModified = true;
                _dataContext.SaveChanges();
                UserRemoveCachedUser(userId);

                return dbOne;
            }
            catch (DbEntityValidationException dbEx)
            {
                HandleDbEntityValidationException(dbEx, "UserUpdateComfirmationToken");
            }
            return null;
        }
        public User UserUpdatePasswordFromOpenSite(int userId, string email, string password)
        {
            try
            {
                var dbOne = _dataContext.Users.Where(x => x.ID == userId).SingleOrDefault();
                if (dbOne == null)
                    throw new Exception(string.Format("UserUpdateAvatar() can NOT find user with id:{0}.", userId));
                string hashedPassword = Crypto.HashPassword(password);
                if (hashedPassword.Length > 128)
                {
                    throw new MembershipCreateUserException(MembershipCreateStatus.InvalidPassword);
                }
                dbOne.Email = email;
                dbOne.Password = hashedPassword;
                dbOne.IsEmailVerified4OpenID = true;
                dbOne.ModifyTime = DateTime.UtcNow;
                _dataContext.Entry(dbOne).Property(x => x.Email).IsModified = true;
                _dataContext.Entry(dbOne).Property(x => x.Password).IsModified = true;
                _dataContext.Entry(dbOne).Property(x => x.IsEmailVerified4OpenID).IsModified = true;
                _dataContext.Entry(dbOne).Property(x => x.ModifyTime).IsModified = true;
                _dataContext.SaveChanges();
                UserRemoveCachedUser(userId);

                return dbOne;
            }
            catch (DbEntityValidationException dbEx)
            {
                HandleDbEntityValidationException(dbEx, "UserUpdatePasswordFromOpenSite");
            }
            return null;
        }
        public UserMsg UserMsgGetByID(int id)
        {
            return _dataContext.UserMsgs.Where(x => x.ID == id).SingleOrDefault();
        }
        public IPageable<UserMsg> UserMsgGetAllByToUserID(int userId, int page, int pageSize,UserMsgStatus status)
        {
            if(status == UserMsgStatus.All)
                return _dataContext.UserMsgs.Where(x => x.ToUserID == userId)
                    .OrderByDescending(x => x.EnterTime).ToPageable(page, pageSize);
            return _dataContext.UserMsgs.Where(x => x.ToUserID == userId&&x.Status == (byte)status)
                .OrderByDescending(x => x.EnterTime).ToPageable(page, pageSize);
        }
        public IPageable<UserMsg> UserMsgGetAllByUserIDs(int userId1, int userId2, int end, int page, int pageSize)
        {
            if (end == 0)
                return _dataContext.UserMsgs.Where(x => (x.FromUserID == userId1 && x.ToUserID == userId2)
                    || (x.ToUserID == userId1 && x.FromUserID == userId2))
                    .OrderByDescending(x => x.EnterTime)
                    .ToPageable(page, pageSize);
            else
                return _dataContext.UserMsgs.Where(x => x.ID <= end && ((x.FromUserID == userId1 && x.ToUserID == userId2)
                    || (x.ToUserID == userId1 && x.FromUserID == userId2)))
                    .OrderByDescending(x => x.EnterTime)
                    .ToPageable(page, pageSize);
        }
        public UserMsg UserMsgCreateOrUpdate(UserMsg key)
        {
            try
            {
                if (key.ID > 0)
                    EnsureAttachedAndModified("UserMsgs", key);
                else
                {
                    _dataContext.UserMsgs.Add(key);
                }
                _dataContext.SaveChanges();
            }
            catch (DbEntityValidationException dbEx)
            {
                HandleDbEntityValidationException(dbEx, " UserMsgCreateOrUpdate()");
            }
            return key;
        }
        public void UserMsgUpdateStatusById(int id, UserMsgStatus status)
        {
            try
            {
                var dbOne = _dataContext.UserMsgs.Where(x => x.ID == id).SingleOrDefault();
                if (dbOne == null)
                    throw new Exception(string.Format("UserMsgUpdateStatus() can NOT find UserMsg with id:{0}.", id));
                dbOne.Status = (byte)status;
                dbOne.ModifyTime = DateTime.UtcNow;
                _dataContext.Entry(dbOne).Property(x => x.Status).IsModified = true;
                _dataContext.Entry(dbOne).Property(x => x.ModifyTime).IsModified = true;
                _dataContext.SaveChanges();
            }
            catch (DbEntityValidationException dbEx)
            {
                HandleDbEntityValidationException(dbEx, "UserMsgUpdateStatus");
            }
        }
        public void UserMsgUpdateStatusByIds(string ids, UserMsgStatus status)
        {
            var sqlStr = string.Format("UPDATE [dbo].[UserMsg] SET [Status]={0} WHERE  [ID] IN({1});",
                (int)status, ids);
            _objectContext.ExecuteStoreCommand(sqlStr);
        }
        public User UserUpdateRole(int userId, UserRole role)
        {
            try
            {
                var dbOne = _dataContext.Users.Where(x => x.ID == userId).SingleOrDefault();
                if (dbOne == null)
                    throw new Exception(string.Format("UserUpdateAvatar() can NOT find user with id:{0}.", userId));
                dbOne.Role = (short)role;
                dbOne.ModifyTime = DateTime.UtcNow;
                _dataContext.Entry(dbOne).Property(x => x.Role).IsModified = true;
                _dataContext.Entry(dbOne).Property(x => x.ModifyTime).IsModified = true;
                _dataContext.SaveChanges();
                UserRemoveCachedUser(userId);
                return dbOne;
            }
            catch (DbEntityValidationException dbEx)
            {
                HandleDbEntityValidationException(dbEx, "UserUpdateRole");
            }
            return null;
        }

        public UserPromo UserPromoGetById(int userId, int promoId)
        {
            return _dataContext.UserPromoes.FirstOrDefault(x => x.UserID == userId && x.PromoID == promoId);
        }

        public UserPromo UserPromoCreateOrUpdate(UserPromo promo)
        {
            try
            {
                if (promo.ID > 0)
                {
                    var dbOne = _dataContext.UserPromoes.Where(x => x.ID == promo.ID).SingleOrDefault();
                    if (dbOne == null)
                        throw new Exception(string.Format("userpromo not found", promo.ID));
                   
                    dbOne.isPromoUsed = promo.isPromoUsed;
                    _dataContext.Entry(dbOne).Property(x => x.isPromoUsed).IsModified = true;
                }
                else
                {
                    _dataContext.UserPromoes.Add(promo);
                }
                _dataContext.SaveChanges();
            }

            catch (DbEntityValidationException dbEx)
            {
                HandleDbEntityValidationException(dbEx, " UserPromoCreateOrUpdate()");
            }
            return promo;
        }
           
        
    }
}
  