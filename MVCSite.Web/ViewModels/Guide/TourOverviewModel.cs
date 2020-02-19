
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
    public class TourOverviewModel : Layout
    {
        [AllowHtml]
        [Required(ErrorMessageResourceName = "Required", ErrorMessageResourceType = typeof(ValidationStrings))]
        [StringLength(3000, MinimumLength = 5, ErrorMessageResourceName = "StringLengthHint", ErrorMessageResourceType = typeof(ValidationStrings))]
        public string Overview { get; set; }

        [AllowHtml]
        [Required(ErrorMessageResourceName = "Required", ErrorMessageResourceType = typeof(ValidationStrings))]
        [StringLength(20000, MinimumLength = 5, ErrorMessageResourceName = "StringLengthHint", ErrorMessageResourceType = typeof(ValidationStrings))]
        public string Itinerary { get; set; }

        [Range(1, 99, ErrorMessageResourceName = "ValidNumber", ErrorMessageResourceType = typeof(ValidationStrings))] 
        public int Duration { get; set; }

        [Required]
        public int? DurationType { get; set; }
        public IEnumerable<SelectListItem> DurationTypeOptions { get; set; }

        //[Required(ErrorMessageResourceName = "Required", ErrorMessageResourceType = typeof(ValidationStrings))]
        //[StringLength(100, MinimumLength = 5, ErrorMessageResourceName = "StringLengthHint", ErrorMessageResourceType = typeof(ValidationStrings))]
        //[Required]
        //public int? TourCityID { get; set; }
        //public IEnumerable<SelectListItem> CityOptions { get; set; }

        [Display(Name = "Tour City")]
        public string TourCity { get; set; }
        public string TourCityHidden { get; set; }

        [Display(Name = "Meetup Location")]
        [Required(ErrorMessageResourceName = "Required", ErrorMessageResourceType = typeof(ValidationStrings))]
        [StringLength(100,MinimumLength = 1,ErrorMessageResourceName = "StringLengthHint",ErrorMessageResourceType = typeof(ValidationStrings))]
        public string MeetupLocation { get; set; }

        public List<TourInclusionExclusionModel> TourInclusions { get; set; }
        public List<TourInclusionExclusionModel> TourInclusionsExtra { get; set; }

        public List<TourInclusionExclusionModel> TourExclusions { get; set; }
        public List<TourInclusionExclusionModel> TourExclusionsExtra { get; set; }


        //[StringLength(300, MinimumLength = 1, ErrorMessageResourceName = "StringLengthHint", ErrorMessageResourceType = typeof(ValidationStrings))]
        //public string Inclusion1 { get; set; }
        ////[StringLength(100, ErrorMessage = "The {0} must be at least {2} characters long.", MinimumLength = 0)]
        //[StringLength(300, MinimumLength = 1, ErrorMessageResourceName = "StringLengthHint", ErrorMessageResourceType = typeof(ValidationStrings))]
        //public string Inclusion2 { get; set; }
        //[StringLength(300, MinimumLength = 1, ErrorMessageResourceName = "StringLengthHint", ErrorMessageResourceType = typeof(ValidationStrings))]
        //public string Inclusion3 { get; set; }
        //[StringLength(300, MinimumLength = 1, ErrorMessageResourceName = "StringLengthHint", ErrorMessageResourceType = typeof(ValidationStrings))]
        //public string Inclusion4 { get; set; }
        //[StringLength(300, MinimumLength = 1, ErrorMessageResourceName = "StringLengthHint", ErrorMessageResourceType = typeof(ValidationStrings))]
        //public string Inclusion5 { get; set; }

        //[StringLength(300, MinimumLength = 1, ErrorMessageResourceName = "StringLengthHint", ErrorMessageResourceType = typeof(ValidationStrings))]
        //public string Exclusion1 { get; set; }
        //[StringLength(300, MinimumLength = 1, ErrorMessageResourceName = "StringLengthHint", ErrorMessageResourceType = typeof(ValidationStrings))]
        //public string Exclusion2 { get; set; }
        //[StringLength(300, MinimumLength = 1, ErrorMessageResourceName = "StringLengthHint", ErrorMessageResourceType = typeof(ValidationStrings))]
        //public string Exclusion3 { get; set; }
        //[StringLength(300, MinimumLength = 1, ErrorMessageResourceName = "StringLengthHint", ErrorMessageResourceType = typeof(ValidationStrings))]
        //public string Exclusion4 { get; set; }
        //[StringLength(300, MinimumLength = 1, ErrorMessageResourceName = "StringLengthHint", ErrorMessageResourceType = typeof(ValidationStrings))]
        //public string Exclusion5 { get; set; }

        public int ID { get; set; }
        public int GuideID { get; set; }
    }

}
                          
                          