using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MVCSite.Web.ViewModels
{
    public class CreateBoard : Layout
    {
        public string Title { get; set; }
        public int CompanyID { get; set; }
        public int Visibility { get; set; }        
    }
}