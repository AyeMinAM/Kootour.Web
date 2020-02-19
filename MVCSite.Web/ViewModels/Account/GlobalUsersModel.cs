using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using MVCSite.DAC.Entities;

namespace MVCSite.Web.ViewModels
{
    public class GlobalUsersModel
    {
        public User NewUser { get; set; }
        public IEnumerable<User> CurrentUsers { get; set; }
        public bool IsCreate
        {
            get;
            set;
        }      
    }


}