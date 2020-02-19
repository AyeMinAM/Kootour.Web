using System;
using System.Text;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using OpenQA.Selenium;
using OpenQA.Selenium.Firefox;
using OpenQA.Selenium.Support.UI;
using TechTalk.SpecFlow;
using TechTalk.SpecFlow.Assist;

namespace MVCSite.Test
{
    [Binding]
    [Scope(Feature ="AdminUpdateTour")]
    public class AdminUpdateTourSteps
    {
        private IWebDriver driver;
        private StringBuilder verificationErrors;
        private string baseURL;
        private bool acceptNextAlert = true;

        [BeforeScenario]
        public void BeforeScenario()
        {
            driver = new FirefoxDriver();
            baseURL = "https://localhost/Admin/TourType/81";
            verificationErrors = new StringBuilder();
        }

        [AfterScenario]
        public void AfterScenario()
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
        [Given(@"I have gone to the first tour editing page ""(.*)""")]
        public void GivenIHaveGoneToTheFirstTourEditingPage(string p0)
        {
            driver.Navigate().GoToUrl(baseURL + "/Account/LogOn/?ReturnUrl=%2fAdmin%2fTourType%2f81");
            driver.FindElement(By.Id("Email")).Clear();
            driver.FindElement(By.Id("Email")).SendKeys("yhansen@kootour.com");
            driver.FindElement(By.Id("Password")).Clear();
            driver.FindElement(By.Id("Password")).SendKeys("12345678");
        }
        
        [Given(@"I have clicked next button to ""(.*)"" Page")]
        public void GivenIHaveClickedNextButtonToPage(string p0)
        {
            driver.FindElement(By.XPath("//button[@type='submit']")).Click();
            driver.FindElement(By.XPath("//button[@type='submit']")).Click();
            driver.FindElement(By.XPath("//button[@type='submit']")).Click();
            driver.FindElement(By.XPath("//form[@id='BookingDetailsForm']/div[2]/button")).Click();
            driver.FindElement(By.XPath("//button[@type='submit']")).Click();
        }
        
        [When(@"I press next")]
        public void WhenIPressNext()
        {
            driver.FindElement(By.XPath("//button[@type='submit']")).Click();
        }
        
        [Then(@"the url should be changed to /Admin/Console/Tour")]
        public void ThenTheUrlShouldBeChangedToAdminConsoleTour()
        {
            Assert.AreEqual(driver.Url, "https://localhost/Admin/Console/Tour");
        }
    }
}
