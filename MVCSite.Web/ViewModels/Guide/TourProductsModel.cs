using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using MVCSite.DAC.Entities;
using System.ComponentModel.DataAnnotations;
using MVCSite.ViewResource;
namespace MVCSite.Web.ViewModels
{
    public class SimpleTourModel
    {
        public int TourID { get; set; }
        public string TourName { get; set; }
        public string TourImageUrl { get; set; }
        public TourStatus Status { get; set; }
        public bool IfExpired { get; set;}
    }
    public class TourProductsModel : Layout
    {
        public List<SimpleTourModel> Tours { get; set; }

      }

}