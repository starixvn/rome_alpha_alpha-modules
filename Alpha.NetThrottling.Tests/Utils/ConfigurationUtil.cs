namespace Alpha.NetThrottling.Tests.Utils
{
    using System;
    using System.Configuration;
    using System.IO;

    public static class ConfigurationUtil
    {
        public static NetThrottlingConfiguration LoadConfiguration(string file)
        {
            var configFileMap = new ExeConfigurationFileMap()
            {
                ExeConfigFilename = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, file)
            };
            var config = ConfigurationManager.OpenMappedExeConfiguration(configFileMap, ConfigurationUserLevel.None);
            return config.GetSection("Alpha.NetThrottling") as NetThrottlingConfiguration;
        }
    }
}
