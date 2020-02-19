//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Web;
//using MVCSite.DAC.Entities;
//using System.ComponentModel.DataAnnotations;
//using MVCSite.ViewResource;
//namespace MVCSite.Web.ViewModels
//{
//    public class ReceivedBookingDetailsModel : Layout
//    {
        
//        public string TourName { get; set; }
//        public string Duration { get; set; }
//        public string Location { get; set; }
//        public string MeetupLocation { get; set; }

       
//        public string TouristName { get; set; }
//        public string ComeFrom { get; set; }
//        public string Gender { get; set; }
//        public string Email { get; set; }
//        public string Phone { get; set; }

        
//        public string Date { get; set; }
//        public string Time { get; set; }
//        public int PackagePriceperPerson { get; set; }
//        public int PackagePriceperGroup { get; set; }
//        public byte BookingType { get; set; }
//        public int TotalTravelers { get; set; }
//        public float TotalCost { get; set; }
//        public float TourCost { get; set; }
//        public float VendorPromoTourCost { get; set; }
//        public List<TourExtraInfo> Extras { get; set; }
//        public bool IfShowBookingExtras { get; set; }
//        public TourPriceBreakdown TourPriceBreakdown { get; set; }
//        public float SubTotalPrice { get; set; }
//        public float ServiceFee { get; set; }
//        public int DiscountTourists { get; set; }
//        public int DiscountValue { get; set; }
//        public float DiscountPercent { get; set; }
//        public int Taxes { get; set; }
//        public float PromoPrice { get; set; }
//        public float TotalPrice { get; set; }
//        public int MinTouristNum { get; set; }
//        public int MaxTouristNum { get; set; }
//        public bool IsDataSaved { get; set; }

//        public string ConfirmationCode { get; set; }
//    }

//}