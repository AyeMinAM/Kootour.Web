
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
    public class TourOverviewActiveModel : Layout
    {
        [AllowHtml]
        [Required(ErrorMessageResourceName = "Required", ErrorMessageResourceType = typeof(ValidationStrings))]
        [StringLength(3000, MinimumLength = 5, ErrorMessageResourceName = "StringLengthHint", ErrorMessageResourceType = typeof(ValidationStrings))]
        public string Overview { get; set; }

        [AllowHtml]
        [Required(ErrorMessageResourceName = "Required", ErrorMessageResourceType = typeof(ValidationStrings))]
        [StringLength(10000, MinimumLength = 5, ErrorMessageResourceName = "StringLengthHint", ErrorMessageResourceType = typeof(ValidationStrings))]
        public string Itinerary { get; set; }

        [Range(0, 365, ErrorMessageResourceName = "ValidNumber", ErrorMessageResourceType = typeof(ValidationStrings))]
        [Required(ErrorMessageResourceName = "Required", ErrorMessageResourceType = typeof(ValidationStrings))]
        public string EndDate { get; set; }

        public int TourID { get; set; }
    }

}
                          
                          