using NUnit.Framework;
using OpenQA.Selenium.DevTools.V120.Page;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Defra.TestAutomation.Specs.FrameworkUtilities
{
    [Binding]
    [Parallelizable]
    public class ConfigReader
    {
        [ThreadStatic]
        public static Configuration? _config;

        protected ConfigReader()
        {
            //To prevent instantiation of class
        }

        public static string ReadConfig(string key)
        {
            try
            {
                _config = ConfigurationManager.OpenExeConfiguration(Assembly.GetExecutingAssembly().Location);
            }
            catch(Exception ex)
            {
                throw new Exception($"Exception on reading config file - {ex.Message}");
            }
            if(_config != null)
            {
                string configValue = GetAppSetting(_config, key);
                return configValue;
            }
            return string.Empty;
        }

        public static string GetAppSetting(Configuration config, string key)
        {
            try
            {
                KeyValueConfigurationElement element = config.AppSettings.Settings[key];
                if (element != null)
                {
                    return element.Value;
                }
                return string.Empty;
            } catch(Exception e)
            {
                throw new Exception($"Exception on reading App Setting section - {e.Message}");
            }
        }
    }
}
