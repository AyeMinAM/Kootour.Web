using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using MVCSite.DAC.Entities;
using System.ComponentModel.DataAnnotations;
using MVCSite.ViewResource;
namespace MVCSite.Web.ViewModels
{
    public class TourWishInfo
    {
        public int TourId { get; set; }
        public string TourName { get; set; }
        public double SugRetailPrice { get; set; }
        public double NowPrice { get; set; }
        public int ReviewCount { get; set; }
        public double ReviewAverageScore { get; set; }
        public string ImageUrl { get; set; }
        public string PerPersonOrGroup { get; set; }


    }
    public class TourPurchase
    {
        public int BookingId { get; set; }
        public int TourId { get; set; }
        public string TourName { get; set; }
        public string GuiderName { get; set; }
        public string ImageUrl { get; set; }
        public string Date { get; set; }
        public string Time { get; set; }
        public string Location { get; set; }
        public int TravellerCount { get; set; }
        public string SubTotal { get; set; }
        public string Taxes { get; set; }
        public string Discount { get; set; }
        public string TotalPay { get; set; }
        public List<TourExtraInfo> Extras { get; set; }
        public bool IsReviewAdded  {get; set; }
        public int DiscountTourists { get; set; }
        public int DiscountValue { get; set; }
        public float DiscountPercent { get; set; }        
        public byte BookingType { get; set; }
        public double PromoPrice { get; set; }
        public double ServiceFee { get; set; }
    }
    public class TouristModel : Layout
    {
        public string UserName { get; set; }
        public string AvatarUrl { get; set; }
        public double Credits { get; set; }
        public Boolean IsEmailConfirmed { get; set; }
        public Boolean IsPhoneConfirmed { get; set; }
        public int WishesCount { get; set; }
        public int PurchasesCount { get; set; }

        public List<TourWishInfo> Wishes { get; set; }
        public List<TourPurchase> Purchases { get; set; }

    }

}