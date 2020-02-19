using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using MVCSite.DAC.Entities;
using System.ComponentModel.DataAnnotations;
using MVCSite.Biz;
using MVCSite.ViewResource;
using MVCSite.Biz.Models;
namespace MVCSite.Web.ViewModels
{

    public class BookingModel : Layout, IBookingModel
    {
        public int TourID { get; set; }
        public int TourUserID { get; set; }
        public int BookingID { get; set; }
        public string City { get; set; }
        public string ImageUrl { get; set; }
        public string TourName { get; set; }
        public string Duration { get; set; }
        public string TravelerName { get; set; }
        public string TravelerEmail { get; set; }
        public string TravelerPhoneAreaCode { get; set; }
        public string TravelerMobile { get; set; }
        public string TravelerAddress { get; set; }
        public string TravelerGender { get; set; }

        public string GuideName { get; set; }
        public int GuideID { get; set; }
        public string GuideAvatarPath { get; set; }
        public string GuidePhoneAreaCode { get; set; }
        public string GuideEmail { get; set; }
        public string GuideMobile { get; set; }

        public string Date { get; set; }
        public string Time { get; set; }
        public string Location { get; set; }
        public string MeetupLocation { get; set; }
        public string TourLocationSimple { get; set; }
        public byte BookingType { get; set; }
        public int TotalTravelers { get; set; }
        public float TotalCost { get; set; }
        public float TourCost { get; set; }
        public float VendorPromoTourCost { get; set; }
        //public List<TourExtraInfo> Extras { get; set; }
        public bool IfShowBookingExtras { get; set; }
        public TourPriceBreakdown TourPriceBreakdown { get; set; }
        public float SubTotalPrice { get; set; }
        public float ServiceFee { get; set; }
        public int DiscountTourists { get; set; }
        public int DiscountValue { get; set; }
        public float DiscountPercent { get; set; }
        public int Taxes { get; set; }
        public float PromoPrice { get; set; }
        public float TotalPrice { get; set; }
        public int MinTouristNum { get; set; }
        public int MaxTouristNum { get; set; }
        public bool IsDataSaved { get; set; }

        public bool IfShowPromoCodeBox { get; set; }
        public string ConfirmationCode { get; set; }
        public string Email { get; set; }
        public List<BookingConfirmationTourExtraInfo> Extras { get; set; }
        public PaymentModel PaymentModel { get; set; }
        public TravellerInformationModel TravellerInformationModel { get; set; }
    }

}