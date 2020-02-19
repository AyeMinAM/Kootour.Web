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
    public enum ContactType
    {
        Services = 0,
        Feedback,
    };
    public class ContactTypeTranslation
    {
        public int Code;
        public string TypeString;
        public ContactTypeTranslation(ContactType code, string lan)
        {
            Code = (int)code;
            TypeString = lan;
        }
        public static List<ContactTypeTranslation> Translations = new List<ContactTypeTranslation>() 
        { 
            new ContactTypeTranslation(ContactType.Services, "General enquiry"), 
            new ContactTypeTranslation(ContactType.Feedback, "Tour enquiry"),
            new ContactTypeTranslation(ContactType.Feedback, "Feedback"),
            new ContactTypeTranslation(ContactType.Feedback, "Report an issue"),

            
        };
        public static string GetTranslationOf(ContactType code)
        {
            ContactTypeTranslation trans = Translations.Where(x => x.Code == (int)code).SingleOrDefault();
            if (trans == null)
                return string.Empty;
            return trans.TypeString;
        }
    };
    public class ContactModel : Layout
    {
        public int Topic { get; set; }
        [Required(ErrorMessageResourceName = "Required", ErrorMessageResourceType = typeof(ValidationStrings))]
        [StringLength(100, MinimumLength = 1, ErrorMessageResourceName = "StringLengthHint", ErrorMessageResourceType = typeof(ValidationStrings))]
        public string Name { get; set; }
        [Required(ErrorMessageResourceName = "Required", ErrorMessageResourceType = typeof(ValidationStrings))]
        [DataType(DataType.EmailAddress)]
        public string Email { get; set; }
        public string Phone { get; set; }
        [Required(ErrorMessageResourceName = "Required", ErrorMessageResourceType = typeof(ValidationStrings))]
        //[StringLength(1000, MinimumLength = 10, ErrorMessageResourceName = "StringLengthHint", ErrorMessageResourceType = typeof(ValidationStrings))]
        [StringLength(1000, ErrorMessageResourceName = "StringLengthHint", ErrorMessageResourceType = typeof(ValidationStrings))]       //modified by Anthony 05/03/2017
        public string Comment { get; set; }
        public IEnumerable<SelectListItem> TopicOptions { get; set; } 
    }

}