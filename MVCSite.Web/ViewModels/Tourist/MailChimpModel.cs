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

    public class MailChimpModel
    {
        //public string email_address { get; set; }

        //public string status { get; set; }
        public string u { get; set; }
        public string id { get; set; }
        public string MERGE0 { get; set; }//Email
        public string MERGE1 { get; set; }//Name
        
    }

}