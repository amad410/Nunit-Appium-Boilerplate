using System;
using System.Collections.Generic;
using System.Text;

namespace AppiumSpecflowCSharp.Models
{
    public class DriverConfiguration
    {
        public string Ip { get; set; }
        public int Port { get; set; }
        public string PlatformName { get; set; } = "Android";
        public string PlatformVersion { get; set; }
        public string DeviceName { get; set; } = "emulator-5554";
        public string AutomationName { get; set; } = "UIAutomator2";
        public string? AppPackage { get; set; }
        public string? AppActivity { get; set; }
        public string? AppPath { get; set; }
        public string? Url { get; set; }
        public string? BrowserName { get; set; }
        public string? BrowserVersion { get; set; }
        
    }
}
