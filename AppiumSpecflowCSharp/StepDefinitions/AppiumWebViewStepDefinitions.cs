using AppiumSpecflowCSharp.Hooks;
using AppiumSpecflowCSharp.PageObjects;
using BoDi;
using NUnit.Framework;
using OpenQA.Selenium.Appium.Android;
using System;
using TechTalk.SpecFlow;

namespace AppiumSpecflowCSharp.StepDefinitions
{
    [Binding]
    public class AppiumWebViewStepDefinitions
    {
        private GoogleView googleView;
        private AndroidDriver driver;

        public AppiumWebViewStepDefinitions(IObjectContainer objectContainer)
        {
            driver = objectContainer.Resolve<AndroidDriver>("driver");
            googleView = new GoogleView(driver);
        }

        [Given(@"we navigate to home")]
        public void GivenWeNavigateToHome()
        {
            googleView.NavigateToGoogleHome();
        }

        [Given(@"and enter ""([^""]*)""")]
        public void GivenAndEnter(string searchTerm)
        {
            googleView.EnterSearchBox(searchTerm);
        }

        [When(@"hitting search")]
        public void WhenHittingSearch()
        {
            googleView.ClickSearch();
        }

        [Then(@"the result should be displayed")]
        public void ThenTheResultShouldBeDisplayed()
        {
            Assert.True(googleView.HasSearchResult());
        }
    }
}
