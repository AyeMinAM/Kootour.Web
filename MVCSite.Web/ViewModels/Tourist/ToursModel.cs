using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using MVCSite.DAC.Entities;
using System.ComponentModel.DataAnnotations;
using MVCSite.ViewResource;
using System.Web.Mvc;
using MVCSite.DAC.Instrumentation;
namespace MVCSite.Web.ViewModels
{
    public enum SearchMode
    {
        ByCity = 0,
        ByCategory = 1,
        ByCountry = 2
    }

    public class SelectType
    {
        public string Text { get; set; }
        public string Value { get; set; }
    }
//public enum BannerImageMode
//{
//    UseCityImage = 0,
//    UseCategoryImage = 1,
//    UseCountryImage = 2,
//    UseDefaultImage = 3
//}
public class ToursModel : Layout
    {
        public string BannerTitle { get; set; }
        public SearchMode SearchMode { get; set; }
        public int SelectCityID { get; set; }
        public string SelectCity { get; set; }
        [Required(ErrorMessageResourceName = "Required", ErrorMessageResourceType = typeof(ValidationStrings))]
        //[StringLength(10, MinimumLength = 10, ErrorMessageResourceName = "StringLengthHint", ErrorMessageResourceType = typeof(ValidationStrings))]
        [RegularExpression(@"^[0-9]{2}\/[0-9]{2}\/[0-9]{4}$", ErrorMessage = "Please input valid date")]
        public string SelectDate { get; set; }
        public int Duration { get; set; }
        public int SortByPrice { get; set; }
        public int CategoryID { get; set; }
        public List<SelectListItem> CategoryOptions { get; set; }
        public int LanguageID { get; set; }
        public List<SelectListItem> LanguageOptions { get; set; }
        public List<Tour> Tours { get; set; }
        
        public string BannerImageName { get; set; }
        public string Name { get; set; }
        public string Intro { get; set; }
        public string Tip1 { get; set; }
        public string Tip2 { get; set; }
        public string Tip3 { get; set; }
        public string Tip4 { get; set; }
        public string Tip5 { get; set; }
        public string Tip6 { get; set; }
        public List<City> DestinationList { get; set; }
        public string BlogList { get; set; }
    }

}