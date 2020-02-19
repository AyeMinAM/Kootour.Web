using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using MVCSite.DAC.Entities;
using System.ComponentModel.DataAnnotations;
using MVCSite.ViewResource;
namespace MVCSite.Web.ViewModels
{
    public class BookingDetailsModel : Layout
    {
        public string PeopleIntakeValidMsg { get; set; }

        [Range(1, 20, ErrorMessageResourceName = "ValidNumber", ErrorMessageResourceType = typeof(ValidationStrings))]
        public int MinTouristNum { get; set; }
        [Range(1, 20, ErrorMessageResourceName = "ValidNumber", ErrorMessageResourceType = typeof(ValidationStrings))]
        public int MaxTouristNum { get; set; }

        public string BookingTypeValidMsg { get; set; }
        public byte BookingType { get; set; }

        [Range(1, 999, ErrorMessageResourceName = "ValidNumber", ErrorMessageResourceType = typeof(ValidationStrings))]
        public int MinHourAdvance { get; set; }

        public int ID { get; set; }
        public int GuideID { get; set; }
    }

}