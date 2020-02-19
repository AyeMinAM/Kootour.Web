using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using MVCSite.DAC.Entities;
using System.ComponentModel.DataAnnotations;
using MVCSite.ViewResource;
namespace MVCSite.Web.ViewModels
{
    public class ReceivedBookingEvent
    {
        public string id { get; set; }
        public string title { get; set; }
        public string start { get; set; }
        public string end { get; set; }
        public string url { get; set; }
        public bool editable { get; set; }
        public bool allDay { get; set; }
    }

    public class SimpleReceivedBookingsModel
    {
        public string TourName { get; set; }
    }
    public class ReceivedBookingsModel : Layout
    {
        public List<SimpleReceivedBookingsModel> ReceivedBookings { get; set; }

    }

}