using AventStack.ExtentReports;
using AventStack.ExtentReports.Gherkin.Model;
using Defra.TestAutomation.Specs.Drivers;
using Defra.TestAutomation.Specs.FrameworkUtilities;
using NUnit.Framework;
using System.Reflection;
using System.Runtime.CompilerServices;
using TechTalk.SpecFlow.Infrastructure;

namespace Defra.TestAutomation.Specs.Hooks
{
    [Binding]
    [Parallelizable]
    public class SpecFlowHooks : ExtentReporter
    {
        private readonly DriverFactory _driverFactory;
        private IWebDriver? _driver;
        private readonly ScenarioContext _scenarioContext;
        public static readonly object _lockObject = new object();
        public static string PassStatus = "PASS";
        public static string FailStatus = "FAIL";

        public SpecFlowHooks(DriverFactory driverFactory, ScenarioContext scenarioContext)
        {
            _driverFactory = driverFactory;
            _scenarioContext = scenarioContext;
        }

        [BeforeTestRun]
        public static void BeforeTestRun()
        {
            InitExtentReport();
        }

        [BeforeScenario]
        public void BeforeScenario(ScenarioContext scenarioContext)
        {
            _extentTestScenario = _extentTestFeature?.CreateNode<Scenario>(scenarioContext.ScenarioInfo.Title);
        }

        [BeforeScenario("Chrome")]
        public void BeforeChromeScenario()
        {
            
            _driver = _driverFactory.GetWebDriver("Chrome");
        }

        [BeforeScenario("Edge")]
        public void BeforeEdgeScenario()
        {
            _driver = _driverFactory.GetWebDriver("Edge");
        }

        [BeforeFeature]
        public static void BeforeFeature(FeatureContext featureContext)
        {
            _extentTestFeature = _extentReports?.CreateTest<Feature>(featureContext.FeatureInfo.Title);
        }

        [AfterStep]
        public void AfterStep(ScenarioContext scenarioContext)
        {
            string stepType = scenarioContext.StepContext.StepInfo.StepDefinitionType.ToString();
            string stepName = scenarioContext.StepContext.StepInfo.Text;
            //Condition Block for Pass
            if (scenarioContext.TestError == null)
            {
                if (stepType == "Given")
                {

                    _extentTestScenario?.CreateNode<Given>(stepName);

                }
                if (stepType == "When")
                {

                    _extentTestScenario?.CreateNode<When>(stepName);

                }
                if (stepType == "Then")
                {

                    _extentTestScenario?.CreateNode<Then>(stepName);

                }
                if (stepType == "And")
                {

                    _extentTestScenario?.CreateNode<And>(stepName);

                }
                if (stepType == "But")
                {
                    _extentTestScenario?.CreateNode<But>(stepName);

                }
            }

            //Condition Block for Fail
            if (scenarioContext.TestError != null)
            {
                string screenshotLocation = addScreenshot();
                if (stepType == "Given")
                {

                    _extentTestScenario?.CreateNode<Given>(stepName).Fail(scenarioContext.TestError.Message, MediaEntityBuilder.CreateScreenCaptureFromPath(screenshotLocation).Build());


                }
                if (stepType == "When")
                {

                    _extentTestScenario?.CreateNode<When>(stepName).Fail(scenarioContext.TestError.Message, MediaEntityBuilder.CreateScreenCaptureFromPath(screenshotLocation).Build());

                }
                if (stepType == "Then")
                {

                    _extentTestScenario?.CreateNode<Then>(stepName).Fail(scenarioContext.TestError.Message, MediaEntityBuilder.CreateScreenCaptureFromPath(screenshotLocation).Build());

                }
                if (stepType == "And")
                {

                    _extentTestScenario?.CreateNode<And>(stepName).Fail(scenarioContext.TestError.Message, MediaEntityBuilder.CreateScreenCaptureFromPath(screenshotLocation).Build());

                }
                if (stepType == "But")
                {

                    _extentTestScenario?.CreateNode<But>(stepName).Fail(scenarioContext.TestError.Message, MediaEntityBuilder.CreateScreenCaptureFromPath(screenshotLocation).Build());

                }
            }
        }

        [AfterTestRun]
        public static void AfterTestRun()
        {
            /*** Flush the extent report ***/
            FlushExtentReport();
            FlushConsolidateReport();
        }

        [AfterScenario]
        public void AfterScenario()
        {
            _driverFactory.CloseDriver();
        }

