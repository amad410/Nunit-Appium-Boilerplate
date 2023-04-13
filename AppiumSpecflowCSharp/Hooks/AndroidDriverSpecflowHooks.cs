using AppiumSpecflowCSharp.Drivers;
using AppiumSpecflowCSharp.Models;
using AppiumSpecflowCSharp.Utilities;
using BoDi;
using OpenQA.Selenium.Appium.Android;
using TechTalk.SpecFlow;

namespace AppiumSpecflowCSharp.Hooks
{
    [Binding]
    public class AndroidDriverSpecflowHooks
    {
        private DriverManager driverManager;
        private DriverConfiguration driverConfiguration = getConfig();
        
        // need a better way to pass config
        private static DriverConfiguration getConfig()
        {
            LoadJsonData loadJsonData = new LoadJsonData();
            return loadJsonData.GetObjectFromFile<DriverConfiguration>("Galaxy-Nexus-Android-v12-Chrome.json");
        }


        protected AndroidDriver driver { get; private set; }
        public IObjectContainer _objectContainer;

        public AndroidDriverSpecflowHooks(IObjectContainer objectContainer)
        {
            _objectContainer = objectContainer;
        }


        [BeforeScenario]
        public void BeforeScenario()
        {
            driverManager = new DriverManager(driverConfiguration);
            driver = driverManager.GetDriver();
            _objectContainer.RegisterInstanceAs(driver, "driver");
        }

        [AfterScenario]
        public void AfterScenario()
        {
            if (driver != null)
            {
                driver.Quit();
            }
        }
    }
}
