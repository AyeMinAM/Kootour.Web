using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MVCSite.Web.ViewModels
{
    public class SNSLogOnDataModel
    {
        public string Result { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }
        public string NickName { get; set; }
        public string OpenId
        {
            get;
            set;
        }
    }
}