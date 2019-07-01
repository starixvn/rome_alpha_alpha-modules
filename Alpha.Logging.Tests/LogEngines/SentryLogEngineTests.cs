namespace Alpha.Logging.Tests.LogEngines
{
    using System.Collections.Specialized;
    using Alpha.Logging.Configurations;
    using Alpha.Logging.LogEngines;
    using Fanex.Logging.Sentry.RavenClient;
    using NUnit.Framework;

    [TestFixture]
    public class SentryLogEngineTests
    {
        [SetUp]
        public void SetUp()
        {
            AppSettings.AppSettingValues = new NameValueCollection
            {
                ["Fanex.Logging:DefaultCategory"] = "Alpha-MBC-AF-Local",
                ["Fanex.Logging:SentryUrl"] = "http://a@sentry.nexdev.net/43",
                ["Fanex.Logging:SentryBackendUrl"] = "http://a@sentry.nexdev.net/43",
                ["AssetAppPath"] = "path"
            };
        }

        [Test]
        public void Create_Always_ReturnSentryLogging()
        {
            // Arrange
            var expectedValue = typeof(SentryLogging);

            // Act
            var sentryLogging = new SentryLogEngine().Create();

            // Assert
            Assert.AreEqual(expectedValue, sentryLogging.GetType());
        }
    }
}
