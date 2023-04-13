using AppiumSpecflowCSharp.Drivers;
using NUnit.Framework;
using OpenQA.Selenium;
using OpenQA.Selenium.Appium;
using OpenQA.Selenium.Appium.Android;
using OpenQA.Selenium.Appium.Enums;
using System.Collections.Generic;
using System.Linq;
using System.Threading;

namespace AppiumSpecflowCSharp.PageObjects
{
    public class GoogleView : BaseView
    {
        private AndroidDriver driver;
        public GoogleView(AndroidDriver androidDriver): base(androidDriver)
        {
            driver = androidDriver;
        }
        public void NavigateToGoogleHome()
        {
            driver.Navigate().GoToUrl("https://www.google.com");
            SwitchToWebView();
            Wait(5);
        }

        public void EnterSearchBox(string search)
        {
            //WaitFor(ElementState.ElementExists, by);
            Wait(3);
            var test = driver.FindElement(By.XPath("//input[1]"));
            Wait(1);
            test.SendKeys(search);
            Wait(1);
        }

        public void ClickSearch()
        {
            driver.PressKeyCode(AndroidKeyCode.Enter);
            Wait(10);
        }

        public bool HasSearchResult()
        {
            // wrapper for list
            AppiumElement result = driver.FindElement(By.Id("rso"));
            return result.Displayed;
        }
    }
}
