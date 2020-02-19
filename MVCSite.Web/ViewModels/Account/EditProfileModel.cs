using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using MVCSite.DAC.Entities;
using System.ComponentModel.DataAnnotations;
using MVCSite.ViewResource;
using System.Web.Mvc;
namespace MVCSite.Web.ViewModels
{
    public class EditProfileModel : Layout
    {
        [Required(ErrorMessageResourceName = "Required", ErrorMessageResourceType = typeof(ValidationStrings))]
        [StringLength(100, ErrorMessage = "The {0} must be at least {2} characters long.", MinimumLength = 2)]
        public string FirstName { get; set; }
        [Required(ErrorMessageResourceName = "Required", ErrorMessageResourceType = typeof(ValidationStrings))]
        [StringLength(100, ErrorMessage = "The {0} must be at least {2} characters long.", MinimumLength = 2)]
        public string LastName { get; set; }
        
        public string Location { get; set; }

        public byte Gender { get; set; }
        public IEnumerable<SelectListItem> YearOptions { get; set; }
        public IEnumerable<SelectListItem> MonthOptions { get; set; }
        public IEnumerable<SelectListItem> DayOptions { get; set; }
        public int BirthDateMonth { get; set; }
        public int BirthDateDay { get; set; }
        public int BirthDateYear { get; set; }

        //[Required(ErrorMessageResourceName = "Required", ErrorMessageResourceType = typeof(ValidationStrings))]
        //[StringLength(100, MinimumLength = 5, ErrorMessageResourceName = "StringLengthHint", ErrorMessageResourceType = typeof(ValidationStrings))]
        public string Email{ get; set; }
        //[Required(ErrorMessageResourceName = "Required", ErrorMessageResourceType = typeof(ValidationStrings))]
        //[StringLength(100, MinimumLength = 5, ErrorMessageResourceName = "StringLengthHint", ErrorMessageResourceType = typeof(ValidationStrings))]
        public string PhoneNumber { get; set; }
        public string MobilePhone { get; set; }
        public string AvatarUrl { get; set; }

        [AllowHtml]
        [StringLength(4096, MinimumLength = 0, ErrorMessageResourceName = "StringLengthHint", ErrorMessageResourceType = typeof(ValidationStrings))]
        public string Introduction { get; set; }

        [StringLength(100, MinimumLength = 0, ErrorMessageResourceName = "StringLengthHint", ErrorMessageResourceType = typeof(ValidationStrings))]
        public string VideoURL { get; set; }

        public bool IsEmailVerified { get; set; }
        public bool IsPhoneVerified { get; set; }
    }

}