namespace Alpha.NetThrottling.Tests.Utils
{
    using System;
    using System.Configuration;
    using System.IO;

    public sealed class NetThrottlingConfigurationFixture : IDisposable
    {
        public NetThrottlingConfiguration Configuration { get; private set; }

        public NetThrottlingConfigurationFixture()
        {
            var configFileMap = new ExeConfigurationFileMap()
            {
                ExeConfigFilename = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "NetThrottlerTests.config")
            };
            var config = ConfigurationManager.OpenMappedExeConfiguration(configFileMap, ConfigurationUserLevel.None);
            this.Configuration = config.GetSection("Alpha.NetThrottling") as NetThrottlingConfiguration;
        }

        public void Dispose()
        {
            // Cleanup
        }
    }
}
