using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using MVCSite.DAC.Entities;
using System.ComponentModel.DataAnnotations;
using MVCSite.ViewResource;
namespace MVCSite.Web.ViewModels
{
        public class ExcludedDatesModel : Layout
        {
            public string ExcludedDates { get; set; }
            public string OrderedDates { get; set; }
        }
    
}