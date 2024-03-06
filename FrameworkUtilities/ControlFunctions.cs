using Defra.TestAutomation.Specs.Drivers;

namespace Defra.TestAutomation.Specs.FrameworkUtilities
{
    [Binding]
    public sealed class ControlFunctions
    {
        private readonly IWebDriver _driver;
        private readonly WebDriverUtil _driverUtil;

        public ControlFunctions(DriverFactory driverFactory, WebDriverUtil driverUtil)
        {
            _driver = driverFactory.GetWebDriver();
            _driverUtil = driverUtil;
        }

        /*** Timeout Declaration ***/
        public static readonly double timeOutInSeconds = double.Parse(ConfigurationManager.AppSettings["ObjectSyncTimeout"]);
        public static readonly double pageTimeOutInSeconds = double.Parse(ConfigurationManager.AppSettings["PageLoadTimeout"]);

        /// <summary>
        ///  Function to input the value using sendkeys   
        /// </summary>
        /// <param name="locator"></param>
        /// <param name="timeOutInSeconds"></param>
        /// <param name="Text"></param>
        /// <param name="elementName"></param>
        /// <exception cref="Exception"></exception>
        public void sendKeys(By locator, double timeOutInSeconds, string Text, string elementName, string pageName)
        {
            if (_driverUtil.waitUntilElementLocated(locator, timeOutInSeconds, elementName, pageName))
            {
                if (_driverUtil.waitUntilElementVisible(locator, timeOutInSeconds, elementName, pageName))
                {
                    if (_driverUtil.waitUntilElementToBeClickable(locator, timeOutInSeconds, elementName, pageName))
                    {
                        try
                        {
                            _driver.FindElement(locator).SendKeys(Text);
                        }
                        catch (Exception e)
                        {
                            throw new Exception($"Exception on enter text in the field {elementName} - {e.Message}");
                        }
                    }
                    else
                    {
                        //false block to avoid warnings
                    }
                }
                else
                {
                    //false block to avoid warnings
                }
            }
        }

        /// <summary>
        /// Function to perform click on element
        /// </summary>
        /// <param name="locator"></param>
        /// <param name="timeOutInSeconds"></param>
        /// <param name="elementName"></param>
        /// <exception cref="Exception"></exception>
        public void click(By locator, double timeOutInSeconds, string elementName, string pageName)
        {
            if (_driverUtil.waitUntilElementLocated(locator, timeOutInSeconds, elementName, pageName))
            {
                if (_driverUtil.waitUntilElementVisible(locator, timeOutInSeconds, elementName, pageName))
                {
                    if (_driverUtil.waitUntilElementToBeClickable(locator, timeOutInSeconds, elementName, pageName))
                    {
                        try
                        {
                            _driver.FindElement(locator).Click();
                        }
                        catch (Exception e)
                        {
                            throw new Exception($"Exception on perform click on {elementName} : {e.Message}");
                        }
                    }
                    else
                    {
                        //false block to avoid warnings
                    }
                }
                else
                {
                    //false block to avoid warnings
                }
            }
        }


        public ISearchContext GetShadowRootHost(IWebElement webElement)
        {
            IJavaScriptExecutor js = (IJavaScriptExecutor)_driver;
            ISearchContext shadowRootElement = (ISearchContext)js.ExecuteScript("return arguments[0].shadowRoot", webElement);
            return shadowRootElement;
        }

        

    }
}
