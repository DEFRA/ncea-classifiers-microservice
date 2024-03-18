using Defra.TestAutomation.Specs.Drivers;
using Defra.TestAutomation.Specs.FrameworkUtilities;
using Defra.TestAutomation.Specs.PageObjects;
using NUnit.Framework;

namespace Defra.TestAutomation.Specs.StepDefinitions
{
    [Binding]
    [Parallelizable]
    public sealed class HomePageSteps
    {
        /****** Section Start: Common to all Step Defn.class files *******/

        /// <summary>
        /// Please copy the entire section from Start to End during creation of new step definition files and rename the constructor name to actual class name
        /// </summary>

        private readonly IWebDriver? _driver;
        private readonly ControlFunctions _ctrlFunc;

        public HomePageSteps(DriverFactory driverFactory, ControlFunctions ctrlFunc, WebDriverUtil webDriverUtil)
        {
            _driver = driverFactory.GetWebDriver();
            _ctrlFunc = ctrlFunc;
        }

        //Timeout Declaration
        public static readonly double timeOutInSeconds = double.Parse(ConfigReader.ReadConfig("ObjectSyncTimeout"));
        public static readonly double pageTimeOutInSeconds = double.Parse(ConfigReader.ReadConfig("PageLoadTimeout"));

        /****** Section End *******/

        [Given(@"User navigate to homepage by launching ""([^""]*)""")]
        public void GivenUserNavigateToHomepageByLaunching(string URLType)
        {
            string url = ConfigReader.ReadConfig(URLType);
            _driver?.Navigate().GoToUrl(url);
            if (!(_ctrlFunc.objectExists(HomePage.SearchTextBox, "isDisplayed", timeOutInSeconds, HomePage.TxtSearch, HomePage.pgeHome)))
            {
                Assert.Fail($"User is not navigated to Home Page by launching the URL - '{url}'");
            }
        }

        [When(@"user enters search term less than four characters ""([^""]*)"" excluding separators")]
        public void WhenUserEntersSearchTermLessThanFourCharactersExcludingSeparators(string TextInput)
        {
            _ctrlFunc.sendKeys(HomePage.SearchTextBox, timeOutInSeconds, TextInput, HomePage.TxtSearch, HomePage.pgeHome);
        }

        [Then(@"user observes the dormant behavior of search button")]
        public void ThenUserObservesTheDormantBehaviorOfSearchButton()
        {
            //Method
        }


    }
}
