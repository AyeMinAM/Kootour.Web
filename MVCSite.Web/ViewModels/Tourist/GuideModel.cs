using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using MVCSite.DAC.Entities;
using System.ComponentModel.DataAnnotations;
using MVCSite.ViewResource;
namespace MVCSite.Web.ViewModels
{
    public class TourListingInfo
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
    public class RecommendedReviews
    {
        public int TourId { get; set; }
        public string AvatarUrl { get; set; }
        public string UserName { get; set; }        
        public double AverageScore { get; set; }
        public string ReviewTime { get; set; }
        public string Comment { get; set; }      
    }
    public class GuiderModel : Layout
    {
        //public int TourID { get; set; }
        public int GuiderID { get; set; }
        public string GuiderName { get; set; }

        public int ReviewCount { get; set; }
        public double ReviewAverageScore { get; set; }

        public DateTime GuiderFromTime { get; set; }
        public string GuiderAvatarUrl { get; set; }          
        public string GuiderLocation { get; set; }
        public Boolean IsEmailConfirmed { get; set; }
        public Boolean IsPhoneConfirmed { get; set; }

        public string GuiderIntroduction { get; set; }
        public string GuiderVideoURL { get; set; }

        public int TourCount { get; set; }

        public List<TourListingInfo> Tours { get; set; }
        public List<RecommendedReviews> Reviews { get; set; }
        public List<string> Languages;
    }

}