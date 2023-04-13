using AppiumSpecflowCSharp.Drivers;
using AppiumSpecflowCSharp.Models;
using AppiumSpecflowCSharp.Utilities;
using NUnit.Framework;
using OpenQA.Selenium.Appium;
using OpenQA.Selenium.Appium.Android;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;

namespace AppiumSpecflowCSharp.Tests
{
    public class SimpleYouTubeTest
    {
        private DriverManager driverManager;
        private AndroidDriver driver;
        private DriverConfiguration driverConfiguration = getConfig();


        private static DriverConfiguration getConfig()
        {
            LoadJsonData loadJsonData = new LoadJsonData();
            return loadJsonData.GetObjectFromFile<DriverConfiguration>("Pixel-XL-API28-Android-v12_YouTube.json");
        }


        [SetUp]
        public void Init()
        {
            driverManager = new DriverManager(driverConfiguration);
            driver = driverManager.GetDriver();
        }

        
        [Test]
        public void RunYouTube()
        {
            Wait(5);
            var magClass = driver.FindElement(MobileBy.AccessibilityId("Search"));
            magClass.Click();
            driver.FindElement(MobileBy.Id("search_edit_text")).SendKeys("Appium, C# and Visual Studio");
            driver.PressKeyCode(AndroidKeyCode.Enter);
            Wait(5);
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
