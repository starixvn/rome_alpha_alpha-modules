namespace Alpha.Logging.Tests.Configurations
{
    using System.Collections.Specialized;
    using Alpha.Logging.Configurations;
    using NUnit.Framework;

    [TestFixture]
    public class AppSettingsTests
    {
        [SetUp]
        public void Setup()
        {
            AppSettings.AppSettingValues = new NameValueCollection
            {
                ["LogLibs"] = "Sentry",
                ["Fanex.Logging:DefaultCategory"] = "Alpha",
                ["Fanex.Logging:SentryUrl"] = "http",
                ["Fanex.Logging:RabbitMQConnectionStrings"] = "host",
                ["Fanex.Logging:SentryBackendUrl"] = "http:",
                ["AssetAppPath"] = "path"
            };
        }

        [Test]
        public void GetLoblibs_Always_ReturnCorrectValue()
        {
            // Arrange
            var expectedLogLibs = "Sentry";

            // Act
            var result = AppSettings.LogLibs;

            // Assert
            Assert.AreEqual(expectedLogLibs, result);
        }

        [Test]
        public void GetDefaultCategory_Always_ReturnCorrectValue()
        {
            // Arrange
            var expectedDefaultCategory = "Alpha";

            // Act
            var result = AppSettings.LoggingDefaultCategory;

            // Assert
            Assert.AreEqual(expectedDefaultCategory, result);
        }

        [Test]
        public void GetSentryUrl_Always_ReturnCorrectValue()
        {
            // Arrange
            var expectedSentryUrl = "http";

            // Act
            var result = AppSettings.SentryUrl;

            // Assert
            Assert.AreEqual(expectedSentryUrl, result);
        }

        [Test]
        public void GetSentryBackendUrl_Always_ReturnCorrectValue()
        {
            // Arrange
            var expectedSentryBackendUrl = "http:";

            // Act
            var result = AppSettings.SentryBackendUrl;

            // Assert
            Assert.AreEqual(expectedSentryBackendUrl, result);
        }

        [Test]
        public void GetRabbitMQConnectionStrings_Always_ReturnCorrectValue()
        {
            //Arrange
            var expectedRabbitMQConnectionStrings = "host";

            // Act
            var result = AppSettings.RabbitMQConnectionStrings;

            // Assert
            Assert.AreEqual(expectedRabbitMQConnectionStrings, result);
        }

        [Test]
        public void GetAppSettingValues_Always_ReturnCorrectValue()
        {
            // Arrange
            var expectedAppSettingValues = "path";

            // Act
            var result = AppSettings.AssetAppPath;

            // Assert
            Assert.AreEqual(expectedAppSettingValues, result);
        }
    }
}
