using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using MVCSite.DAC.Entities;
using System.ComponentModel.DataAnnotations;
using MVCSite.ViewResource;
namespace MVCSite.Web.ViewModels
{
    public class PaymentModel : Layout
    {
        public int BookID { get; set; }

        public string CardNumber { get; set; }
        public string ExpirationMonth { get; set; }
        public string ExpirationYear { get; set; }
        public string CVC { get; set; }
        public string PostalCode { get; set; }
        public bool RememberMe { get; set; }
        public string PhoneNumber { get; set; }
        public double Price { get; set; }
        public string StripePublishableKey { get; set; }
        public string StripeToken { get; set; }
        public string StripeEmail { get; set; }
        public string TourName { get; set; }
    }

}