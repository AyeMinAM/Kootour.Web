using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MVCSite.Web.ViewModels
{
    public class CreateMemberModel
    {
        public string CompanyID { get; set; }
        public string BoardID { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public string Title { get; set; }
        public string Phone { get; set; }
        public string Password { get; set; }
        public bool SyncGmailCalendar { get; set; }
    }
}