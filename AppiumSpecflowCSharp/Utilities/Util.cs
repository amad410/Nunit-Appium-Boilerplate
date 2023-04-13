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
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading;

namespace AppiumSpecflowCSharp.Utilities
{
    public class Util
    {
        public static string AppLocation()
        {
            var location = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
            if (location is null)
            {
                throw new ArgumentException("Location - Base Directory - is NULL");
            }
            else
            {
                return location;
            }
        }

        // ported from JAVA - can we still use TouchAction?
        // moved to BaseView
        public static void SwipeScreen(Direction dir, AndroidDriver androidDriver)
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
            Size dims = androidDriver.Manage().Window.Size;

            // init start point = center of screen
            pointStart = new Point(dims.Width / 2, dims.Height / 2);

            switch (dir)
            {
                case Direction.DOWN: // center of footer
                    pointEnd = new Point(dims.Width / 2, dims.Height - edgeBorder);
                    break;
                case Direction.UP: // center of header
                    pointEnd = new Point(dims.Width / 2, edgeBorder);
                    break;
                case Direction.LEFT: // center of left side
                    pointEnd = new Point(edgeBorder, dims.Height / 2);
                    break;
                case Direction.RIGHT: // center of right side
                    pointEnd = new Point(dims.Width - edgeBorder, dims.Height / 2);
                    break;
                default:
                    throw new ArgumentException($"SwipeScreen(): dir: '{dir}' NOT supported");
            }

            // execute swipe using TouchAction
            try
            {
                new TouchAction(androidDriver)
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
            catch
            {
                // ignore
            }
        }

        public static void DragAndDrop(AppiumElement source, AppiumElement target, AndroidDriver androidDriver)
        {
            try
            {
                new TouchAction(androidDriver)
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

        public static void ScrollDown(AndroidDriver androidDriver)
        {
            Size dims = androidDriver.Manage().Window.Size;
            // Check coordinates on device
            int scrollStart = 10;
            int scrollEnd = (int)(dims.Height * 0.8);

            try
            {
                new TouchAction(androidDriver)
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

        public void ScrollToElement(AndroidDriver androidDriver, string selector, bool clickElement = false)
        {
            bool done = false;
            var count = 30; // max paging attempts
            while (!done && count > 0)
            {
                IList<AppiumElement> els = androidDriver.FindElements(By.Id(selector)).ToList();
                if (els.Count == 0)
                {
                    count--;
                    ScrollDown(androidDriver);
                }
                else
                {
                    done = true;
                    if (clickElement)
                    {
                        Thread.Sleep(500);
                        els.First().Click();
                    }
                }
            }
        }

        public void ZoomIn(AndroidDriver androidDriver, AppiumElement map)
        {
            int x = map.Location.X + map.Size.Width / 2;
            int y = map.Location.Y + map.Size.Height / 2;

            TouchAction finger1 = new TouchAction(androidDriver);
            finger1.Press(map, x, y - 10).MoveTo(map, x, y - 100);
            TouchAction finger2 = new TouchAction(androidDriver);
            finger2.Press(map, x, y + 10).MoveTo(map, x, y + 100);
            IMultiAction action = new MultiAction(androidDriver);
            action.Add(finger1).Add(finger2).Perform();
            Thread.Sleep(8000);
        }
        public void ZoomOut(AndroidDriver androidDriver, AppiumElement map)
        {
            int x1 = map.Size.Width;
            int y1 = map.Size.Height;
            int x = map.Location.X + map.Size.Width / 2;
            int y = map.Location.Y + map.Size.Height / 2;

            TouchAction finger1 = new TouchAction(androidDriver);
            finger1.Press(map, x, y - 100).MoveTo(map, x, y - 10);

            TouchAction finger2 = new TouchAction(androidDriver);
            finger2.Press(map, x, y + 100).MoveTo(map, x, y + 10);

            IMultiAction action = new MultiAction(androidDriver);
            action.Add(finger1).Add(finger2).Perform();
            Thread.Sleep(5000);
        }

        public enum Direction
        {
            UP,
            DOWN,
            LEFT,
            RIGHT
        }

        public static void WaitForVisibility(By locator, AndroidDriver driver, long duration)
        {
            DefaultWait<AndroidDriver> wait = new DefaultWait<AndroidDriver>(driver);
            wait.Timeout = TimeSpan.FromSeconds(duration);
            wait.PollingInterval = TimeSpan.FromMilliseconds(500);
            wait.IgnoreExceptionTypes(typeof(NoSuchElementException));
            wait.Until(ExpectedConditions.ElementIsVisible(locator));
        }
    }
}
