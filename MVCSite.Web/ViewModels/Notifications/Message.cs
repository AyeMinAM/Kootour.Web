using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MVCSite.Web.ViewModels
{
    public class Message : Layout
    {
        public string Title { get; set; }
        public string Text { get; set; }
        public bool Result { get; set; }
        public string Referrer { get; set; }
    }
}