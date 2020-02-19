using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Web.Mvc;
using System.IO;
using MVCSite.DAC.Common;
using System.Data;
using MVCSite.Common;

namespace MVCSite.DAC.Entities
{

    public enum UserTourBookingStatus
    {
        Initial = 0,
        Paid,
        Reviewed,

    };
    public enum TopicType
    {
        Feedback = 0,
        Services,        

    };
    //public class TopicTypeTranslation
    //{
    //    public int Code;
    //    public string TopicTypes;
    //    public TopicTypeTranslation(TopicType code, string lan)
    //    {
    //        Code = (int)code;
    //        TopicTypes = lan;
    //    }
    //    public static List<TopicTypeTranslation> Translations = new List<TopicTypeTranslation>() 
    //    { 
    //        new TopicTypeTranslation(TopicType.Feedback , "Feedback"), 
    //        new TopicTypeTranslation(TopicType.Services, "Services"), 
            
    //    };
    //    public static string GetTranslationOf(TopicType code)
    //    {
    //        TopicTypeTranslation trans = Translations.Where(x => x.Code == (int)code).SingleOrDefault();
    //        if (trans == null)
    //            return string.Empty;
    //        return trans.TopicTypes ;
    //    }
    //};


    public partial class UserTourBooking
    {
        #region Constructors
        public UserTourBooking()
        {
            this.ID = 0;
            this.TourID = 0;
            this.UserID = 0;
            this.Calendar = string.Empty;
            this.Travellers = 0;
            this.Time = string.Empty;
            this.EnterTime = DateTime.UtcNow;
            this.ModifyTime = DateTime.UtcNow;
            this.TourUserID = 0;
            this.TourName = string.Empty;
            this.CalendarStart = DateTime.UtcNow;
            this.CalendarEnd = DateTime.UtcNow;
        }
        public UserTourBooking(UserTourBooking src)
        {
            this.ID = src.ID;
            this.TourID = src.TourID;
            this.UserID = src.UserID;
            this.Calendar = src.Calendar;
            this.Time = src.Time;
            this.Travellers = src.Travellers;
            this.EnterTime = src.EnterTime;
            this.ModifyTime = src.ModifyTime;
            this.TourUserID = src.TourUserID;
            this.TourName = src.TourName;
            this.CalendarStart = src.CalendarStart;
            this.CalendarEnd = src.CalendarEnd;

            this.TourImgPath = src.TourImgPath;
            this.GuiderName = src.GuiderName;
            this.Location = src.Location;
            this.Status = src.Status;
            this.SubTotal = src.SubTotal;
            this.Taxes = src.Taxes;
            this.DiscountValue = src.DiscountValue;
            this.DiscountTourist = src.DiscountTourist;
            this.DiscountPercent = src.DiscountPercent;
            this.PromoPrice = src.PromoPrice;
            this.TotalPay = src.TotalPay;
            this.ExtraIds = src.ExtraIds;

        }
        /// <summary>
        /// Constructor using IDataRecord.
        /// </summary>
        public UserTourBooking(IDataRecord record)
        {
            FieldLoader loader = new FieldLoader(record);

            this.ID = loader.LoadInt32("ID");
            this.TourID = loader.LoadInt32("TourID");
            this.UserID = loader.LoadInt32("UserID");
            this.Calendar = loader.LoadString("Calendar");
            this.Time = loader.LoadString("Time");
            this.Travellers = loader.LoadInt32("Travellers");
            this.EnterTime = loader.LoadDateTime("EnterTime");
            this.ModifyTime = loader.LoadDateTime("ModifyTime");
            this.TourUserID = loader.LoadInt32("TourUserID");
            this.TourName = loader.LoadString("TourName");
            this.CalendarStart = loader.LoadDateTime("CalendarStart");
            this.CalendarEnd = loader.LoadDateTime("CalendarEnd");

            this.TourImgPath = loader.LoadString("TourImgPath");
            this.GuiderName = loader.LoadString("GuiderName");
            this.Location = loader.LoadString("Location");
            this.Status = loader.LoadByte("Status");
            this.SubTotal = loader.LoadDouble("SubTotal");
            this.Taxes = loader.LoadDouble("Taxes");
            this.DiscountValue = loader.LoadDouble("DiscountValue");
            this.DiscountTourist = loader.LoadByte("DiscountTourist");
            this.DiscountPercent = loader.LoadDouble("DiscountPercent");
            this.ServiceFee = loader.LoadDouble("ServiceFee");
            this.PromoPrice = loader.LoadDouble("PromoPrice");
            this.TotalPay = loader.LoadDouble("TotalPay");
            this.ExtraIds = loader.LoadString("ExtraIds");

            
        }

        #endregion
        public bool IsReviewed
        {
            get;
            set;
        }
        public byte TourBookingType
        {
            get;
            set;
        }
        public string TourImgUrl
        {
            get
            {
                if (string.IsNullOrEmpty(TourImgPath))
                {
                    return string.Empty;
                }
                return Path.Combine(StaticSiteConfiguration.ImageServerUrl, TourImgPath);
            }
        } 
    }
}
