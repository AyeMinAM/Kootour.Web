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
    public class TourExtraInfo
    {
        public int ID { get; set; }
        public string Name { get; set; }
        public int Price { get; set; }
        public int Times { get; set; }
        public TourTimeType TimeType { get; set; }
    }
   
    public class TourModel : Layout
    {
        //public Tour TourInfo { get; set; }
        public int TourID { get; set; }
        public int GuiderID { get; set; }
        public List<string> ImageUrls { get; set; }
        public string CoverImagePath { get; set; }
        public List<string> Inclusions { get; set; }
        public List<string> Exclusions { get; set; }
        public List<TourExtraInfo> Extras { get; set; }
        public bool IfShowExtras { get; set; }
        public List<TourPriceBreakdownModel> TourPriceBreakdowns { get; set; }

        public string Name { get; set; }
        public string Overview { get; set; }
        public string Itinerary { get; set; }
        public string Language { get; set; }

        public int MinTouristNum { get; set; }
        public int MaxTouristNum { get; set; }
        public int Duration { get; set; }
        public int DurationTimeType { get; set; }
        public int MinHourAdvance { get; set; }

        public string DurationString { get; set; }
        public string TourLocation { get; set; }
        public string TourLocationSimple { get; set; }
        public string MeetupLocation { get; set; }
        public string GuiderName { get; set; }
        public string GuiderIntro { get; set; }
        public string GuiderFromTime { get; set; }
        public string GuiderAvatarUrl { get; set; }

        public string BookDate { get; set; }
        public int TravellerCount { get; set; }
        public List<SelectListItem> TravellerCountOptions { get; set; }
        public string BookTime { get; set; }
        public List<SelectListItem> BookTimeOptions { get; set; }

        public double SugRetailPrice { get; set; }
        public double NowPrice { get; set; }
        public int DiscountTourists { get; set; }
        public int DiscountValue { get; set; }
        public float DiscountPercent { get; set; }

        public List<RecommendedReviews> Reviews { get; set; }
        
        public float SubTotal { get; set; }
        public float Total { get; set; }
        public string ExtraIds { get; set; }
        public float Tax { get; set; }

        public int TourReviewCount { get; set; }
        public double TourReviewAverageScore { get; set; }
        public byte BookingType { get; set; } 
        public bool IsDataSaved { get; set; }
        public int NextReviewPageNo { get; set; }
        public bool IsAvailable { get; set; }

        public string GuiderExcludedDatesListStr { get; set; }
        public string BeginDate { get; set; }
        public string EndDate { get; set; }
        public string IsSunday { get; set; }
        public string IsMonday { get; set; }
        public string IsTuesday { get; set; }
        public string IsWednesday { get; set; }
        public string IsThursday { get; set; }
        public string IsFriday { get; set; }
        public string IsSaturday { get; set; }

        public bool IsGuestBooking { get; set; }

    }
    public class BookTimeDropDownListModel 
    {
        public string BookTime { get; set; }
        public List<SelectListItem> BookTimeOptions { get; set; }
    }


}