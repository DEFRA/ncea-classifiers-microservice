using Defra.TestAutomation.Specs.Drivers;
using Defra.TestAutomation.Specs.FrameworkUtilities;
using Defra.TestAutomation.Specs.PageObjects;
using OpenQA.Selenium;

namespace Defra.TestAutomation.Specs.StepDefinitions
{
    [Binding]
    public sealed class LoginSteps
    {
        private readonly IWebDriver _driver;
        private readonly ControlFunctions _ctrlFunc;

        public LoginSteps(DriverFactory driverFactory, ControlFunctions ctrlFunc, WebDriverUtil webDriverUtil)
        {
            _driver = driverFactory.GetWebDriver();
            _ctrlFunc = ctrlFunc;
        }

        [Given(@"User open the url ""([^""]*)""")]
        public void GivenUserOpenTheUrl(string url)
        {
            _driver.Navigate().GoToUrl(url);
            _ctrlFunc.click(LoginPage.AcceptTandC, 20, "Search Bar", "Google Home Page");

        }

        [When(@"the user enters search term ""([^""]*)"" and clicks search button")]
        public void WhenTheUserEntersSearchTermAndClicksSearchButton(string strSearchText)
        {
            _ctrlFunc.sendKeys(LoginPage.SearchTextBox, 20, strSearchText, "Search Text", "Google Home Page");
        }

        [Then(@"the search results page is displayed")]
        public void ThenTheSearchResultsPageIsDisplayed()
        {
            _ctrlFunc.click(LoginPage.SearchButton, 20, "Search Button", "Google Home Page");
        }

    }
}
