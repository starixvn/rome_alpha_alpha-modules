namespace Alpha.Logging.Tests
{
    using System.Collections.Generic;
    using System.Collections.Specialized;
    using System.Linq;
    using Alpha.Logging.Configurations;
    using Fanex.Logging;
    using Fanex.Logging.Extensions;
    using Fanex.Logging.Extensions.Database;
    using KellermanSoftware.CompareNetObjects;
    using NSubstitute;
    using NUnit.Framework;

    [TestFixture]
    public class LoggingConfigTests
    {
        private CompareLogic comparer;
        [SetUp]
        public void SetUp()
        {
            AppSettings.AppSettingValues = new NameValueCollection
            {
                ["LogLibs"] = "Sentry,Rabbit",
                ["Fanex.Logging:DefaultCategory"] = "Alpha-MBC-AF-Local",
                ["Fanex.Logging:SentryUrl"] = "http://a@sentry.nexdev.net/43",
                ["Fanex.Logging:RabbitMQConnectionStrings"] = "host=10.18.200.103;virtualHost=/Fanex_Logging;username=fanexlogging;password=YKQrMtla7oYZGlP8apPtSw==",
                ["Fanex.Logging:SentryBackendUrl"] = "http://a@sentry.nexdev.net/43",
                ["AssetAppPath"] = "path"
            };
            comparer = new CompareLogic();
        }

        [Test]
        public void Init_Always_SetConfigurationForExceptionHandler()
        {
            // Arrange
            var sub = Substitute.For<ExceptionHandlerAdapter>();
            LoggingConfig.ExceptionHandler = sub;

            // Act
            LoggingConfig.Init();

            // Assert
            sub.Received().UseConfig(Arg.Any<LoggingConfiguration>());
        }

        [Test]
        public void Init_Always_SetDefaultCategoryForLogManger()
        {
            // Arrange
            var sub = Substitute.For<LogManagerAdapter>();
            LoggingConfig.LogManager = sub;
            var logManager = Substitute.ForPartsOf<LogManager>();
            sub.SetDefaultLogCategory(Arg.Any<string>()).Returns(logManager);

            // Act
            LoggingConfig.Init();

            // Assert
            sub.Received().SetDefaultLogCategory("Alpha-MBC-AF-Local");
        }

        [Test]
        public void Init_GetDbExceptionInfoIsNull_CorrectLogconfiguration()
        {
            // Arrange
            var expectedValue = new LoggingConfiguration();

            // Act
            LoggingConfig.Init();

            // Assert
            Assert.True(comparer.Compare(expectedValue, LoggingConfig.LoggingConfiguration).AreEqual);
        }

        [Test]
        public void Init_GetDbExceptionInfoIsNotNull_CorrectLogconfiguration()
        {
            // Arrange
            var expectedValue = new LoggingConfiguration();

            // Act
            LoggingConfig.Init(getDbExceptionInfo: DatabaseInfoGetters.GetDbExceptionInfo);

            // Assert
            Assert.True(comparer.Compare(expectedValue, LoggingConfig.LoggingConfiguration).AreEqual);
        }

        [Test]
        public void GetRegexToHideSensitiveInfo_EmptySensitiveInfo_EmptyStringList()
        {
            // Arrange
            var sensitiveInfoName = Enumerable.Empty<string>();

            // Act
            var sensitiveInfos = LoggingConfig.GetRegexToHideSensitiveInfo(sensitiveInfoName);

            // Assert
            Assert.IsEmpty(sensitiveInfos);
        }

        [Test]
        public void GetRegexToHideSensitiveInfo_SingleSensitiveInfo_ReturnCorrectValue()
        {
            // Arrange
            var sensitiveInfoName = new[] { "password" };
            var expectedValue = new List<string>
            {
                "(?:password=)(?<paramValue>[^&|^\\s]+)",
                "(\"password\":)(?<paramValue>[^,|^\r]+)"
            };

            // Act
            var sensitiveInfos = LoggingConfig.GetRegexToHideSensitiveInfo(sensitiveInfoName);

            // Assert
            Assert.AreEqual(sensitiveInfos, expectedValue);
        }

        [Test]
        public void GetRegexToHideSensitiveInfo_MultipleSensitiveInfo_ReturnCorrectValue()
        {
            // Arrange
            var sensitiveInfoName = new[] { "password", "code" };
            var expectedValues = new List<string>
            {
                "(?:password=)(?<paramValue>[^&|^\\s]+)",
                "(\"password\":)(?<paramValue>[^,|^\r]+)",
                "(?:code=)(?<paramValue>[^&|^\\s]+)",
                "(\"code\":)(?<paramValue>[^,|^\r]+)"
            };

            // Act
            var sensitiveInfos = LoggingConfig.GetRegexToHideSensitiveInfo(sensitiveInfoName);

            // Assert
            Assert.AreEqual(sensitiveInfos, expectedValues);
        }

        [Test]
        public void GetRegexToHideSensitiveInfo_NullSensitiveInfo_EmptyStringList()
        {
            // Act
            var sensitiveInfos = LoggingConfig.GetRegexToHideSensitiveInfo(null);

            //Assert
            Assert.IsEmpty(sensitiveInfos);
        }

        [TestCase("Sentry", "SentryLogging")]
        [TestCase("Rabbit", "RabbitMQLogging")]
        public void CreateLobLib_Sentry_ReturnCorrectLoggingType(string lib, string expectedValue)
        {
            // Act
            var logEngine = LoggingConfig.CreateLogLib(lib);
            var logEngineTypeName = logEngine.GetType().Name;

            // Assert
            Assert.AreEqual(expectedValue, logEngineTypeName);
        }

        [Test]
        public void CreateLobLib_InvalidLogEnging_ReturnNull()
        {
            // Act
            var logEngine = LoggingConfig.CreateLogLib("Something");

            // Assert
            Assert.IsNull(logEngine);
        }
    }
}
