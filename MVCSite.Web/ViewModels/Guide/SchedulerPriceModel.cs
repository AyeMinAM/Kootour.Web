
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
    public class SchedulerPriceModel : Layout
    {
        //[Required(ErrorMessageResourceName = "Required", ErrorMessageResourceType = typeof(ValidationStrings))]
        [StringLength(100, MinimumLength = 0, ErrorMessageResourceName = "StringLengthHint", ErrorMessageResourceType = typeof(ValidationStrings))]
        public string ScheduleName { get; set; }

        [Required(ErrorMessageResourceName = "Required", ErrorMessageResourceType = typeof(ValidationStrings))]   
        public string Daterange { get; set; }
        //[Required(ErrorMessageResourceName = "Required", ErrorMessageResourceType = typeof(ValidationStrings))]       
        //public string BgnDate { get; set; }
        //[Required(ErrorMessageResourceName = "Required", ErrorMessageResourceType = typeof(ValidationStrings))]        
        //public string EndDate { get; set; }

        public string AvailableDaysValidMsg { get; set; }

        public bool IsMonday { get; set; }
        public bool IsTuesday { get; set; }
        public bool IsWednesday { get; set; }
        public bool IsThursday { get; set; }
        public bool IsFriday { get; set; }
        public bool IsSaturday { get; set; }
        public bool IsSunday { get; set; }

        public string ExtraTourValidMsg { get; set; }

        [StringLength(100, MinimumLength = 0, ErrorMessageResourceName = "StringLengthHint1", ErrorMessageResourceType = typeof(ValidationStrings))]
        public string ExtraNames1 { get; set; }
        [StringLength(100, MinimumLength = 0, ErrorMessageResourceName = "StringLengthHint1", ErrorMessageResourceType = typeof(ValidationStrings))]
        public string ExtraNames2 { get; set; }
        [StringLength(100, MinimumLength = 0, ErrorMessageResourceName = "StringLengthHint1", ErrorMessageResourceType = typeof(ValidationStrings))]
        public string ExtraNames3 { get; set; }
        [StringLength(100, MinimumLength = 0, ErrorMessageResourceName = "StringLengthHint1", ErrorMessageResourceType = typeof(ValidationStrings))]
        public string ExtraNames4 { get; set; }
        [StringLength(100, MinimumLength = 0, ErrorMessageResourceName = "StringLengthHint1", ErrorMessageResourceType = typeof(ValidationStrings))]
        public string ExtraNames5 { get; set; }

        [Range(-1000, 9999, ErrorMessageResourceName = "ValidNumber", ErrorMessageResourceType = typeof(ValidationStrings))]
        public int ExtraPrices1 { get; set; }
        [Range(-1000, 9999, ErrorMessageResourceName = "ValidNumber", ErrorMessageResourceType = typeof(ValidationStrings))]
        public int ExtraPrices2 { get; set; }
        [Range(-1000, 9999, ErrorMessageResourceName = "ValidNumber", ErrorMessageResourceType = typeof(ValidationStrings))]
        public int ExtraPrices3 { get; set; }
        [Range(-1000, 9999, ErrorMessageResourceName = "ValidNumber", ErrorMessageResourceType = typeof(ValidationStrings))]
        public int ExtraPrices4 { get; set; }
        [Range(-1000, 9999, ErrorMessageResourceName = "ValidNumber", ErrorMessageResourceType = typeof(ValidationStrings))]
        public int ExtraPrices5 { get; set; }

        [Range(0, 100, ErrorMessageResourceName = "ValidNumber", ErrorMessageResourceType = typeof(ValidationStrings))]
        public int ExtraTimes1 { get; set; }
        [Range(0, 100, ErrorMessageResourceName = "ValidNumber", ErrorMessageResourceType = typeof(ValidationStrings))]
        public int ExtraTimes2 { get; set; }
        [Range(0, 100, ErrorMessageResourceName = "ValidNumber", ErrorMessageResourceType = typeof(ValidationStrings))]
        public int ExtraTimes3 { get; set; }
        [Range(0, 100, ErrorMessageResourceName = "ValidNumber", ErrorMessageResourceType = typeof(ValidationStrings))]
        public int ExtraTimes4 { get; set; }
        [Range(0, 100, ErrorMessageResourceName = "ValidNumber", ErrorMessageResourceType = typeof(ValidationStrings))]
        public int ExtraTimes5 { get; set; }

        public int ExtraTimesType1 { get; set; }
        public int ExtraTimesType2 { get; set; }
        public int ExtraTimesType3 { get; set; }
        public int ExtraTimesType4 { get; set; }
        public int ExtraTimesType5 { get; set; }

        //public int TimesTypeOptions { get; set; }
        public IEnumerable<SelectListItem> TimesTypeOptions { get; set; }

        public string DiscountLargeGroupValidMsg { get; set; }

        [Range(0, 20, ErrorMessageResourceName = "ValidNumber", ErrorMessageResourceType = typeof(ValidationStrings))]
        public int DiscountTourists { get; set; }
        [Range(0, 99999, ErrorMessageResourceName = "ValidNumber", ErrorMessageResourceType = typeof(ValidationStrings))]
        public int DiscountValue { get; set; }
        [Range(0, 100, ErrorMessageResourceName = "ValidNumber", ErrorMessageResourceType = typeof(ValidationStrings))]
        public float DiscountPercent { get; set; }    

        public string ELStartTimeValidMsg { get; set; }
        [Required(ErrorMessageResourceName = "Required", ErrorMessageResourceType = typeof(ValidationStrings))]
        [StringLength(10, MinimumLength = 0, ErrorMessageResourceName = "StringLengthHint", ErrorMessageResourceType = typeof(ValidationStrings))]
        public string StartTime1 { get; set; }
        //[Required(ErrorMessageResourceName = "Required", ErrorMessageResourceType = typeof(ValidationStrings))]
        //[StringLength(10, MinimumLength = 0, ErrorMessageResourceName = "StringLengthHint", ErrorMessageResourceType = typeof(ValidationStrings))]
        public string StartTime2 { get; set; }
        public string StartTime3 { get; set; }        
        public string StartTime4 { get; set; }        
        public string StartTime5 { get; set; }
        public string StartTime6 { get; set; }

        [Required(ErrorMessageResourceName = "Required", ErrorMessageResourceType = typeof(ValidationStrings))]
        [Range(1, 99999, ErrorMessageResourceName = "ValidNumber", ErrorMessageResourceType = typeof(ValidationStrings))]
        public float NetPrice { get; set; }

        [Required(ErrorMessageResourceName = "Required", ErrorMessageResourceType = typeof(ValidationStrings))]
        [Range(1, 99999, ErrorMessageResourceName = "ValidNumber", ErrorMessageResourceType = typeof(ValidationStrings))]
        public float SugRetailPrice { get; set; }

        //[Range(10, 30, ErrorMessageResourceName = "ValidNumber", ErrorMessageResourceType = typeof(ValidationStrings))]
        public float CommisionPay { get; set; }

        public List<TourPriceBreakdownModel> TourPriceBreakdowns { get; set; }
        public List<TourPriceBreakdownModel> TourPriceBreakdownsExtra { get; set; }
        public List<TourVendorPromoModel> TourVendorPromos { get; set; }
        public List<TourVendorPromoModel> TourVendorPromosExtra { get; set; }
        public bool IfShowTourPriceBreakdowns { get; set; }
        public int MinTouristNum { get; set; }
        public int MaxTouristNum { get; set; }
        public int ID { get; set; }
        public int TourID { get; set; }
    }

}
                          
                          