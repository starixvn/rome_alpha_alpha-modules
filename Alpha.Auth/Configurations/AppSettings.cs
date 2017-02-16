namespace Alpha.Auth.Configurations
{
    using System.Configuration;

    public static class AppSettings
    {
        public static bool IsDisabledCheckDuplicationLogin => GetConfigValue("DisabledCheckDuplicationLogin") == "1";

        public static string GetConfigValue(string configName) => ConfigurationManager.AppSettings[configName];
    }
}