
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using MVCSite.DAC.Entities;
using System.ComponentModel.DataAnnotations;
using MVCSite.ViewResource;
namespace MVCSite.Web.ViewModels
{
    public class SinglePicture
    {
        public string Url { get; set; }
        public int ID { get; set; }
        public int TourID { get; set; }

    }
    public class PicturesModel : Layout
    {
        public List<SinglePicture> Pictures { get; set; }
        public int TourID { get; set; }
        public string ValidateInfo { get; set; }
    }

}
                          
                          