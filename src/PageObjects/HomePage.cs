using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Defra.TestAutomation.Specs.PageObjects
{
    [Binding]
    [Parallelizable]
    public sealed class HomePage
    {
        private HomePage()
        {
            //Instantiate the object
        }

        public static readonly By SearchTextBox = By.Id("search");
        public static readonly By SearchButton = By.XPath(".//button[text()='Search' and @type='submit']");
        public static readonly By SearchErrorMsg = By.XPath(".//*[@id='search-error' and contains(@class, 'error-message')]");

        /****** Field Names - To be used for Reporting *********/
        public static readonly string TxtSearch = "Search Input";
        public static readonly string BtnSearch = "Submit";

        /****** Page Names - To be used for Reporting *********/
        public static readonly string pgeHome = "Home Page";
    }
}
