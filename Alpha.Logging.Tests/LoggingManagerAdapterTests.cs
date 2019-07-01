namespace Alpha.Logging.Tests
{
    using Fanex.Logging;
    using KellermanSoftware.CompareNetObjects;
    using NUnit.Framework;

    [TestFixture]
    public class LoggingManagerAdapterTests
    {
        [Test]
        public void SetDefaultLogCategory_EmptyString_ReturnLogManager()
        {
            // Arrange
            var handler = new LogManagerAdapter();
            var expectRessult = new LogManager();

            // Act
            var result = handler.SetDefaultLogCategory(string.Empty);

            // Assert
            Assert.True(new CompareLogic().Compare(expectRessult, result).AreEqual);
        }
    }
}
