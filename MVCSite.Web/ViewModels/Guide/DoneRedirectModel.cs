
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using MVCSite.DAC.Entities;
using System.ComponentModel.DataAnnotations;
using System.Web.Routing;
using DotNetOpenAuth.OpenId.Extensions.AttributeExchange;
using MVCSite.ViewResource;
namespace MVCSite.Web.ViewModels
{

    public class DoneRedirect
    {
        public string DoneControllerName { get; set; }
        public string DoneActionName { get; set; }
    }
}
                          
                          