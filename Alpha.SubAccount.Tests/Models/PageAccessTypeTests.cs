namespace Alpha.SubAccount.Tests.Models
{
    using System.Collections;
    using NUnit.Framework;
    using Sunplus.Agent.Model.Entities;

    [TestFixture]
    public class PageAccessTypeTests
    {
        [Test]
        public void AccessTypes_Always_ReturnsCorrectTypes()
        {
            // Arrange
            var expectedTypes = new Hashtable
            {
                ["[A]"] = false,
                ["[B]"] = false,
                ["[C]"] = false,
                ["[D]"] = false,
                ["[E]"] = false,
                ["[G]"] = false,
                ["[H]"] = false,
                ["[L]"] = false,
            };

            // Act
            var actualTypes = PageAccessType.AccessTypes;

            // Assert
            Assert.AreEqual(expectedTypes, actualTypes);
        }

        [TestCase("[A]")]
        [TestCase("[B]")]
        [TestCase("[C]")]
        [TestCase("[D]")]
        [TestCase("[E]")]
        [TestCase("[G]")]
        [TestCase("[H]")]
        [TestCase("[L]")]
        public void IsValidType_ValidType_ReturnsTrue(string type)
        {
            // Act
            var actulaIsValid = PageAccessType.IsValidType(type);

            // Assert
            Assert.IsTrue(actulaIsValid);
        }

        [TestCase("[]")]
        [TestCase("A")]
        [TestCase("[A")]
        [TestCase("A]")]
        [TestCase("[A][B]")]
        [TestCase("[A]B")]
        [TestCase("[F]")]
        [TestCase("[J]")]
        public void IsValidType_InvalidType_ReturnsFalse(string type)
        {
            // Act
            var actulaIsValid = PageAccessType.IsValidType(type);

            // Assert
            Assert.IsFalse(actulaIsValid);
        }
    }
}
