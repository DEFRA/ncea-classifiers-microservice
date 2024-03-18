using Defra.TestAutomation.Specs.Drivers;
using NUnit.Framework;

namespace Defra.TestAutomation.Specs.FrameworkUtilities
{
    [Binding]
    [Parallelizable]
    public sealed class WebDriverUtil
    {
        private readonly IWebDriver? _driver;

        public WebDriverUtil(DriverFactory driverFactory)
        {
            _driver = driverFactory.GetWebDriver();
        }

        /*** Timeouts Declaration ***/
        public static readonly double pageloadTimeout = double.Parse(ConfigReader.ReadConfig("PageLoadTimeout"));
        public static readonly double invisibilityTimeout = double.Parse(ConfigReader.ReadConfig("InvisibilityTimeout"));
        public static readonly double StaleTimeout = double.Parse(ConfigReader.ReadConfig("StaleTimeout"));

        /// <summary>
        /// Function to pause the execution for the specified time period
        /// </summary>
        /// <param name="milliSeconds"></param>
        /// <param name="elementName"></param>
        /// <param name="elementType"></param>
        /// <exception cref="Exception"></exception>
        public void WaitFor(int milliSeconds, string elementName, string pageName)
        {
            try
            {
                Thread.Sleep(milliSeconds);
            }
            catch (Exception ex)
            {
                throw new Exception($"Unable to wait for element '{elementName}' with error - {ex.Message}");
            }
        }

        /// <summary>
        /// Function to wait until specified element is located
        /// </summary>
        /// <param name="locator"></param>
        /// <param name="timeOutInSeconds"></param>
        /// <param name="elementName"></param>
        /// <param name="pageName"></param>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        public bool WaitUntilElementLocated(By locator, double timeOutInSeconds, string elementName, string pageName)
        {
            try
            {
                new WebDriverWait(_driver, TimeSpan.FromSeconds(timeOutInSeconds)).Until(ExpectedConditions.ElementExists(locator));
                return true;
            }
            catch (Exception ex)
            {
                throw new Exception($"'{elementName}' is not found in the page {pageName} even after waiting for {timeOutInSeconds} Seconds with error : '{ex.Message}'");
            }
        }

        /// <summary>
        /// Function to wait until specified element is clickable & enabled
        /// </summary>
        /// <param name="locator"></param>
        /// <param name="timeOutInSeconds"></param>
        /// <param name="elementName"></param>
        /// <param name="pageName"></param>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        public bool WaitUntilElementToBeClickable(By locator, double timeOutInSeconds, string elementName, string pageName)
        {
            try
            {
                new WebDriverWait(_driver, TimeSpan.FromSeconds(timeOutInSeconds)).Until(ExpectedConditions.ElementToBeClickable(locator));
                return true;
            }
            catch (Exception ex)
            {
                throw new Exception($"'{elementName}' is not enabled in the page {pageName} even after waiting for {timeOutInSeconds} Seconds with error : '{ex.Message}'");
            }
        }

        /// <summary>
        /// Function to wait until specified element is visible
        /// </summary>
        /// <param name="locator"></param>
        /// <param name="timeOutInSeconds"></param>
        /// <param name="elementName"></param>
        /// <param name="pageName"></param>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        public bool WaitUntilElementVisible(By locator, double timeOutInSeconds, string elementName, string pageName)
        {
            try
            {
                new WebDriverWait(_driver, TimeSpan.FromSeconds(timeOutInSeconds)).Until(ExpectedConditions.ElementIsVisible(locator));
                return true;
            }
            catch (Exception ex)
            {
                throw new Exception($"'{elementName}' is not visible in the page {pageName} even after waiting for {timeOutInSeconds} Seconds with error : '{ex.Message}'");
            }
        }

