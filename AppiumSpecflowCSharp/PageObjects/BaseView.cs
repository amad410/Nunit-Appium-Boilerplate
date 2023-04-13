using OpenQA.Selenium;
using OpenQA.Selenium.Appium;
using OpenQA.Selenium.Appium.Android;
using OpenQA.Selenium.Appium.Interfaces;
using OpenQA.Selenium.Appium.MultiTouch;
using OpenQA.Selenium.Support.UI;
using SeleniumExtras.WaitHelpers;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Threading;


namespace AppiumSpecflowCSharp.PageObjects
{
    // Please note, this is WIP
    // todo: use ActionSequence - see https://appium.io/docs/en/commands/interactions/actions
    public class BaseView
    {
        private AndroidDriver driver;
        // or WebDriverWait ? currently got both
        private DefaultWait<AndroidDriver> waitUntil;
        private int timeOut = 10;

        public BaseView(AndroidDriver androidDriver)
        {
            driver = androidDriver;
            waitUntil = new DefaultWait<AndroidDriver>(driver)
            {
                Timeout = TimeSpan.FromSeconds(timeOut),
                PollingInterval = TimeSpan.FromMilliseconds(500)
            };
        }

        #region Locators and Identifiers
        #endregion


        public void Clear(By e, long timeOut = 10)
        {
            new WebDriverWait(driver, TimeSpan.FromSeconds(timeOut))
                .Until(ExpectedConditions.VisibilityOfAllElementsLocatedBy(e));
            driver.FindElement(e).Clear();
        }
        public void Click(By e, long timeOut = 10)
        {
            new WebDriverWait(driver, TimeSpan.FromSeconds(timeOut))
                .Until(ExpectedConditions.VisibilityOfAllElementsLocatedBy(e));
            driver.FindElement(e).Click();
        }
        public void CloseApplication()
        {
            ((IInteractsWithApps)driver).CloseApp();
        }
        public void DragAndDrop(AppiumElement source, AppiumElement target)
        {
            try
            {
                new TouchAction(driver)
                    .LongPress(source)
                    .Wait()
                    .MoveTo(target)
                    .Release()
                    .Perform();
            }
            catch (Exception e)
            {
                Console.WriteLine($"DragAndDrop(): TouchAction FAILED\n {e.Message}");
                return;
            }
        }
        public void DragDrop(AppiumElement drag, AppiumElement drop)
        {
            new TouchAction(this.driver)
                .LongPress(drag)
                .Wait()
                .MoveTo(drop)
                .Release()
                .Perform();
        }
        public void FrameToBeAvailableAndSwitchToIt(By locator)
        {
            waitUntil.Until(ExpectedConditions.FrameToBeAvailableAndSwitchToIt(locator));
        }
        public void LaunchApplication()
        {
            ((IInteractsWithApps)driver).LaunchApp();
        }
        public void ScrollDown()
        {
            Size dims = driver.Manage().Window.Size;
            // Check coordinates on device
            int scrollStart = 10;
            int scrollEnd = (int)(dims.Height * 0.8);

            try
            {
                new TouchAction(driver)
                    .Press(0, scrollStart)
                    .Wait(3000)
                    .MoveTo(0, scrollEnd)
                    .Release()
                    .Perform();
            }
            catch (Exception e)
            {
                Console.WriteLine($"ScrollDown(): TouchAction FAILED\n {e.Message}");
                return;
            }
        }
        private void scrollDown(double startPosition, double endPosition, long waitDuration)
        {
            Size dimension = this.driver.Manage().Window.Size;
            int scrollStart = (int)(dimension.Height * startPosition);
            int scrollEnd = (int)(dimension.Height * endPosition);

            new TouchAction(this.driver)
                .Press(0, scrollStart)
                .Wait(waitDuration)
                .MoveTo(0, scrollEnd)
                .Release()
                .Perform();
        }
        public AppiumElement ScrollToElement(string uiSelector)
        {
            return ((IFindByAndroidUIAutomator<AppiumElement>)driver).FindElementByAndroidUIAutomator(
                    "new UiScrollable(new UiSelector()" + ".scrollable(true)).scrollIntoView("
                            + $"new UiSelector().description(\"{uiSelector}\"));");
        }
        public void SendKeys(By e, string text, long timeOut = 10)
        {
            WebDriverWait wait = new WebDriverWait(driver, TimeSpan.FromSeconds(timeOut));
            wait.Until(ExpectedConditions.VisibilityOfAllElementsLocatedBy(e));
            driver.FindElement(e).SendKeys(text);
        }
        public void Swipe(AppiumElement startingElem, int xOffset, int yOffset)
        {

            new TouchAction(this.driver)
                .Press(startingElem)
                .Wait()
                .MoveTo(xOffset, yOffset)
                .Release()
                .Perform();
        }
        public void Swipe(int startX, int startY, int endX, int endY, int millis)
        {

            TouchAction t = new TouchAction(driver);
            t.Press(startX, startY)
                .Wait(millis)
                .MoveTo(endX, endY)
                .Release()
                .Perform();
        }
        public void SwipeScreen(SwipeDirection dir)
        {
            // Animation default time:
            //  - Android: 300 ms
            //  - iOS: 200 ms
            // final value depends on your app and could be greater
            int ANIMATION_TIME = 300; // ms
            int PRESS_TIME = 200; // ms

            int edgeBorder = 10; // better avoid edges
            Point pointStart;
            Point pointEnd;

            // init screen variables
            Size dims = driver.Manage().Window.Size;

            // init start point = center of screen
            pointStart = new Point(dims.Width / 2, dims.Height / 2);

            switch (dir)
            {
                case SwipeDirection.DOWN: // center of footer
                    pointEnd = new Point(dims.Width / 2, dims.Height - edgeBorder);
                    break;
                case SwipeDirection.UP: // center of header
                    pointEnd = new Point(dims.Width / 2, edgeBorder);
                    break;
                case SwipeDirection.LEFT: // center of left side
                    pointEnd = new Point(edgeBorder, dims.Height / 2);
                    break;
                case SwipeDirection.RIGHT: // center of right side
                    pointEnd = new Point(dims.Width - edgeBorder, dims.Height / 2);
                    break;
                default:
                    throw new ArgumentException($"SwipeScreen(): dir: '{dir}' NOT supported");
            }

            // execute swipe using TouchAction
            try
            {
                new TouchAction(driver)
                        .Press(pointStart.X, pointStart.Y)
                        .Wait(PRESS_TIME)
                        .MoveTo(pointEnd.X, pointEnd.Y)
                        .Release().Perform();
            }
            catch (Exception e)
            {
                Console.WriteLine($"swipeScreen(): TouchAction FAILED\n {e.Message}");
                return;
            }

            // always allow swipe action to complete
            try
            {
                Thread.Sleep(ANIMATION_TIME);
            }
            catch (Exception e)
            {
                // ignore
            }
        }
        public void SwitchToWebView()
        {
            IList<string> contexts = driver.Contexts;
            foreach (string context in contexts)
            {
                if (context.Contains("WEBVIEW"))
                {
                    driver.Context = context;
                    break;
                }
            }
        }
        public void Tap(AppiumElement element)
        {
            new TouchAction(this.driver)
                .Tap(element).Perform();
        }
        public void Wait(int seconds)
        {
            Thread.Sleep(TimeSpan.FromSeconds(seconds));
        }


