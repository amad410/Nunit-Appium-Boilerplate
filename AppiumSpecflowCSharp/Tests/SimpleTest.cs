using AppiumSpecflowCSharp.Drivers;
using NUnit.Framework;
using OpenQA.Selenium;
using OpenQA.Selenium.Appium.Android;
using System;
using System.Threading;
using AppiumSpecflowCSharp.Models;
using OpenQA.Selenium.Appium;
using System.IO;
using AppiumSpecflowCSharp.Utilities;
using AppiumSpecflowCSharp.PageObjects;

namespace AppiumSpecflowCSharp.Tests
{
    // ToDo: 
    // Support MultiThreading
    // Create Page Objects or Page Views using page factory
    public class SimpleTest
    {
        private DriverManager driverManager;
        private AndroidDriver driver;
        private DriverConfiguration driverConfiguration = getConfig();
        
        private static DriverConfiguration getConfig()
        {
            LoadJsonData loadJsonData = new LoadJsonData();
            return loadJsonData.GetObjectFromFile<DriverConfiguration>("Galaxy-Nexus-Android-v12.json");
        }


        [SetUp] 
        public void Init()
        {
            driverManager = new DriverManager(driverConfiguration);
            driver = driverManager.GetDriver();
        }

        [Test]
        // Google search by starting on the home screen
        public void FirstTest()
        {
            AppiumElement googleSearch = driver.FindElement(By.XPath("//android.widget.FrameLayout[@content-desc='Search']"));
            googleSearch.Click();
            Wait();
            AppiumElement googleSearchTextBox = driver.FindElement(By.XPath("/hierarchy/android.widget.FrameLayout/android.widget.LinearLayout/android.widget.FrameLayout/android.widget.LinearLayout/android.widget.FrameLayout/android.view.ViewGroup/android.widget.FrameLayout/android.view.ViewGroup/android.view.ViewGroup/android.widget.FrameLayout/android.widget.FrameLayout/android.view.ViewGroup/android.widget.FrameLayout/android.widget.LinearLayout/android.widget.EditText"));
            googleSearchTextBox.SendKeys("Visual Studio, Appium and C#");
            Wait();
            driver.PressKeyCode(AndroidKeyCode.Enter);
            Wait(5);
        }


        private void Wait(double seconds = 3)
        {
            Thread.Sleep(TimeSpan.FromSeconds(seconds));
        }

        [TearDown]
        public void TearDown()
        {
            if (driver != null)
            {
                driver.Quit();
            }
        }
    }
}
