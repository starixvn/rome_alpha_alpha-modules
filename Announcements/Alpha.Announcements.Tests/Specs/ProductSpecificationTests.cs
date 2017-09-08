namespace Alpha.Announcements.Tests.Specs
{
    using System.Collections.Generic;
    using Announcements.Specs;
    using Models;
    using NSubstitute;
    using NUnit.Framework;

    [TestFixture]
    public class ProductSpecificationTests
    {
        private readonly List<byte> OfferedProductIds = new List<byte> { 22, 30, 50, 51 };

        [Test]
        public void IsSatisfiedBy_UserIsAllowedToViewAnnouncementByProduct_ReturnTrue()
        {
            // Arrange
            var productSpec = new ProductSpecification(new List<byte> { 22, 33 });
            productSpec.SpecificationOperator = SpecificationOperator.In;
            var param = Substitute.For<IParam>();
            param.OfferedProductIds.Returns(OfferedProductIds);

            // Act
            var actualIsSatisfiedBy = productSpec.IsSatisfiedBy(param);

            // Assert
            Assert.IsTrue(actualIsSatisfiedBy);
        }

        [Test]
        public void IsSatisfiedBy_UserIsNotAllowedToViewAnnouncementByProduct_ReturnFalse()
        {
            // Arrange
            var productSpec = new ProductSpecification(new List<byte> { 12, 17 });
            productSpec.SpecificationOperator = SpecificationOperator.In;
            var param = Substitute.For<IParam>();
            param.OfferedProductIds.Returns(OfferedProductIds);

            // Act
            var actualIsSatisfiedBy = productSpec.IsSatisfiedBy(param);

            // Assert
            Assert.IsFalse(actualIsSatisfiedBy);
        }

        [Test]
        public void IsSatisfiedBy_OperatorIsNotIn_UserProductIsNotInOfferdProductList_ReturnTrue()
        {
            // Arrange
            var productSpec = new ProductSpecification(new List<byte> { 5, 7 });
            productSpec.SpecificationOperator = SpecificationOperator.NotIn;
            var param = Substitute.For<IParam>();
            param.OfferedProductIds.Returns(OfferedProductIds);

            // Act
            var actualIsSatisfiedBy = productSpec.IsSatisfiedBy(param);

            // Assert
            Assert.IsTrue(actualIsSatisfiedBy);
        }

        [Test]
        public void IsSatisfiedBy_OperatorIsNotIn_UserProductIsInOfferdProductList_ReturnFalse()
        {
            // Arrange
            var productSpec = new ProductSpecification(new List<byte> { 22, 30 });
            productSpec.SpecificationOperator = SpecificationOperator.NotIn;
            var param = Substitute.For<IParam>();
            param.OfferedProductIds.Returns(OfferedProductIds);

            // Act
            var actualIsSatisfiedBy = productSpec.IsSatisfiedBy(param);

            // Assert
            Assert.IsFalse(actualIsSatisfiedBy);
        }
    }
}