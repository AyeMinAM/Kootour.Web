using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using MVCSite.DAC.Entities;

namespace MVCSite.Web.ViewModels
{
    public class MyVoicesModel
    {

    } 
    public class VoicesModel : Layout
    {
        public MyVoicesModel MyVoices;
        public AccountNavigateModel Navigate { get; set; }
        public string Audio { get; set; }
    } 
}