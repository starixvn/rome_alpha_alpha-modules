namespace Alpha.Logging.Tests.LogEngines
{
    using System.Collections.Specialized;
    using Alpha.Logging.Configurations;
    using Alpha.Logging.LogEngines;
    using Fanex.Logging.RabbitMQ;
    using NUnit.Framework;

    [TestFixture]
    public class RabbitLogEngineTests
    {
        [SetUp]
        public void SetUp()
        {
            AppSettings.AppSettingValues = new NameValueCollection
            {
                ["Fanex.Logging:RabbitMQConnectionStrings"] = "host=10.18.200.103;virtualHost=/Fanex_Logging;username=fanexlogging;password=YKQrMtla7oYZGlP8apPtSw=="
            };
        }

        [TestCase("host=10.18.200.103;virtualHost=/Fanex_Logging;username=fanexlogging;password=YKQrMtla7oYZGlP8apPtSw==")]
        public void Create_ReturnRabbitMQLogging(string rabbitMQConnectionString)
        {
            // Arrange
            var expectedValue = typeof(RabbitMQLogging);

            // Act
            var rabbitLogging = new RabbitLogEngine().Create();

            // Assert
            Assert.AreEqual(expectedValue, rabbitLogging.GetType());
        }
    }
}
