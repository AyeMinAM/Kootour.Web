using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MVCSite.Biz;
using MVCSite.Biz.Interfaces;
using MVCSite.DAC.Common;
using MVCSite.DAC.Instrumentation;
using MVCSite.DAC.Interfaces;
using MVCSite.DAC.Repositories;
using MVCSite.DAC.Services;
using MVCSite.Web.ViewModels;
using NUnit.Framework;

namespace MVCSite.Test
{
    class email_confirmation_unit
    {
        private StatDataContext statDataContext;
        private BookingConfirmationModel bookingConfirmationModel;
        private RepositoryStats repositoryStats;
        private EmailGenerator emailGenerator;
        private Emailer emailer;
        private String no_reply;
        private String subject;
        private String placeholder;
        

        [SetUp]
        public void TestInitialize()
        {
            // Set up connection to email server database
            var connectionString = ConfigurationManager.ConnectionStrings["statConnectionString"].ConnectionString;
            statDataContext = new StatDataContext(connectionString, "");
            repositoryStats = new RepositoryStats(statDataContext);

            emailGenerator = new EmailGenerator(new Logger());
            emailer = new Emailer(repositoryStats, new SiteConfiguration());

            no_reply = "noreply@kootour.com";

            bookingConfirmationModel = new BookingConfirmationModel(); 
            bookingConfirmationModel.UserName = "Kevin_Li";
            bookingConfirmationModel.ConfirmationCode = Guid.NewGuid().ToString().Substring(0, 8).ToUpper();
            bookingConfirmationModel.Email = "lkevin@kootour.com";

            // Set up test booking model
            bookingConfirmationModel.TourID = 100;
            bookingConfirmationModel.TourUserID = 200;
            bookingConfirmationModel.BookingID = 300;
            bookingConfirmationModel.City = "Vancouver";
            bookingConfirmationModel.ImageUrl = "link_to_image";
            bookingConfirmationModel.TourName = "2 Day Vancouver Tour";

            // Traveler info 
            bookingConfirmationModel.TravelerName = "Kevin Li";
            bookingConfirmationModel.TravelerEmail = "lkevin@kootour.com";
            bookingConfirmationModel.TravelerPhoneAreaCode = "604";
            bookingConfirmationModel.TravelerMobile = "5622331";

            // Guide info
            bookingConfirmationModel.GuideName = "Kevin Wong";
            bookingConfirmationModel.GuideID = 400;
            bookingConfirmationModel.GuideAvatarPath = "avatar_path";
            bookingConfirmationModel.GuideEmail = "wkevin@kootour.com";
            bookingConfirmationModel.GuidePhoneAreaCode = "604";
            bookingConfirmationModel.GuideMobile = "1111111";

            // Tour info

            bookingConfirmationModel.Date = "1-16-2017";
            bookingConfirmationModel.Time = "4:00 PM";
            bookingConfirmationModel.Location = "BC Tech Innovation Hub";
            bookingConfirmationModel.MeetupLocation = "BC Tech Innovation Hub";
            bookingConfirmationModel.TourLocationSimple = "Offices";
            bookingConfirmationModel.SubTotalPrice = 500;
            bookingConfirmationModel.ServiceFee = 15;
            bookingConfirmationModel.DiscountTourists = 0;
            bookingConfirmationModel.DiscountValue = 0;
            bookingConfirmationModel.DiscountPercent = 0;
            bookingConfirmationModel.Taxes = 0;
            bookingConfirmationModel.BookingType = 0;
            bookingConfirmationModel.TotalPrice = 515;
            bookingConfirmationModel.TotalTravelers = 1;
            
            bookingConfirmationModel.IsDataSaved = true;
            bookingConfirmationModel.TotalCost = 515;
            bookingConfirmationModel.Extras = null;

            subject = "Booking Confirmation" +
                             string.Format(" - {0} - {1}", bookingConfirmationModel.City,
                                 bookingConfirmationModel.TourName);
            placeholder = "Placeholder body";


        }
        

        [Test]
        public void emailTestKevinWong()
        {
           // var accountManagerString = emailGenerator.GetAccountManagerBookingConfirmationEmailString(bookingConfirmationModel);
            //emailer.SendHtml(no_reply, "wkevin@kootour.com", subject, placeholder);

            var queuedEmails = repositoryStats.QueuedEmailsGetAll().OrderBy(item => item.EnterTime);
            var last = queuedEmails.Last();
            Assert.AreEqual(last.Sender, "noreply@kootour.com");
            Assert.AreEqual(last.Receiver, "wkevin@kootour.com");
            Assert.AreEqual(last.Title, subject);

            
        }

        [Test]
        public void emailTestTraveler()
        {

            emailer.SendHtml(no_reply, bookingConfirmationModel.TravelerEmail, subject, placeholder);

            var queuedEmails = repositoryStats.QueuedEmailsGetAll().OrderBy(item => item.EnterTime);
            var last = queuedEmails.Last();
            Assert.AreEqual(last.Sender, "noreply@kootour.com");
            Assert.AreEqual(last.Receiver, bookingConfirmationModel.TravelerEmail);
            Assert.AreEqual(last.Title, subject);
        }

        [Test]
        public void emailTestGuide()
        {

            emailer.SendHtml(no_reply, bookingConfirmationModel.GuideEmail, subject, placeholder);

            var queuedEmails = repositoryStats.QueuedEmailsGetAll().OrderBy(item => item.EnterTime);
            var last = queuedEmails.Last();
            Assert.AreEqual(last.Sender, "noreply@kootour.com");
            Assert.AreEqual(last.Receiver, bookingConfirmationModel.GuideEmail);
            Assert.AreEqual(last.Title, subject);


        }

    }
}
