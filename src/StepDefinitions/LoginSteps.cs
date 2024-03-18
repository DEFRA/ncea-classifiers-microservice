﻿using Defra.TestAutomation.Specs.Drivers;
using Defra.TestAutomation.Specs.FrameworkUtilities;
using Defra.TestAutomation.Specs.PageObjects;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Defra.TestAutomation.Specs.StepDefinitions
{
    [Binding]
    [Parallelizable]
    public sealed class LoginSteps
    {
        /****** Section Start: Common to all Step Defn.class files *******/

        /// <summary>
        /// Please copy the entire section from Start to End during creation of new step definition files and rename the constructor name to actual class name
        /// </summary>
        
        private readonly IWebDriver? _driver;
        private readonly ControlFunctions _ctrlFunc;
        
        public LoginSteps(DriverFactory driverFactory, ControlFunctions ctrlFunc, WebDriverUtil webDriverUtil)
        {
            _driver = driverFactory.GetWebDriver();
            _ctrlFunc = ctrlFunc;
        }

        //Timeout Declaration
        public static readonly double timeOutInSeconds = double.Parse(ConfigReader.ReadConfig("ObjectSyncTimeout"));
        public static readonly double pageTimeOutInSeconds = double.Parse(ConfigReader.ReadConfig("PageLoadTimeout"));

        /****** Section End *******/

        [Given(@"User open the url ""([^""]*)""")]
        public void GivenUserOpenTheUrl(string URLType)
        {
            string url = ConfigReader.ReadConfig(URLType);
            _driver?.Navigate().GoToUrl(url);
            _ctrlFunc.click(LoginPage.AcceptTandC, timeOutInSeconds, "Search Bar", "Google Home Page");
        }

        [When(@"the user enters search term ""([^""]*)"" and clicks search button")]
        public void WhenTheUserEntersSearchTermAndClicksSearchButton(string strSearchText)
        {
            _ctrlFunc.sendKeys(LoginPage.SearchTextBox, timeOutInSeconds, strSearchText, "Search Text", "Google Home Page");
        }

        [Then(@"the search results page is displayed")]
        public void ThenTheSearchResultsPageIsDisplayed()
        {
            _ctrlFunc.click(LoginPage.SearchButton, timeOutInSeconds, "Search Button", "Google Home Page");
        }

    }
}
