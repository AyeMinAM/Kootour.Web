
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
    public class TourVendorPromoModel
    {
        public TourVendorPromoModel() { }

        public TourVendorPromoModel(TourVendorPromo tourVendorPromo)
        {
            this.ID = tourVendorPromo.ID;
            this.PromoName = tourVendorPromo.PromoName;
            this.PromoValue = tourVendorPromo.PromoValue;
            this.PromoPercent = tourVendorPromo.PromoPercent;
            this.SortNo = tourVendorPromo.SortNo;
            this.OpenToUse = tourVendorPromo.OpenToUse;
            this.MinTouristsToUse = tourVendorPromo.MinTouristsToUse;
            this.MinValueToUse = tourVendorPromo.MinValueToUse;
            this.BeginDate = tourVendorPromo.BeginDate;
            this.EndDate = tourVendorPromo.EndDate;
            this.DateRange = tourVendorPromo.DateRange;
            this.EnterTime = tourVendorPromo.EnterTime;
            this.ModifyTime = tourVendorPromo.ModifyTime;
            this.TourID = tourVendorPromo.TourID;
            this.GuideID = tourVendorPromo.GuideID;
        }
        public int ID { get; set; }
        public string PromoName { get; set; }
        [Display(Name = "Promotion Value")]
        public int PromoValue { get; set; }
        [Display(Name = "Promotion Percentage")]
        public double PromoPercent { get; set; }
        public byte SortNo { get; set; }
        public bool? OpenToUse { get; set; }
        public int? MinTouristsToUse { get; set; }
        public int? MinValueToUse { get; set; }
        public DateTime BeginDate { get; set; }
        public DateTime EndDate { get; set; }
        public string DateRange { get; set; }
        public DateTime EnterTime { get; set; }
        public DateTime ModifyTime { get; set; }
        public int? TourID { get; set; }
        public int? GuideID { get; set; }
    }
}
                          
                          