namespace Alpha.Announcements.Tests.Specs
{
    using Announcements.Specs;
    using Models;
    using NSubstitute;
    using NUnit.Framework;

    [TestFixture]
    public class EnableYesterdayTransferSpecificationTests
    {
        [Test]
        public void IsSatisfiedBy_UserIsEnableYesterdayTransfer_ReturnTrue()
        {
            // Arrange
            var enableYesterdayTransferSpec = new EnableYesterdayTransferSpecification();
            enableYesterdayTransferSpec.SpecificationOperator = SpecificationOperator.In;
            var param = Substitute.For<IParam>();
            param.EnableYesterdayTransfer.Returns(true);

            // Act
            var actualIsSatisfiedBy = enableYesterdayTransferSpec.IsSatisfiedBy(param);

            // Assert
            Assert.IsTrue(actualIsSatisfiedBy);
        }

        [Test]
        public void IsSatisfiedBy_UserIsEnableYesterdayTransfer_ReturnFalse()
        {
            // Arrange
            var enableYesterdayTransferSpec = new EnableYesterdayTransferSpecification();
            enableYesterdayTransferSpec.SpecificationOperator = SpecificationOperator.In;
            var param = Substitute.For<IParam>();
            param.EnableYesterdayTransfer.Returns(false);

            // Act
            var actualIsSatisfiedBy = enableYesterdayTransferSpec.IsSatisfiedBy(param);

            // Assert
            Assert.IsFalse(actualIsSatisfiedBy);
        }
    }
}