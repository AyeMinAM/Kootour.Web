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
    public class TourTypeModel : Layout
    {
        [Required(ErrorMessageResourceName = "Required", ErrorMessageResourceType = typeof(ValidationStrings))]
        [StringLength(100, MinimumLength = 1, ErrorMessageResourceName = "StringLengthHint", ErrorMessageResourceType = typeof(ValidationStrings))]
        public string Name { get; set; }
        public string TypeValidMsg { get; set; }
        public string LanguageValidMsg { get; set; }

        public bool IsHistorical { get; set; }
        public bool IsAdventure { get; set; }
        public bool IsLeisureSports { get; set; }
        public bool IsCultureArts { get; set; }
        public bool IsNatureRural { get; set; }
        public bool IsFestivalEvents { get; set; }
        public bool IsNightlifeParty { get; set; }
        public bool IsFoodDrink { get; set; }
        public bool IsShoppingMarket { get; set; }
        public bool IsTransportation { get; set; }
        public bool IsBusinessInterpretation { get; set; }
        public bool IsPhotography { get; set; }
        public int[] LanguageIDs { get; set; }
        public List<SelectListItem> LanguageOptions { get; set; }
        public int ID { get; set; }
        public int GuideID { get; set; }
    }

}