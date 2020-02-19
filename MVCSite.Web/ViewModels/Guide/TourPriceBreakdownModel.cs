
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
    public class TourPriceBreakdownModel
    {
        public TourPriceBreakdownModel() { }

        public TourPriceBreakdownModel(TourPriceBreakdown tourPriceBreakdown)
        {
            this.ID = tourPriceBreakdown.ID;
            this.TourID = tourPriceBreakdown.TourID;
            this.EndPoint1 = tourPriceBreakdown.EndPoint1;
            this.EndPoint2 = tourPriceBreakdown.EndPoint2;
            this.DiscountValue = tourPriceBreakdown.DiscountValue;
            this.DiscountPercent = tourPriceBreakdown.DiscountPercent;
            this.SortNo = tourPriceBreakdown.SortNo;
            this.BeginDate = tourPriceBreakdown.BeginDate;
            this.EndDate = tourPriceBreakdown.EndDate;
            this.DateRange = tourPriceBreakdown.DateRange;
            this.EnterTime = tourPriceBreakdown.EnterTime;
            this.ModifyTime = tourPriceBreakdown.ModifyTime;
        }


        public int ID { get; set; }

        public int TourID { get; set; }

        [Display(Name="Tourist lower bound")]
        //[StringLength(2, MinimumLength = 1, ErrorMessageResourceName = "StringLengthHint", ErrorMessageResourceType = typeof(ValidationStrings))]
        public int EndPoint1 { get; set; }
        [Display(Name = "Tourist upper bound")]
        public int EndPoint2 { get; set; }
        [Display(Name = "Discount Value")]
        public Nullable<int> DiscountValue { get; set; }
        [Display(Name = "Discount Percentage")]
        public Nullable<double> DiscountPercent { get; set; }

        public byte SortNo { get; set; }
        public DateTime BeginDate { get; set; }

        public DateTime EndDate { get; set; }

        public string DateRange { get; set; }

        public DateTime EnterTime { get; set; }

        public DateTime ModifyTime { get; set; }
    }
}
                          
                          