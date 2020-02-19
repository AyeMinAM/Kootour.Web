using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using MVCSite.DAC.Entities;
namespace MVCSite.Web.ViewModels
{
    public class InternalErrorModel : Layout
    {
        public string Referrer { get; set; }
    }
    public class MInternalErrorModel : CommunityLayout
    {
        public string Referrer { get; set; }
    }
}