using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using MVCSite.DAC.Entities;


namespace MVCSite.DAC.Services
{
    public class PriceService
    {

        public PriceService()
        {

        }
        public static double CalculateVendorPromotedPrice(double nowPrice, IEnumerable<TourVendorPromo> tourVendorPromoSubset)
        {
            foreach (var vendorPromo in tourVendorPromoSubset)
            {
                if (vendorPromo.BeginDate <= DateTime.Now && DateTime.Now <= vendorPromo.EndDate)
                {
                    var promoValue = vendorPromo.PromoValue;
                    var promoPercent = vendorPromo.PromoPercent;
                    if (promoValue > 0)
                    {
                        return nowPrice - promoValue;
                    }
                    if (promoPercent > 0)
                    {
                        return Math.Round((1 - promoPercent / 100) * nowPrice);
                    }
                }
            }

            return nowPrice;
        }

    }
}