        // todo: not complete - fill it up as you go
        public void WaitFor(ElementState elementState, By locator)
        {
            waitUntil.IgnoreExceptionTypes(typeof(NoSuchElementException));
            switch (elementState)
            {
                case ElementState.ElementExists:
                    {
                        waitUntil.Until(ExpectedConditions.ElementToBeClickable(locator));
                        break;
                    }
                case ElementState.ElementToBeClickable:
                    {
                        waitUntil.Until(ExpectedConditions.ElementToBeClickable(locator));
                        break;
                    }
                case ElementState.ElementToBeSelected:
                    {
                        waitUntil.Until(ExpectedConditions.ElementToBeSelected(locator));
                        break;
                    }
            }
        }
        public void WaitFor(ElementState elementState, AppiumElement element)
        {
            waitUntil.IgnoreExceptionTypes(typeof(NoSuchElementException));
            switch (elementState)
            {
                case ElementState.ElementToBeClickable:
                    {
                        waitUntil.Until(ExpectedConditions.ElementToBeClickable(element));
                        break;
                    }
                case ElementState.ElementToBeSelected:
                    {
                        waitUntil.Until(ExpectedConditions.ElementToBeSelected(element));
                        break;
                    }
                case ElementState.StalenessOf:
                    {
                        waitUntil.Until(ExpectedConditions.StalenessOf(element));
                        break;
                    }
            }
        }
        public void WaitFor(ElementState elementState, By locator, string text)
        {
            waitUntil.IgnoreExceptionTypes(typeof(NoSuchElementException));
            switch (elementState)
            {
                case ElementState.TextToBePresentInElementValue:
                    {
                        waitUntil.Until(ExpectedConditions.TextToBePresentInElementValue(locator, text));
                        break;
                    }
            }
        }
        public void WaitFor(ElementState elementState, AppiumElement element, string text)
        {
            waitUntil.IgnoreExceptionTypes(typeof(NoSuchElementException));
            switch (elementState)
            {
                case ElementState.TextToBePresentInElement:
                    {
                        waitUntil.Until(ExpectedConditions.TextToBePresentInElement(element, text));
                        break;
                    }
                case ElementState.TextToBePresentInElementValue:
                    {
                        waitUntil.Until(ExpectedConditions.TextToBePresentInElementValue(element, text));
                        break;
                    }
            }
        }
        
        
        public void WaitForTitleContains(string title)
        {
            waitUntil.Until(ExpectedConditions.TitleContains(title));
        }
        public void WaitForUrlContains(string url)
        {
            waitUntil.Until(ExpectedConditions.UrlContains(url));
        }
        public void WaitForAlertIsPresent()
        {
            waitUntil.Until(ExpectedConditions.AlertIsPresent());
        }

