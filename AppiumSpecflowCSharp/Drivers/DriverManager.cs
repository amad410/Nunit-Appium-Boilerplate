using AppiumSpecflowCSharp.Models;
using OpenQA.Selenium;
using OpenQA.Selenium.Appium;
using OpenQA.Selenium.Appium.Android;
using OpenQA.Selenium.Appium.Enums;
using OpenQA.Selenium.Appium.Service;
using OpenQA.Selenium.Appium.Service.Options;
using System;
using System.Threading;


namespace AppiumSpecflowCSharp.Drivers
{
    public class DriverManager
    {
        // todo: MutiThread 
        private static ThreadLocal<AndroidDriver> driverPool = new ThreadLocal<AndroidDriver>();
        private DriverConfiguration _configuration;


        public DriverManager(DriverConfiguration configuration)
        {
            _configuration = configuration;
        }

        public AndroidDriver GetDriver()
        {
            if (_configuration is null)
            {
                throw new ArgumentNullException("DriverConfiguration is null");
            }
            
            // "Required"
            AppiumOptions appiumOptions = new AppiumOptions()
            {
                PlatformName = _configuration.PlatformName,
                PlatformVersion = _configuration.PlatformVersion,
                AutomationName = _configuration.AutomationName,
                DeviceName = _configuration.DeviceName
            };

            if (!string.IsNullOrEmpty(_configuration.AppPath))
            {
                appiumOptions.App = _configuration.AppPath;
            }
            if (!string.IsNullOrEmpty(_configuration.BrowserName))
            {
                appiumOptions.BrowserName = _configuration.BrowserName;
            }
            if (!string.IsNullOrEmpty(_configuration.BrowserVersion))
            {
                appiumOptions.BrowserVersion = _configuration.BrowserVersion;
            }
            if (!string.IsNullOrEmpty(_configuration.AppPackage))
            {
                appiumOptions.AddAdditionalAppiumOption(AndroidMobileCapabilityType.AppPackage, _configuration.AppPackage);
            }
            if (!string.IsNullOrEmpty(_configuration.AppActivity))
            {
                appiumOptions.AddAdditionalAppiumOption(AndroidMobileCapabilityType.AppActivity, _configuration.AppActivity);
            }

            // Control current version - wip
            // WebDriverManager.DriverManager driverManager = new WebDriverManager.DriverManager(@"C:\ChromeDriver2\");
            // driverManager.SetUpDriver(new ChromeConfig(), "Latest");


            //OptionCollector args = new OptionCollector();
            //if (!string.IsNullOrEmpty(_configuration.Url))
            //{
            //    args.AddArguments(GeneralOptionList.CallbackAddress(_configuration.Url));
            //}

            //// for local server instance
            // var appiumServiceBuilder = new AppiumServiceBuilder()
            //    .WithIPAddress(_configuration.Ip)
            //    //.UsingPort(_configuration.Port)
            //    .UsingAnyFreePort()
            //    .WithArguments(args)
            //    .Build();
            // appiumServiceBuilder.Start();

            // initialize Android driver on local server instance
            //return new AndroidDriver(appiumServiceBuilder, appiumOptions);

            // initialize Android remote driver
            return new AndroidDriver(new Uri(_configuration.Url), appiumOptions);
        }
    }
}
    