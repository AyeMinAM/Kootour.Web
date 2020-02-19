using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Web.Mvc;
using System.IO;
using MVCSite.DAC.Common;
using System.Data;
using MVCSite.Common;

namespace MVCSite.DAC.Entities
{

    public enum OpenSiteType
    {
        NotSet = 0,
        WeChat = 1,
        Facebook = 2,

    }
    public enum Month
    {
        NotSet = 0,
        January = 1,
        February = 2,
        March = 3,
        April = 4,
        May = 5,
        June = 6,
        July = 7,
        August = 8,
        September = 9,
        October = 10,
        November = 11,
        December = 12,
    }
    public class MonthTranslation
    {
        public int MonthCode;
        public string MonthName;
        public MonthTranslation(Month code, string name)
        {
            MonthCode = (int)code;
            MonthName = name;
        }
        public static List<MonthTranslation> Translations = new List<MonthTranslation>() 
        { 
            new MonthTranslation(Month.January, "January"), 
            new MonthTranslation(Month.February, "February"), 
            new MonthTranslation(Month.March, "March"), 
            new MonthTranslation(Month.April, "April"), 
            new MonthTranslation(Month.May, "May"), 
            new MonthTranslation(Month.June, "June"), 
            new MonthTranslation(Month.July, "July"), 
            new MonthTranslation(Month.August, "August"), 
            new MonthTranslation(Month.September, "September"), 
            new MonthTranslation(Month.October, "October"), 
            new MonthTranslation(Month.November, "November"), 
            new MonthTranslation(Month.December, "December"), 
        };
    };
   

