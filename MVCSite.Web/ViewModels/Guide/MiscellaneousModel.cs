
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using MVCSite.DAC.Entities;
using System.ComponentModel.DataAnnotations;
using MVCSite.ViewResource;
namespace MVCSite.Web.ViewModels
{

    public class MiscellaneousModel : Layout
    {
        public int TourID { get; set; }
        public int UserID { get; set; }
        public int GuideID { get; set; }
        public bool IsEmailAndPhoneVerified { get; set; }
        //public bool IsEmailVerified { get; set; }
        //public bool IsPhoneVerified { get; set; }
        public bool IfActive { get; set; }
    }

}
                          
                          