        /// <summary>
        /// Function to wait until specified element is invisible
        /// </summary>
        /// <param name="locator"></param>
        /// <param name="timeOutInSeconds"></param>
        /// <param name="elementName"></param>
        /// <param name="pageName"></param>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        public bool WaitUntilElementInVisible(By locator, double timeOutInSeconds, string elementName, string pageName)
        {
            try
            {
                new WebDriverWait(_driver, TimeSpan.FromSeconds(timeOutInSeconds)).Until(ExpectedConditions.InvisibilityOfElementLocated(locator));
                return true;
            }
            catch (Exception ex)
            {
                throw new Exception($"'{elementName}' is visible in the page {pageName} even after waiting for {timeOutInSeconds} Seconds with error : '{ex.Message}'");
            }
        }

        /// <summary>
        /// Function to wait until specified element is invisible
        /// </summary>
        /// <param name="locator"></param>
        /// <param name="elementName"></param>
        /// <param name="pageName"></param>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        public bool WaitUntilElementInVisible(By locator, string elementName, string pageName)
        {
            try
            {
                new WebDriverWait(_driver, TimeSpan.FromSeconds(invisibilityTimeout)).Until(ExpectedConditions.InvisibilityOfElementLocated(locator));
                return true;
            }
            catch (Exception ex)
            {
                throw new Exception($"'{elementName}' is visible in the page {pageName} even after waiting for {invisibilityTimeout} Seconds with error : '{ex.Message}'");
            }
        }

        /// <summary>
        /// Function to wait until staleness of locator
        /// </summary>
        /// <param name="locator"></param>
        /// <param name="elementName"></param>
        /// <param name="pageName"></param>
        public void WaitUntilStalenessOfElement(By locator, string elementName, string pageName)
        {
            try
            {
                IWebElement webElement = _driver!.FindElement(locator);
                new WebDriverWait(_driver, TimeSpan.FromSeconds(StaleTimeout)).Until(ExpectedConditions.StalenessOf(webElement));
            }
            catch (Exception)
            {
                //no code
            }
        }

        /// <summary>
        /// Function to wait until staleness of element
        /// </summary>
        /// <param name="element"></param>
        /// <param name="elementName"></param>
        /// <param name="pageName"></param>
        public void WaitUntilStalenessOfElement(IWebElement element, string elementName, string pageName)
        {
            try
            {
                new WebDriverWait(_driver, TimeSpan.FromSeconds(StaleTimeout)).Until(ExpectedConditions.StalenessOf(element));
            }
            catch (Exception)
            {
                //no code
            }
        }

        /// <summary>
        /// Function to wait until staleness of locator
        /// </summary>
        /// <param name="locator"></param>
        /// <param name="StaleTimeoutInSeconds"></param>
        /// <param name="elementName"></param>
        /// <param name="pageName"></param>
        public void WaitUntilStalenessOfElement(By locator, double StaleTimeoutInSeconds, string elementName, string pageName)
        {
            try
            {
                IWebElement webElement = _driver!.FindElement(locator);
                new WebDriverWait(_driver, TimeSpan.FromSeconds(StaleTimeoutInSeconds)).Until(ExpectedConditions.StalenessOf(webElement));
            }
            catch (Exception)
            {
                //no code
            }
        }

        /// <summary>
        /// Function to wait until the page loads completely with readystate as 'complete'
        /// </summary>
        /// <param name="timeOutInSeconds"></param>
        /// <param name="pageName"></param>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        public bool WaitUntilPageReadyStateComplete(double timeOutInSeconds, string pageName)
        {
            try
            {
                IJavaScriptExecutor _jsExec = (IJavaScriptExecutor)_driver!;
                new WebDriverWait(_driver, TimeSpan.FromSeconds(timeOutInSeconds)).Until(_driver => _jsExec.ExecuteScript("return document.readyState").ToString() == "complete");
                bool jQueryDefined = (bool)_jsExec.ExecuteScript("return typeof jQuery != 'undefined' && jQuery !== 'null'");
                if (jQueryDefined)
                {
                    new WebDriverWait(_driver, TimeSpan.FromSeconds(timeOutInSeconds)).Until(_driver => _jsExec.ExecuteScript("return jQuery.active").ToString() == "0");
                }
                return true;

            }
            catch (Exception ex)
            {
                throw new Exception($"State of the page : '{pageName}' is not reached as 'Complete' even after waiting for {timeOutInSeconds} Seconds with error : '{ex.Message}'");
            }
        }
    }
}