        /// <summary>
        /// Function to take screenshot and save as file & path
        /// </summary>
        /// <returns>Screenshot Location Path</returns>
        public string addScreenshot()
        {
            ITakesScreenshot takesScreenshot = (ITakesScreenshot)_driver!;
            Screenshot screenshot = takesScreenshot.GetScreenshot();
            string timeStamp = DateTime.Now.ToString("yyyyMMddHHmmssfff");
            string screenshotName = $"screenShotName_{timeStamp}";
            string screenshotDirectory = Path.Combine(testResultPath + "\\Screenshots");
            if (!Directory.Exists(screenshotDirectory))
            {
                Directory.CreateDirectory(screenshotDirectory);
            }
            string screenshotPath = Path.Combine(screenshotDirectory + $"\\{screenshotName}.png");
            screenshot.SaveAsFile(screenshotPath);
            return screenshotPath;
        }

        /// <summary>
        /// Function for custom reporting to append to extent report
        /// </summary>
        /// <param name="ReportDescription"></param>
        /// <param name="ReportStatus"></param>
        /// <param name="blnTakeScreenshot"></param>
        public void CallWriteReport(string ReportDescription, string ReportStatus, bool blnTakeScreenshot)
        {
            string stepType = _scenarioContext.StepContext.StepInfo.StepDefinitionType.ToString();
            string stepName = _scenarioContext.StepContext.StepInfo.Text;

            //Condition Block for Pass
            if (ReportStatus.ToLower().Equals("pass"))
            {
                string screenshotLocation = addScreenshot();
                if (stepType == "Given")
                {
                    if (blnTakeScreenshot)
                    {
                        _extentTestScenario?.CreateNode<Given>(stepName).Pass(ReportDescription, MediaEntityBuilder.CreateScreenCaptureFromPath(screenshotLocation).Build());
                    }
                    else
                    {
                        _extentTestScenario?.CreateNode<Given>(stepName).Pass(ReportDescription);
                    }
                }
                if (stepType == "When")
                {
                    if (blnTakeScreenshot)
                    {
                        _extentTestScenario?.CreateNode<When>(stepName).Pass(ReportDescription, MediaEntityBuilder.CreateScreenCaptureFromPath(screenshotLocation).Build());
                    }
                    else
                    {
                        _extentTestScenario?.CreateNode<When>(stepName).Pass(ReportDescription);
                    }
                }
                if (stepType == "Then")
                {
                    if (blnTakeScreenshot)
                    {
                        _extentTestScenario?.CreateNode<Then>(stepName).Pass(ReportDescription, MediaEntityBuilder.CreateScreenCaptureFromPath(screenshotLocation).Build());
                    }
                    else
                    {
                        _extentTestScenario?.CreateNode<Then>(stepName).Pass(ReportDescription);
                    }
                }
                if (stepType == "And")
                {
                    if (blnTakeScreenshot)
                    {
                        _extentTestScenario?.CreateNode<And>(stepName).Pass(ReportDescription, MediaEntityBuilder.CreateScreenCaptureFromPath(screenshotLocation).Build());
                    }
                    else
                    {
                        _extentTestScenario?.CreateNode<And>(stepName).Pass(ReportDescription);
                    }
                }
                if (stepType == "But")
                {
                    if (blnTakeScreenshot)
                    {
                        _extentTestScenario?.CreateNode<But>(stepName).Pass(ReportDescription, MediaEntityBuilder.CreateScreenCaptureFromPath(screenshotLocation).Build());
                    }
                    else
                    {
                        _extentTestScenario?.CreateNode<But>(stepName).Pass(ReportDescription);
                    }
                }
            }

            //Condition Block for Fail
            if (ReportStatus.ToLower().Equals("fail"))
            {
                string screenshotLocation = addScreenshot();
                if (stepType == "Given")
                {
                    _extentTestScenario?.CreateNode<Given>(stepName).Fail(ReportDescription, MediaEntityBuilder.CreateScreenCaptureFromPath(screenshotLocation).Build());
                }
                if (stepType == "When")
                {
                    _extentTestScenario?.CreateNode<When>(stepName).Fail(ReportDescription, MediaEntityBuilder.CreateScreenCaptureFromPath(screenshotLocation).Build());
                }
                if (stepType == "Then")
                {
                    _extentTestScenario?.CreateNode<Then>(stepName).Fail(ReportDescription, MediaEntityBuilder.CreateScreenCaptureFromPath(screenshotLocation).Build());
                }
                if (stepType == "And")
                {
                    _extentTestScenario?.CreateNode<And>(stepName).Fail(ReportDescription, MediaEntityBuilder.CreateScreenCaptureFromPath(screenshotLocation).Build());
                }
                if (stepType == "But")
                {
                    _extentTestScenario?.CreateNode<But>(stepName).Fail(ReportDescription, MediaEntityBuilder.CreateScreenCaptureFromPath(screenshotLocation).Build());
                }
            }
        }

        public ExtentTest GetExtentTest()
        {
            return _extentTestFeature!;
        }
    }
}
