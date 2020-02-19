using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Globalization;
//using System.Web.Mvc;
using System.Web.Security;
using MVCSite.ViewResource;
using MVCSite.DAC.Extensions;
using MVCSite.Common;
using MVCSite.DAC.Entities;
using System.Web.Mvc;
using Compare = System.Web.Mvc.CompareAttribute;

namespace MVCSite.Web.ViewModels
{
    public enum LogonMode
    {
        NormalLogon = 0,
        InviteeLogon=1,
        JoinFavoriteLogon=2,
        AndroidMobileLogon=3,
        iPhoneLogon = 4,
        //The following from Unity
        OSXEditor = 100,
        OSXPlayer = 101,
        WindowsPlayer = 102,
        OSXWebPlayer = 103,
        OSXDashboardPlayer = 104,
        WindowsWebPlayer = 105,
        WiiPlayer = 106,
        WindowsEditor = 107,
        //IPhonePlayer = 8,
        PS3 = 109,
        XBOX360 = 110,
        //Android = 11,
        NaCl = 112,
        LinuxPlayer = 113,
        FlashPlayer = 115,
        MetroPlayerX86 = 118,
        MetroPlayerX64 = 119,
        MetroPlayerARM = 120,
        WP8Player = 121,
        BB10Player = 122,
    }

    //[PropertiesMustMatch("Password", "ConfirmPassword", 
    //ErrorMessageResourceName = "PasswordsMustMatch", 
    //ErrorMessageResourceType = typeof(ValidationStrings))]
    public class ChangePasswordModel:Layout
    {
        [Display(Name = "Old Password")]
        //[Required(ErrorMessageResourceName = "Required", ErrorMessageResourceType = typeof(ValidationStrings))]
        //[DataType(DataType.Password)]
        ////[Display(Name = "Current password")]
        //[LocalizedDisplayName("OldPassword", NameResourceType = typeof(ValidationStrings))]
        public string OldPassword { get; set; }

        [Display(Name = "New Password")]
        [Required(ErrorMessageResourceName = "Required", ErrorMessageResourceType = typeof(ValidationStrings))]
        [StringLength(100, ErrorMessageResourceName = "StringLengthHint", ErrorMessageResourceType = typeof(ValidationStrings), MinimumLength = 8)]
        [DataType(DataType.Password)]
        //[Display(Name = "New password")]
        [LocalizedDisplayName("NewPassword", NameResourceType = typeof(ValidationStrings))]
        public string NewPassword { get; set; }

        [Display(Name = "Confirm Password")]
        [DataType(DataType.Password)]
        //[Display(Name = "Confirm new password")]
        [LocalizedDisplayName("ConfirmNewPassword", NameResourceType = typeof(ValidationStrings))]
        [Compare("NewPassword", ErrorMessageResourceName = "ConfirmationPasswordNotMatch", ErrorMessageResourceType = typeof(ValidationStrings))]
        public string ConfirmPassword { get; set; }
    }
    public class ChangePasswordSuccessModel : Layout
    {
    }                                      
    public class JSLogOnModel
    {
        public string loginId { get; set; } 
        public int loginType { get; set; }
        public string email { get; set; } 
    }
    public class JSRegisterModel
    {
        public string loginId { get; set; }
        public byte loginType { get; set; }
        public string firstName { get; set; }
        public string lastName { get; set; }
        public string email { get; set; }
        public string password { get; set; }
        public string location { get; set; }
        public string avatar { get; set; }
        public int Role { get; set; }
    }
    public class LogOnModel : Layout
    {

        [Required(ErrorMessageResourceName = "Required", ErrorMessageResourceType = typeof(ValidationStrings))]
        [StringLength(100, ErrorMessage = "The {0} must be at least {2} characters long.", MinimumLength = 2)]
        //[LocalizedDisplayName("Email", NameResourceType = typeof(ValidationStrings))]
        public string Email { get; set; }

        [Required(ErrorMessageResourceName = "Required", ErrorMessageResourceType = typeof(ValidationStrings))]
        [StringLength(100, ErrorMessage = "The {0} must be at least {2} characters long.", MinimumLength = 8)]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        [Display(Name = "Remember me?")]
        public bool RememberMe { get; set; }


        public string InviteToken { get; set; }
        public int From { get; set; }
        public string HoldId { get; set; } //Not changed when switching between logon and register.
        public string DeviceID { get; set; }

        public string WeChatUrl { get; set; }
        public Nullable<LanguageCode> Lan { get; set; }
    }

    public class TravellerInformationModel : Layout
    {
       

        [Display(Name = "First Name")]
        [Required(ErrorMessageResourceName = "Required", ErrorMessageResourceType = typeof(ValidationStrings))]
        [StringLength(100, ErrorMessage = "The {0} must be at least {2} characters long.", MinimumLength = 2)]
        public string FirstName { get; set; }

        [Display(Name = "Last Name")]
        [Required(ErrorMessageResourceName = "Required", ErrorMessageResourceType = typeof(ValidationStrings))]
        [StringLength(100, ErrorMessage = "The {0} must be at least {2} characters long.", MinimumLength = 2)]
        public string LastName { get; set; }

        [Required(ErrorMessageResourceName = "Required", ErrorMessageResourceType = typeof(ValidationStrings))]
        [DataType(DataType.EmailAddress, ErrorMessageResourceType = typeof(ValidationStrings))]
        public string Email { get; set; }

