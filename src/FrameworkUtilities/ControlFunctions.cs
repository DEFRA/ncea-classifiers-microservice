using Defra.TestAutomation.Specs.Drivers;
using NUnit.Framework;

namespace Defra.TestAutomation.Specs.FrameworkUtilities
{
    [Binding]
    [Parallelizable]
    public sealed class ControlFunctions
    {
        private readonly IWebDriver? _driver;
        private readonly WebDriverUtil _driverUtil;

        public ControlFunctions(DriverFactory driverFactory, WebDriverUtil driverUtil)
        {
            _driver = driverFactory.GetWebDriver();
            _driverUtil = driverUtil;
        }

        /*** Timeout Declaration ***/
        public static readonly double timeOutInSeconds = double.Parse(ConfigReader.ReadConfig("ObjectSyncTimeout"));
        public static readonly double pageTimeOutInSeconds = double.Parse(ConfigReader.ReadConfig("PageLoadTimeout"));

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
            if (_driverUtil.WaitUntilElementLocated(locator, timeOutInSeconds, elementName, pageName))
            {
                if (_driverUtil.WaitUntilElementVisible(locator, timeOutInSeconds, elementName, pageName))
                {
                    if (_driverUtil.WaitUntilElementToBeClickable(locator, timeOutInSeconds, elementName, pageName))
                    {
                        try
                        {
                            _driver?.FindElement(locator).SendKeys(Text);
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
            if (_driverUtil.WaitUntilElementLocated(locator, timeOutInSeconds, elementName, pageName))
            {
                if (_driverUtil.WaitUntilElementVisible(locator, timeOutInSeconds, elementName, pageName))
                {
                    if (_driverUtil.WaitUntilElementToBeClickable(locator, timeOutInSeconds, elementName, pageName))
                    {
                        try
                        {
                            _driver?.FindElement(locator).Click();                            
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


        public bool objectExists(By locator, string condition, double timeOutInSeconds, string elementName, string pageName)
        {
            bool StatusFlag = false;
            if (_driverUtil.WaitUntilElementLocated(locator, timeOutInSeconds, elementName, pageName))
            {
                if (_driverUtil.WaitUntilElementVisible(locator, timeOutInSeconds, elementName, pageName))
                {
                    switch (condition)
                    {
                        case "isDisplayed":
                            StatusFlag = _driver!.FindElement(locator).Displayed;
                            break;
                        case "isEnabled":
                            StatusFlag = _driver!.FindElement(locator).Enabled;
                            break;
                        case "isSelected":
                            StatusFlag = _driver!.FindElement(locator).Selected;
                            break;
                        default:
                            throw new Exception($"This operation '{condition}' are not allowed. Change the pre-defined options as per switch case");
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
            return StatusFlag;
        }
    }
}