        // todo: use ActionSequence - see https://appium.io/docs/en/commands/interactions/actions/
        public void ZoomIn(AppiumElement appiumElement)
        {
            
            int x = appiumElement.Location.X + appiumElement.Size.Width / 2;
            int y = appiumElement.Location.Y + appiumElement.Size.Height / 2;

            TouchAction finger1 = new TouchAction(driver);
            finger1.Press(appiumElement, x, y - 10).MoveTo(appiumElement, x, y - 100);
            TouchAction finger2 = new TouchAction(driver);
            finger2.Press(appiumElement, x, y + 10).MoveTo(appiumElement, x, y + 100);
            IMultiAction action = new MultiAction(driver);
            action.Add(finger1).Add(finger2).Perform();
            Wait(8);
        }
        public void ZoomOut(AppiumElement appiumElement)
        {
            int x = appiumElement.Location.X + appiumElement.Size.Width / 2;
            int y = appiumElement.Location.Y + appiumElement.Size.Height / 2;

            TouchAction finger1 = new TouchAction(driver);
            finger1.Press(appiumElement, x, y - 100).MoveTo(appiumElement, x, y - 10);

            TouchAction finger2 = new TouchAction(driver);
            finger2.Press(appiumElement, x, y + 100).MoveTo(appiumElement, x, y + 10);

            IMultiAction action = new MultiAction(driver);
            action.Add(finger1).Add(finger2).Perform();
            Wait(5);
        }


        #region Enums
        public enum SwipeDirection
        {
            UP,
            DOWN,
            LEFT,
            RIGHT
        }
        public enum ElementState
        {
            ElementExists,
            ElementIsVisible,
            ElementToBeClickable,
            ElementToBeSelected,
            FrameToBeAvailableAndSwitchToIt,
            InvisibilityOfElementLocated,
            PresenceOfAllElementsLocatedBy,
            StalenessOf,
            TextToBePresentInElement,
            TextToBePresentInElementValue,
            VisibilityOfAllElementsLocatedBy
        }
        #endregion
    }
}
