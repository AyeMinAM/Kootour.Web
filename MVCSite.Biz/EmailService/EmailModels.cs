using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MVCSite.Biz.Models;
using MVCSite.DAC.Entities;

namespace MVCSite.Biz
{
    public class EmailModel
    {
        public string UserName { get; set; }
    }
    public class EmailConfirmation : EmailModel
    {
        public string ConfirmationLink { get; set; }
        public string Password { get; set; }
    }
    public class EmailForgotPassword : EmailModel
    {
        public string ConfirmationLink { get; set; }
    }

    public class UserAddedNotification : EmailModel
    {
        public string Email { get; set; }
        public string Password { get; set; }
    }
    public class ActivityNotification : EmailModel
    {
        public string Who { get; set; }
        public string Action { get; set; }
        public string Object { get; set; }
        public string Where { get; set; }
        public string When { get; set; }
    }

    public class EmailInviteFriend : EmailModel
    {
        public string ConfirmationLink { get; set; }
        public string BoardName { get; set; }
    }
    public class PromotionModel : EmailModel
    {
        public string PromotionLink { get; set; }
        public string PromotionName { get; set; }
    }
    public class EmailUserContactCSR : EmailModel
    {
        public string Topic { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
        public string Comment { get; set; }
    }

    public class BookingConfirmationModel: EmailModel, IBookingModel
    {
        public string ConfirmationCode { get; set; }
        public string Email { get; set; }
        #region BookingInfo
        public int TourID { get; set; }
        public int TourUserID { get; set; }
        public int BookingID { get; set; }
        public string City { get; set; }
        public string ImageUrl { get; set; }
        public string TourName { get; set; }

        public string TravelerName { get; set; }
        public string TravelerEmail { get; set; }
        public string TravelerPhoneAreaCode { get; set; }
        public string TravelerMobile { get; set; }

        public string GuideName { get; set; }
        public int GuideID { get; set; }
        public string GuideAvatarPath { get; set; }
        public string GuideEmail { get; set; }
        public string GuidePhoneAreaCode { get; set; }
        public string GuideMobile { get; set; }

        public string Date { get; set; }
        public string Time { get; set; }
        public string Location { get; set; }
        public string MeetupLocation { get; set; }
        public string TourLocationSimple { get; set; }
        public float SubTotalPrice { get; set; }
        public float ServiceFee { get; set; }
        public int DiscountTourists { get; set; }
        public int DiscountValue { get; set; }
        public float DiscountPercent { get; set; }
        public int Taxes { get; set; }
        public byte BookingType { get; set; }

        public float TotalPrice { get; set; }

        public int TotalTravelers { get; set; }

        #region BookingConfirmationTourExtraInfo
        public List<BookingConfirmationTourExtraInfo> Extras { get; set; }
        #endregion

        public bool IsDataSaved { get; set; }
        public float TotalCost { get; set; }
        public float TourCost { get; set; }
        public float VendorPromoTourCost { get; set; }
        public bool IfShowBookingExtras { get; set; }
        public TourPriceBreakdown TourPriceBreakdown { get; set; }
        public int MinTouristNum { get; set; }
        public int MaxTouristNum { get; set; }
        public string TempPassword { get; set; }
        #endregion
    }
    public class BookingConfirmationTourExtraInfo
    {
        public int ID { get; set; }
        public string Name { get; set; }
        public int Price { get; set; }
        public int Times { get; set; }
        public TourTimeType TimeType { get; set; }
    }
}
