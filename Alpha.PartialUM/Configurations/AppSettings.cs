namespace Alpha.PartialUM.Configurations
{
    using System.Configuration;

    internal static class AppSettings
    {
        static AppSettings()
        {
            var appSettings = ConfigurationManager.AppSettings;

            PartialUMReportsBetListsCacheInSeconds = ToInt(appSettings["PartialUM:ReportsBetListsCacheInSeconds"], 15);
        }

        public static int PartialUMReportsBetListsCacheInSeconds { get; }

        private static int ToInt(string value, int defaultValue = 0)
            => string.IsNullOrWhiteSpace(value) || !int.TryParse(value, out var valueInt)
            ? defaultValue
            : valueInt;
    }
}
