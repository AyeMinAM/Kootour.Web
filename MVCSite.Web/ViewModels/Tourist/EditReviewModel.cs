using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using MVCSite.DAC.Entities;
using System.ComponentModel.DataAnnotations;
using MVCSite.ViewResource;
using System.Web.Mvc;
namespace MVCSite.Web.ViewModels
{
    public class EditReviewModel : Layout
    {
        public int Accuracy { get; set; }
        public int Communication { get; set; }
        public int Services { get; set; }
        public int Knowledge { get; set; }
        public int Value { get; set; }
        //[AllowHtml]
        [Required(ErrorMessageResourceName = "Required", ErrorMessageResourceType = typeof(ValidationStrings))]
        [StringLength(1024, MinimumLength = 5, ErrorMessageResourceName = "StringLengthHint", ErrorMessageResourceType = typeof(ValidationStrings))]        
        public string Comment { get; set; }

        public bool IsReviewAdded { get; set; }
    }

}