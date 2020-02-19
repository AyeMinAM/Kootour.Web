using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MVCSite.DAC.Entities;

namespace MVCSite.Biz.Models
{
     public interface IBookingModel
    {

        string ConfirmationCode { get; set; }
        string Email { get; set; }
        #region BookingInfo
        int TourID { get; set; }
        int TourUserID { get; set; }
        int BookingID { get; set; }
        string City { get; set; }
        string ImageUrl { get; set; }
        string TourName { get; set; }

        string TravelerName { get; set; }
        string TravelerEmail { get; set; }
        string TravelerPhoneAreaCode { get; set; }
        string TravelerMobile { get; set; }

        string GuideName { get; set; }
        int GuideID { get; set; }
        string GuideAvatarPath { get; set; }
        string GuideEmail { get; set; }
        string GuidePhoneAreaCode { get; set; }
        string GuideMobile { get; set; }

        string Date { get; set; }
        string Time { get; set; }
        string Location { get; set; }
        string MeetupLocation { get; set; }
        string TourLocationSimple { get; set; }
        float SubTotalPrice { get; set; }
        float ServiceFee { get; set; }
        int DiscountTourists { get; set; }
        int DiscountValue { get; set; }
        float DiscountPercent { get; set; }
        int Taxes { get; set; }
        byte BookingType { get; set; }

        float TotalPrice { get; set; }

        int TotalTravelers { get; set; }

        #region BookingConfirmationTourExtraInfo
        List<BookingConfirmationTourExtraInfo> Extras { get; set; }
        #endregion

        bool IsDataSaved { get; set; }
        float TotalCost { get; set; }
        float TourCost { get; set; }
        float VendorPromoTourCost { get; set; }
        bool IfShowBookingExtras { get; set; }
        TourPriceBreakdown TourPriceBreakdown { get; set; }
        int MinTouristNum { get; set; }
        int MaxTouristNum { get; set; }
        #endregion
    }
}
