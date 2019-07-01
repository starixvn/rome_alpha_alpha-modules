namespace Alpha.Logging.LogEngines
{
    using Alpha.Logging.Configurations;
    using Fanex.Logging;
    using Fanex.Logging.Sentry.RavenClient;

    public class SentryLogEngine : LogEngineFactory
    {
        public ILogEngine Create()
        {
            var sentryOptions = new SentryEngineOptions
            {
                Dsn = new SharpRaven.Dsn(AppSettings.SentryBackendUrl),
                FrontEndDsn = new SharpRaven.Dsn(AppSettings.SentryUrl)
            };

            return new SentryLogging(sentryOptions, $"{AppSettings.AssetAppPath}/bundles/common/raven.min.js");
        }
    }
}
