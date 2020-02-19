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
    public class IndexModel : Layout
    {
        public int SelectCityID { get; set; }
        public string SelectCity { get; set; }
        public string SelectCityHidden { get; set; }
        public IEnumerable<SelectListItem> CityOptions { get; set; }
        public string SelectDate { get; set; }
        public bool IsMobile { get; set; }
        public IPageable<Tour> FeaturedTours { get; set; }
    }

}