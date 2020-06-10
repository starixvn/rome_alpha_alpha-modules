namespace Alpha.Auth.Configurations
{
    using System;
    using System.Configuration;

    public static class AppSettings
    {
        public static bool IsDisabledCheckDuplicationLogin => GetConfigValue("DisabledCheckDuplicationLogin") == "1";

        public static string[] OfferClearGivenCreditSites
            => string.IsNullOrWhiteSpace(GetConfigValue("OfferClearGivenCredit:Sites")) ?
            new string[] { } : GetConfigValue("OfferClearGivenCredit:Sites").Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries);

        public static string GetConfigValue(string configName) => ConfigurationManager.AppSettings[configName];
    }
}