        [DataType(DataType.PhoneNumber)]
        [RegularExpression(@"^\(?([0-9]{3})\)?[-. ]?([0-9]{3})[-. ]?([0-9]{4})$", ErrorMessageResourceName = "InvalidPhoneNumber", ErrorMessageResourceType = typeof(ValidationStrings))]
        public string PhoneNumber { get; set; }

        public bool IfWantNewsletter { get; set; }               
    }

    public class RegisterModel : Layout
    {
        [Display(Name = "First Name")]
        [Required(ErrorMessageResourceName = "Required", ErrorMessageResourceType = typeof(ValidationStrings))]
        [StringLength(100, ErrorMessage = "The {0} must be at least {2} characters long.", MinimumLength = 2)]
        public string FirstName { get; set; }

        [Display(Name = "Last Name")]
        [Required(ErrorMessageResourceName = "Required", ErrorMessageResourceType = typeof(ValidationStrings))]
        [StringLength(100, ErrorMessage = "The {0} must be at least {2} characters long.", MinimumLength = 2)]
        public string LastName { get; set; }

        [Required(ErrorMessageResourceName = "Required", ErrorMessageResourceType = typeof(ValidationStrings))]
        [DataType(DataType.EmailAddress)]
        public string Email { get; set; }

        [Required(ErrorMessageResourceName = "Required", ErrorMessageResourceType = typeof(ValidationStrings))]
        [StringLength(100, ErrorMessage = "The {0} must be at least {2} characters long.", MinimumLength = 8)]
        //[ValidatePasswordLength(ErrorMessageResourceName = "PasswordMinLength",ErrorMessageResourceType = typeof(ValidationStrings))]
        //[StringLength(ErrorMessageResourceName = "PasswordMinLength", ErrorMessageResourceType = typeof(ValidationStrings))]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        [Display(Name = "Re-enter Password")]
        [Required(ErrorMessageResourceName = "Required", ErrorMessageResourceType = typeof(ValidationStrings))]
        [StringLength(100, ErrorMessage = "The {0} must be at least {2} characters long.", MinimumLength = 8)]
        [DataType(DataType.Password)]
        public string ConfirmPassword { get; set; }

        public bool IsAgreed { get; set; }

        public string NickName { get; set; } 
        //public string ReturnUrl { get; set; }
        public bool SendConfirmEmail { get; set; }
        public string InviteToken { get; set; }
        public int From { get; set; }
        public string HoldId { get; set; }

        public LanguageCode Language { get; set; }
        public string WeChatUrl { get; set; }

        public bool IfWantNewsletter { get; set; }

    }
    public class ConfirmEmailModel : Layout
    {
        [Required(ErrorMessageResourceName = "Required", ErrorMessageResourceType = typeof(ValidationStrings))]
        [LocalizedDisplayName("Email", NameResourceType = typeof(ValidationStrings))]
        public string Email { get; set; }
        public string DoneControllerName { get; set; }
        public string DoneActionName { get; set; }
    }
    public class RememberNewPasswordModel : Layout
    {
        public string Email { get; set; }
        public string NewPassword { get; set; }
    }
    public class ConfirmEmailSendModel : Layout
    {
        public DoneRedirect DoneRedirect { get; set; }
    }
    public class ConfirmPhoneModel : Layout
    {
        [Display(Name = "Phone Number")]
        [Required(ErrorMessageResourceName = "Required", ErrorMessageResourceType = typeof(ValidationStrings))]
        [StringLength(100, MinimumLength = 5, ErrorMessageResourceName = "StringLengthHint", ErrorMessageResourceType = typeof(ValidationStrings))]
        [DataType(DataType.PhoneNumber)]
        //[RegularExpression(@"^\(?([0-9]{3})\)?[-. ]?([0-9]{3})[-. ]?([0-9]{4})$", ErrorMessageResourceName = "InvalidPhoneNumber", ErrorMessageResourceType = typeof(ValidationStrings))]
        public string PhoneNumber { get; set; }
        public IEnumerable<SelectListItem> AreaCodeOptions { get; set; }
        public string AreaCode { get; set; }
        public string DoneControllerName { get; set; }
        public string DoneActionName { get; set; }
    }
    public class ConfirmPhoneSendDigitModel : Layout
    {
        [Display(Name = "Digit Code")]
        [Required(ErrorMessageResourceName = "Required", ErrorMessageResourceType = typeof(ValidationStrings))]
        [StringLength(100, MinimumLength = 4, ErrorMessageResourceName = "StringLengthHint", ErrorMessageResourceType = typeof(ValidationStrings))]
        public string DigitCode { get; set; }
        public string DoneControllerName { get; set; }
        public string DoneActionName { get; set; }
    }
    public class ConfirmPhoneDoneModel : Layout
    {
        public DoneRedirect DoneRedirect { get; set; }
    }
    public class SaveUserInfoModel
    {

        public int MazeLevel { get; set; }
        public  int BookID
        {
            get;
            set;
        }

        public  byte ReciteType
        {
            get;
            set;
        }

        public  byte ReviewType
        {
            get;
            set;
        }
        public  int Score
        {
            get;
            set;
        }

        public  byte WordCount
        {
            get;
            set;
        }

        public  int WordWrongCount
        {
            get;
            set;
        }

        public  int SpendSeconds
        {
            get;
            set;
        }

        public  byte DamagedLifeCount
        {
            get;
            set;
        }

        public  short KillMonsterCount
        {
            get;
            set;
        }

        public  byte WaitTime
        {
            get;
            set;
        }

        public  byte PronounceTime
        {
            get;
            set;
        }
        public int UnitID
        {
            get;
            set;
        }
    }

}
