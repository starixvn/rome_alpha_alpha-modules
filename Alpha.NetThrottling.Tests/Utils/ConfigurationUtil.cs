namespace Alpha.NetThrottling.Tests.Utils
{
    using System;
    using System.Configuration;
    using System.IO;

    public static class ConfigurationUtil
    {
        public static Configuration Load(string relativePath)
        {
            var configFileMap = new ExeConfigurationFileMap()
            {
                ExeConfigFilename = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, relativePath)
            };
            return ConfigurationManager.OpenMappedExeConfiguration(configFileMap, ConfigurationUserLevel.None);
        }

        public static T Load<T>(string relativePath, string sectionName) where T: class
        {
            var configuration = Load(relativePath);
            return configuration.GetSection(sectionName) as T;
        }
    }
}
