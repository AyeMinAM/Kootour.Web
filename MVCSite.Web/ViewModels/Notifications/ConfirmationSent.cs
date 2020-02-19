using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MVCSite.Web.ViewModels
{
    public class ConfirmationSent : Layout
    {
        public bool RedirectToCustomPage { get; set; }
        public string ReturnUrl { get; set; }
    }
}