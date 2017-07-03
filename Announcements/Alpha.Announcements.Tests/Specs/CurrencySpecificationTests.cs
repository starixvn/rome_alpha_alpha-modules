namespace Alpha.Announcements.Tests.Specs
{
    using System.Collections.Generic;
    using Announcements.Specs;
    using Models;
    using NSubstitute;
    using NUnit.Framework;

    [TestFixture]
    public class CurrencySpecificationTests
    {
        [Test]
        public void IsSatisfiedBy_UserIsAllowedToViewAnnouncementByCurrency_ReturnTrue()
        {
            // Arrange
            var currencySpec = new CurrencySpecification(new List<byte> { 22, 30 });
            currencySpec.SpecificationOperator = SpecificationOperator.In;
            var param = Substitute.For<IParam>();
            byte currencyId = 22;
            param.CurrencyId.Returns(currencyId);

            // Act
            var actualIsSatisfiedBy = currencySpec.IsSatisfiedBy(param);

            // Assert
            Assert.IsTrue(actualIsSatisfiedBy);
        }

        [Test]
        public void IsSatisfiedBy_UserIsNotAllowedToViewAnnouncementByCurrency_ReturnFalse()
        {
            // Arrange
            var currencySpec = new CurrencySpecification(new List<byte> { 22, 30 });
            currencySpec.SpecificationOperator = SpecificationOperator.In;
            var param = Substitute.For<IParam>();
            byte currencyId = 55;
            param.CurrencyId.Returns(currencyId);

            // Act
            var actualIsSatisfiedBy = currencySpec.IsSatisfiedBy(param);

            // Assert
            Assert.IsFalse(actualIsSatisfiedBy);
        }

        [TestCase(5)]
        [TestCase(7)]
        public void IsSatisfiedBy_OperatorIsNotIn_UserCurrencyIsNotInList_ReturnTrue(byte userCurrencyId)
        {
            // Arrange
            var currencySpec = new CurrencySpecification(new List<byte> { 22, 30 });
            currencySpec.SpecificationOperator = SpecificationOperator.NotIn;
            var param = Substitute.For<IParam>();
            param.CurrencyId.Returns(userCurrencyId);

            // Act
            var actualIsSatisfiedBy = currencySpec.IsSatisfiedBy(param);

            // Assert
            Assert.IsTrue(actualIsSatisfiedBy);
        }

        [TestCase(22)]
        [TestCase(30)]
        public void IsSatisfiedBy_OperatorIsNotIn_UserCurrencyIsInList_ReturnFalse(byte userCurrencyId)
        {
            // Arrange
            var currencySpec = new CurrencySpecification(new List<byte> { 22, 30 });
            currencySpec.SpecificationOperator = SpecificationOperator.NotIn;
            var param = Substitute.For<IParam>();
            param.CurrencyId.Returns(userCurrencyId);

            // Act
            var actualIsSatisfiedBy = currencySpec.IsSatisfiedBy(param);

            // Assert
            Assert.IsFalse(actualIsSatisfiedBy);
        }
    }
}