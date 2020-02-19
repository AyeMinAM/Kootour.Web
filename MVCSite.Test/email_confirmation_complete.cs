using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MVCSite.Biz;
using MVCSite.DAC.Common;
using MVCSite.DAC.Entities;
using MVCSite.DAC.Instrumentation;
using MVCSite.DAC.Repositories;
using NUnit.Framework;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Support.UI;
using TechTalk.SpecFlow;
using Assert = Microsoft.VisualStudio.TestTools.UnitTesting.Assert;

namespace MVCSite.Test
{
    class email_confirmation_complete
    {
        private IWebDriver driver;
        private String baseUrl;

        private StringBuilder verificationErrors;

        private StatDataContext statDataContext;
        private BookingConfirmationModel bookingConfirmationModel;

        private GuideDataContext guideDataContext;
        private RepositoryGuides repositoryGuides;
        private RepositoryStats repositoryStats;
        private EmailGenerator emailGenerator;
        private string HtmlFilesPath = @"D:/EmailText";

        [SetUp]
        public void TestInitialize()
        {
            verificationErrors = new StringBuilder();

            // EmailServer DB

            var statConnectionString = ConfigurationManager.ConnectionStrings["statConnectionString"].ConnectionString;
            statDataContext = new StatDataContext(statConnectionString, "");
            repositoryStats = new RepositoryStats(statDataContext);

            // KootourFront DB

            var connectionString = ConfigurationManager.ConnectionStrings["kootourConnectionString"].ConnectionString;
            guideDataContext = new GuideDataContext(connectionString, "");
            repositoryGuides = new RepositoryGuides(new Logger(), guideDataContext,
                new HttpCacheProvider());

            // Clear all bookings
            //repositoryGuides.UserTourBookingDeleteAll();

            // Email Generator
            emailGenerator = new EmailGenerator(new Logger());

            bookingConfirmationModel = new BookingConfirmationModel();
            bookingConfirmationModel.MinTouristNum = 2;
            bookingConfirmationModel.MaxTouristNum = 9;
            bookingConfirmationModel.UserName = "Username";
            bookingConfirmationModel.ConfirmationCode = "4D7A943B";
            bookingConfirmationModel.Email = "email";

            // Set up test booking model
            bookingConfirmationModel.TourID = 185;
            bookingConfirmationModel.TourUserID = 0;
            bookingConfirmationModel.BookingID = 156;
            bookingConfirmationModel.City = "Vancouver,BC,Canada";
            bookingConfirmationModel.ImageUrl = "https://media.kootour.com/Tour/1997/20161130103035.43506913.jpg";
            bookingConfirmationModel.TourName = "Test Tour";

            // Traveler info 
            bookingConfirmationModel.TravelerName = "Hansen Yi";
            bookingConfirmationModel.TravelerEmail = "Yhansen@kootour.com";
            bookingConfirmationModel.TravelerPhoneAreaCode = "1";
            bookingConfirmationModel.TravelerMobile = "7788834431";

            // Guide info
            bookingConfirmationModel.GuideName = "Kevin Wong";
            bookingConfirmationModel.GuideID = 84;
            bookingConfirmationModel.GuideAvatarPath = "";
            bookingConfirmationModel.GuideEmail = "wkevin@kootour.com";
            bookingConfirmationModel.GuidePhoneAreaCode = "1";
            bookingConfirmationModel.GuideMobile = "7789525828";

            // Tour info

            bookingConfirmationModel.Date = "1/17/2018";
            bookingConfirmationModel.Time = "8:00 AM";
            bookingConfirmationModel.Location = "Vancouver, BC, Canada";
            bookingConfirmationModel.MeetupLocation = "Meetup Location";
            bookingConfirmationModel.TourLocationSimple = "Canada-Vancouver";
            bookingConfirmationModel.SubTotalPrice = 1000;
            bookingConfirmationModel.ServiceFee = (float)390;
            //bookingConfirmationModel.DiscountTourists = 0;
            //bookingConfirmationModel.DiscountValue = 0;
            //bookingConfirmationModel.DiscountPercent = 0;
            bookingConfirmationModel.Taxes = 0;
            bookingConfirmationModel.BookingType = 0;
            bookingConfirmationModel.TotalPrice = (float)1370;
            bookingConfirmationModel.TotalTravelers = 5;
            bookingConfirmationModel.IsDataSaved = false;
            bookingConfirmationModel.TourCost = 150;
            bookingConfirmationModel.VendorPromoTourCost = 140;
            var extras = new List<BookingConfirmationTourExtraInfo>();
            var extra = new BookingConfirmationTourExtraInfo();
            extra.ID = 1012;
            extra.Name = "Option 1";
            extra.Price = 50;
            extra.TimeType = TourTimeType.Hours;
            extra.Times = 1;
            extras.Add(extra);
            extra = new BookingConfirmationTourExtraInfo();
            extra.ID = 1013;
            extra.Name = "Option 2";
            extra.Price = 10;
            extra.TimeType = TourTimeType.Hours;
            extra.Times = 2;
            extras.Add(extra);

            bookingConfirmationModel.Extras = extras;


            var tourPriceBreakdown = new TourPriceBreakdown();
            tourPriceBreakdown.BeginDate = DateTime.MinValue;
            tourPriceBreakdown.EndDate = DateTime.MaxValue;
            tourPriceBreakdown.EndPoint1 = 1;
            tourPriceBreakdown.EndPoint2 = 10;
            
            tourPriceBreakdown.DiscountValue = 5;

            bookingConfirmationModel.TourPriceBreakdown = tourPriceBreakdown;
        }