    public class UserRegInfo
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Password { get; set; }
        public string Email { get; set; }
        public string Title { get; set; }
        public string AreaCode { get; set; }
        public string LocalCode { get; set; }
        public bool RequireConfirmationToken { get; set; }
        public string UserIp { get; set; }
        public string UserBrowser { get; set; }
        public string NickName { get; set; }
        public short Role { get; set; }
        public byte Language { get; set; }
        public string ReturnUrl { get; set; }
        public byte SiteID { get; set; }
        public string OpenID { get; set; }
        public bool WithOpenSiteEmail { get; set; }
        public string Location { get; set; }
        public string Avatar { get; set; }
    }
    public partial class User
    {
        public byte RealOpenSite
        {
            get
            {
                return OpenSite??0;
            }
        }
        public string ShowName
        {
            get
            {
                return FirstName + " " + LastName;
            }
        }
        public bool EmailReadOnly
        {
            get;
            set;
        }
        public UserRole RealRole
        {
            get { return (UserRole)Role; }
            set { Role = (short)value; }
        }
        public List<SelectListItem> RoleOptions { get; set; }
        public bool Selected
        {
            get;
            set;
        }

        public bool IsGuider
        {
            get { return RealRole == UserRole.Guider; }
        }
        public ConnectPhoneType RealDefaultPhoneType
        {
            get { return (ConnectPhoneType)DefaultPhoneType; }
            set { DefaultPhoneType = (byte)value; }
        }
        public List<SelectListItem> ConnectPhoneOptions { get; set; }
        public string CallResultMsg
        {
            get;
            set;
        }
        public string AvatarUrl
        {
            get
            {
                if (!string.IsNullOrEmpty(AvatarPath))
                {
                    if (!AvatarPath.ToLower().StartsWith("http"))
                        return Path.Combine(StaticSiteConfiguration.ImageServerUrl, AvatarPath);
                    else
                        return AvatarPath;
                }
                else
                    return @"/Content/Images/community/noavatar.jpg";
            }
        }
        public string PrettyLastLogonTime
        {
            get
            {
                return ShowInfoHelper.GetPrettyDate(LastLoginTime == null ? EnterTime : LastLoginTime.Value);
            }
        }
        public SexType SexReal
        {
            get
            {
                return (SexType)(Sex ?? 1);
            }
            set
            {
                Sex = (byte)value;
            }
        }
        public string GetSexString(LanguageCode lan)
        {
            if(lan == LanguageCode.ChineseMandarian)
            {
                return EnumTranslation.SexTypeCn[(Sex ?? 1)];
            }
            return EnumTranslation.SexTypeEn[(Sex ?? 1)];
        }
        public List<SelectListItem> SexOptions { get; set; }
        #region Constructors
        public User()
        {
        }
        /// <summary>
        /// Constructor using IDataRecord.
        /// </summary>
        public User(IDataRecord record)
        {
            FieldLoader loader = new FieldLoader(record);

            this.ID = loader.LoadInt32("ID");
            this.FirstName = loader.LoadString("FirstName");
            this.LastName = loader.LoadString("LastName");
            this.Password = loader.LoadString("Password");
            this.Email = loader.LoadString("Email");
            this.EnterTime = loader.LoadDateTime("EnterTime");
            this.IsConfirmed = loader.LoadBoolean("IsConfirmed");
            this.CanChangeName = loader.LoadBoolean("CanChangeName");
            this.LastLoginTime = loader.LoadDateTime("LastLoginTime");
            this.ResetPasswordTime = loader.LoadByte("ResetPasswordTime");
            this.SignUpIP = loader.LoadString("SignUpIP");
            this.SignUpBrowser = loader.LoadString("SignUpBrowser");
            this.ConfirmationToken = loader.LoadString("ConfirmationToken");
            this.TokenExpireTime = loader.LoadDateTime("TokenExpireTime");
            this.PasswordChangeTime = loader.LoadDateTime("PasswordChangeTime");
            this.PasswordFailedCount = loader.LoadInt16("PasswordFailedCount");
            this.PasswordFailedTime = loader.LoadDateTime("PasswordFailedTime");
            this.ChangeToEmail = loader.LoadString("ChangeToEmail");
            this.Mobile = loader.LoadString("Mobile");
            this.PhoneAreaCode = loader.LoadString("PhoneAreaCode");
            this.PhoneLocalCode = loader.LoadString("PhoneLocalCode");
            this.DefaultPhoneType = loader.LoadByte("DefaultPhoneType");
            this.Initials = loader.LoadString("Initials");
            this.FullName = loader.LoadString("FullName");
            this.Bio = loader.LoadString("Bio");
            this.AvatarPath = loader.LoadString("AvatarPath");
            this.UseAvatar = loader.LoadBoolean("UseAvatar");
            this.NotificationEmailType = loader.LoadByte("NotificationEmailType");
            this.Title = loader.LoadString("Title");
            this.Role = loader.LoadInt16("Role");
            this.IsOnline = loader.LoadBoolean("IsOnline");

            this.Address = loader.LoadString("Address");
            this.GeoX = loader.LoadInt32("GeoX");
            this.GeoY = loader.LoadInt32("GeoY");

            this.LocationId = loader.LoadInt32("LocationId");

            this.Sex = loader.LoadByte("Sex");
            this.VideoPath = loader.LoadString("VideoPath");
            this.Birthday = loader.LoadDateTime("Birthday");
            this.IsPhoneConfirmed = loader.LoadBoolean("IsPhoneConfirmed");
            this.OpenID = loader.LoadString("OpenID");
            this.OpenSite = loader.LoadByte("OpenSite");
            this.IsEmailVerified4OpenID = loader.LoadBoolean("IsEmailVerified4OpenID");
            this.Credits = loader.LoadDouble("Credits");
        }
        public void CopyFields(User curUser)
        {
            this.ID = curUser.ID;
            this.FirstName = curUser.FirstName;
            this.LastName = curUser.LastName;
            this.Password = curUser.Password;
            this.Email = curUser.Email;
            this.EnterTime = curUser.EnterTime;
            this.ModifyTime = curUser.ModifyTime;
            this.IsConfirmed = curUser.IsConfirmed;
            this.CanChangeName = curUser.CanChangeName;
            this.LastLoginTime = curUser.LastLoginTime;
            this.ResetPasswordTime = curUser.ResetPasswordTime;
            this.SignUpIP = curUser.SignUpIP;
            this.SignUpBrowser = curUser.SignUpBrowser;
            this.ConfirmationToken = curUser.ConfirmationToken;
            this.TokenExpireTime = curUser.TokenExpireTime;
            this.PasswordChangeTime = curUser.PasswordChangeTime;
            this.PasswordFailedCount = curUser.PasswordFailedCount;
            this.PasswordFailedTime = curUser.PasswordFailedTime;
            this.ChangeToEmail = curUser.ChangeToEmail;
            this.Mobile = curUser.Mobile;
            this.PhoneAreaCode = curUser.PhoneAreaCode;
            this.PhoneLocalCode = curUser.PhoneLocalCode;
            this.DefaultPhoneType = curUser.DefaultPhoneType;
            this.Initials = curUser.Initials;
            this.FullName = curUser.FullName;
            this.Bio = curUser.Bio;
            this.AvatarPath = curUser.AvatarPath;
            this.UseAvatar = curUser.UseAvatar;
            this.NotificationEmailType = curUser.NotificationEmailType;
            this.Title = curUser.Title;
            this.Role = curUser.Role;
            this.IsOnline = curUser.IsOnline;
            this.Address = curUser.Address;
            this.GeoX = curUser.GeoX;
            this.GeoY = curUser.GeoY;
            this.LocationId = curUser.LocationId;

            this.LocationId = curUser.LocationId;
            this.Sex = curUser.Sex;
            this.Birthday = curUser.Birthday;
            this.IsPhoneConfirmed = curUser.IsPhoneConfirmed;
            this.OpenID = curUser.OpenID;
            this.OpenSite = curUser.OpenSite;
            this.IsEmailVerified4OpenID = curUser.IsEmailVerified4OpenID;
            this.Credits = curUser.Credits;
        }
        #endregion
    }
}
