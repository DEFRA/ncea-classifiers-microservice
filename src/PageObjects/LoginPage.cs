namespace Defra.TestAutomation.Specs.PageObjects
{
    [Binding]
    public sealed class LoginPage
    {
        private LoginPage()
        {
            //Instantiate the object
        }

        public static readonly By AcceptTandC = By.Id("L2AGLb");
        public static readonly By SearchTextBox = By.Name("q");
        public static readonly By SearchButton = By.Name("btnK");


        /****** Field Names - To be used for Reporting *********/
        public static readonly string UserName = "username";
    }
}
