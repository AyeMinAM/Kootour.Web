using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using MVCSite.DAC.Entities;
using System.ComponentModel.DataAnnotations;
using MVCSite.Biz;
using MVCSite.ViewResource;
using MVCSite.Biz.Models;
namespace MVCSite.Web.ViewModels
{

    public class PromotionLandingModel : Layout
    {
        [Display(Name="Full Name")]
        public string FullName { get; set; }

        [Required(ErrorMessageResourceName = "Required", ErrorMessageResourceType = typeof(ValidationStrings))]
        [EmailAddress]
        public string Email { get; set; }
        
    }

}