namespace Alpha.Logging.Tests
{
    using System;
    using Fanex.Logging.Extensions;
    using NUnit.Framework;

    [TestFixture]
    public class ExceptionHandlerAdapterTests
    {
        [Test]
        public void UseConfig_NotNullLoggingConfiguration_ReturnLoggingCongifuration()
        {
            // Arrange
            var handler = new ExceptionHandlerAdapter();
            var loggingConfig = new LoggingConfiguration();
            var expectedValue = loggingConfig;

            // Act
            var result = handler.UseConfig(loggingConfig);

            // Asset
            Assert.AreEqual(expectedValue, result);
        }

        [Test]
        public void UseConfig_NullLoggingConfiguration_ThrowArgumentNullException()
        {
            // Asset
            Assert.Throws<ArgumentNullException>(() => new ExceptionHandlerAdapter().UseConfig(null));
        }
    }
}
