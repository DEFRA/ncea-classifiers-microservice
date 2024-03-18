using NUnit.Framework;
using OpenQA.Selenium.DevTools;

namespace Defra.TestAutomation.Specs.Drivers
{
    [Binding]
    [Parallelizable]
    public class DriverFactory
    {
        public IWebDriver? _driver;

        public IWebDriver? GetWebDriver()
        {
            return _driver;
        }

        /// <summary>
        /// Function to get driver instance
        /// </summary>
        /// <returns></returns>
        public IWebDriver GetWebDriver(string browserType)
        {
            if (_driver == null)
            {
                _driver = CreateWebDriverInstance(browserType);
                _driver.Manage().Window.Maximize();
            }
            return _driver;
        }

        /// <summary>
        /// Function to create webdriver instance using the browser type
        /// </summary>
        /// <param name="browserType"></param>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        public IWebDriver CreateWebDriverInstance(string browserType)
        {
            switch (browserType)
            {
                case "Chrome":
                    new WebDriverManager.DriverManager().SetUpDriver(new ChromeConfig(), VersionResolveStrategy.MatchingBrowser);
                    ChromeOptions options = new();
                    options.AddArgument("no-sandbox");

                    return new ChromeDriver();
                case "Edge":
                    new WebDriverManager.DriverManager().SetUpDriver(new EdgeConfig());
                    return new EdgeDriver();
                default:
                    throw new Exception($"Undefined Browser Type - '{browserType}' is provided to switch statement");
            }
        }

        /// <summary>
        /// Funtion to the close the driver instances
        /// </summary>
        public void CloseDriver()
        {
            _driver?.Close();
            _driver?.Quit();
        }
    }
}