        [TearDown]
        public void TestTearDown()
        {
            try
            {
                driver.Quit();
            }
            catch (Exception)
            {
                // Ignore errors if unable to close the browser
            }
            Assert.AreEqual("", verificationErrors.ToString());

        }

        [Test]
        public void LoginAndGoToPaymentPage()
        {
            driver = new ChromeDriver();
            baseUrl = "https://localhost";
            var testTourUrl = "https://localhost/Tourist/Tour/185?calendar=01%2F16%2F2018";

            driver.Navigate().GoToUrl(baseUrl);
            driver.FindElement(By.LinkText("Log In")).Click();
            driver.FindElement(By.Id("Email")).Clear();
            driver.FindElement(By.Id("Email")).SendKeys("wkevin@kootour.com");
            driver.FindElement(By.Id("Password")).Clear();
            driver.FindElement(By.Id("Password")).SendKeys("abcABC!@#123");
            driver.FindElement(By.XPath("//button[@type='submit']")).Click();
            driver.Navigate().GoToUrl(testTourUrl);

            new SelectElement(driver.FindElement(By.Id("TravellerCount"))).SelectByText("5");
            driver.FindElement(By.CssSelector("#BookTourForm > div:nth-child(20) > div.option-checkbox > label"))
                .Click();
            driver.FindElement(By.CssSelector("#BookTourForm > div:nth-child(21) > div.option-checkbox > label"))
                .Click();
            driver.FindElement(By.CssSelector("#BookTourForm > div:nth-child(22) > div.option-checkbox > label"))
                .Click();

            driver.FindElement(By.Id("BookSubmitButton")).Click();

            WebDriverWait wait = new WebDriverWait(driver, TimeSpan.FromMinutes(1));

            Func<IWebDriver, bool> waitForElement = new Func<IWebDriver, bool>((IWebDriver Web) =>
            {
                Console.WriteLine(Web.FindElement(By.XPath("//*[@id=\"widget-row-promo-form\"]/div[2]/div")));
                return true;
            });
            wait.Until(waitForElement);

            driver.FindElement(By.Id("promoValue")).Clear();
            driver.FindElement(By.Id("promoValue")).SendKeys("kootour20");
            driver.FindElement(By.Id("promoButton")).Click();
            driver.FindElement(By.XPath("//a/span")).Click();
            driver.FindElement(By.XPath("//*[@id=\"payment-form\"]/div[1]/div[3]/p/button")).Click();
            //WebDriverWait wait2 = new WebDriverWait(driver, TimeSpan.FromMinutes(1));

            //Func<IWebDriver, bool> waitForElement2 = new Func<IWebDriver, bool>((IWebDriver Web) =>
            //{
            //    Console.WriteLine(Web.FindElement(By.XPath("/html/body/div/div/div/section/span/p")));
            //    return true;
            //});
            //wait2.Until(waitForElement2);
        }

        [Test]
        public void retrieveEmailHTMLAccountManager()
        {

            var html = emailGenerator.GetAccountManagerBookingConfirmationEmailString(bookingConfirmationModel);
            var guid = Guid.NewGuid();
            var path = HtmlFilesPath + @"\AccountManagerBooking_" + guid + ".html";
            System.IO.File.WriteAllText(path, html);
            System.Diagnostics.Process.Start(path);
        }

        [Test]
        public void retrieveEmailHTMLTraveler()
        {

            var html = emailGenerator.GetTravelerBookingConfirmationEmailString(bookingConfirmationModel);
            var guid = Guid.NewGuid();
            var path = HtmlFilesPath + @"\TravelerBooking_" + guid + ".html";
            System.IO.File.WriteAllText(path, html);
            System.Diagnostics.Process.Start(path);
        }

        [Test]
        public void retrieveEmailHTMLGuide()
        {

            var html = emailGenerator.GetGuideBookingConfirmationEmailString(bookingConfirmationModel);
            var guid = Guid.NewGuid();
            var path = HtmlFilesPath + @"\GuideBooking_" + guid + ".html";
            System.IO.File.WriteAllText(path, html);
            System.Diagnostics.Process.Start(path);
        }


    }
}
