using AppiumSpecflowCSharp.Drivers;
using AppiumSpecflowCSharp.Models;
using AppiumSpecflowCSharp.Utilities;
using NUnit.Framework;
using OpenQA.Selenium.Appium.Android;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;

namespace AppiumSpecflowCSharp.Tests
{
    public class SimpleChromeTest
    {
        private DriverManager driverManager;
        private AndroidDriver driver;
        private DriverConfiguration driverConfiguration = getConfig();

        private static DriverConfiguration getConfig()
        {
            LoadJsonData loadJsonData = new LoadJsonData();
            return loadJsonData.GetObjectFromFile<DriverConfiguration>("Galaxy-Nexus-Android-v12-Chrome.json");
        }


        [SetUp]
        public void Init()
        {
            driverManager = new DriverManager(driverConfiguration);
            driver = driverManager.GetDriver();
        }

        [Test]
        public void RunBrowserChrome()
        {
            Wait(5);
            driver.Navigate().GoToUrl("https://www.google.com");
            Wait(5);
            driver.CloseApp();
        }


        [TearDown]
        public void TearDown()
        {
            if (driver != null)
            {
                driver.Quit();
            }
        }

        private void Wait(double seconds = 3)
        {
            Thread.Sleep(TimeSpan.FromSeconds(seconds));
        }
    }
}
