using AventStack.ExtentReports;
using AventStack.ExtentReports.Reporter;
using AventStack.ExtentReports.Reporter.Config;
using NUnit.Framework;
using System.Reflection;
using System.Runtime.CompilerServices;

namespace Defra.TestAutomation.Specs.FrameworkUtilities
{
    [Binding]
    [Parallelizable]
    public class ExtentReporter
    {
        [ThreadStatic]
        public static ExtentReports? _extentReports;

        [ThreadStatic]
        public static ExtentTest? _extentTestFeature;

        [ThreadStatic]
        public static ExtentTest? _extentTestScenario;

        [ThreadStatic]
        public static ExtentSparkReporter? _extentSparkReporter;

        [ThreadStatic]
        public static ExtentJsonFormatter? _jsonFormatter;

        [ThreadStatic]
        public static ExtentReports? _extentReportMerger;

        [ThreadStatic]
        public static ExtentSparkReporter? _extentSparkReportMerger;

        public static string rootFolder = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location)!;

        public static string testResultPath = Path.Combine(rootFolder, "TestResults");

        protected ExtentReporter()
        {
            //Instantiated to the class
        }

        /// <summary>
        /// Function to initialize Extent Spark Report & attach to Extent Reports
        /// </summary>
        public static void InitExtentReport()
        {
            string timeStamp = DateTime.Now.ToString("yyyyMMddHHmmssfff");
            Thread.Sleep(1000);
            _extentSparkReporter = new ExtentSparkReporter(testResultPath + $"\\SparkReportFiles\\SparkReport_{timeStamp}.html");
            Thread.Sleep(1000);
            _extentSparkReporter.Config.ReportName = "Defra Automation Execution Status Report";
            _extentSparkReporter.Config.DocumentTitle = "Defra Automation Execution Status Report";
            _extentSparkReporter.Config.Theme = Theme.Standard;
            _extentReports = new ExtentReports();
            Thread.Sleep(1000);
            _jsonFormatter = new ExtentJsonFormatter(testResultPath + $"\\ExtentJson\\SparkJson_{timeStamp}.json");
            Thread.Sleep(1000);
            _extentReports.CreateDomainFromJsonArchive(testResultPath + $"\\ExtentJson\\SparkJson_{timeStamp}.json");
            _extentReports.AttachReporter(_jsonFormatter, _extentSparkReporter);
            _extentReports.AddSystemInfo("Application", "Defra NCEA Search");
            _extentReports.AddSystemInfo("Browser", ConfigurationManager.AppSettings["Browser"]);
            _extentReports.AddSystemInfo("OS", "WINDOWS 11");
        }

        /// <summary>
        /// Function to flush out the Extent Report
        /// </summary>
        public static void FlushExtentReport()
        {
            _extentReports?.Flush();
        }

        /// <summary>
        /// Function to flush out the consolidated Extent Report
        /// </summary>
        /// <exception cref="Exception"></exception>
        public static void FlushConsolidateReport()
        {
            _extentSparkReportMerger = new ExtentSparkReporter(testResultPath + $"\\AutomationRunResult.html");
            _extentSparkReportMerger.Config.ReportName = "Defra Automation Execution Status Report";
            _extentSparkReportMerger.Config.DocumentTitle = "Defra Automation Execution Status Report";
            _extentSparkReportMerger.Config.Theme = Theme.Standard;
            _extentReportMerger = new ExtentReports();
            string[] jsonFiles = Directory.GetFiles(testResultPath + $"\\ExtentJson", "*.json");
            foreach (string file in jsonFiles)
            {
                try
                {
                    _extentReportMerger.CreateDomainFromJsonArchive(file);
                }
                catch (Exception ex)
                {
                    throw new Exception($"Error in consolidating Extent Report - {ex.Message}");
                }
            }

            try
            {
                _extentReportMerger.AttachReporter(_extentSparkReportMerger);
                _extentReportMerger.Flush();
            }
            catch (Exception ex)
            {
                throw new Exception($"Error in attaching Extent Report - {ex.Message}");
            }

        }

        /// <summary>
        /// Function to clean-up the files inside the specified directory
        /// </summary>
        /// <exception cref="Exception"></exception>
        [MethodImpl(MethodImplOptions.Synchronized)]
        public static void CleanUpResultFolder()
        {
            string[] jsonFiles = Directory.GetFiles(testResultPath + $"\\ExtentJson", "*.json");
            foreach (string file in jsonFiles)
            {
                try
                {
                    File.Delete(file);
                }
                catch (Exception ex)
                {
                    throw new Exception($"Error in deleting the files in specified directory - {ex.Message}");
                }
            }
        }

    }
}
