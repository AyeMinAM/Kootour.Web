using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MVCSite.Web.ViewModels
{
    public class MemberModel
    {
        public string ID { get; set; }
        public string CompanyID { get; set; }
        public string BoardID { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public string FullName { get; set; }
        public string Initials { get; set; }

        public bool IsChecked { get; set; }


    }
}