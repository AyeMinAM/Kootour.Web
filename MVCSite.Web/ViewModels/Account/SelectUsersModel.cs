using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using MVCSite.DAC.Entities;
using System.Web.Mvc;

namespace MVCSite.Web.ViewModels
{
    public class SelectUsersModel
    {
        public short ConferenceUserType { get; set; }
        public List<SelectListItem> ConferenceUserTypeOptions { get; set; }

        public int CompanyID { get; set; }
        public int ConferenceID { get; set; }
        public IEnumerable<User> SelectedUsers { get; set; }     
        public IEnumerable<User> GlobalUsers { get; set; }        
    }


}