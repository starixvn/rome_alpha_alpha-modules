namespace Alpha.Announcements.Tests.Specs
{
    using Announcements.Specs;
    using Models;
    using NSubstitute;
    using NUnit.Framework;

    [TestFixture]
    public class EnableYesterdayTransferSpecificationTests
    {
        [TestCase(true, false)]
        [TestCase(false, true)]
        public void IsSatisfiedBy_UserEnableYesterdayTransferIsOppositeSpecValue_ReturnFalse(
            bool userEnableYesterdayTransfer,
            bool specEnableYesterdayTransfer)
        {
            // Arrange
            var enableYesterdayTransferSpec = new EnableYesterdayTransferSpecification(specEnableYesterdayTransfer);
            enableYesterdayTransferSpec.SpecificationOperator = SpecificationOperator.In;
            var param = Substitute.For<IParam>();
            param.EnableYesterdayTransfer.Returns(userEnableYesterdayTransfer);

            // Act
            var actualIsSatisfiedBy = enableYesterdayTransferSpec.IsSatisfiedBy(param);

            // Assert
            Assert.IsFalse(actualIsSatisfiedBy);
        }

        [TestCase(true, true)]
        [TestCase(false, false)]
        public void IsSatisfiedBy_UserIsNotEnableYesterdayTransfer_ReturnTrue(
            bool userEnableYesterdayTransfer,
            bool specEnableYesterdayTransfer)
        {
            // Arrange
            var enableYesterdayTransferSpec = new EnableYesterdayTransferSpecification(specEnableYesterdayTransfer);
            enableYesterdayTransferSpec.SpecificationOperator = SpecificationOperator.In;
            var param = Substitute.For<IParam>();
            param.EnableYesterdayTransfer.Returns(userEnableYesterdayTransfer);

            // Act
            var actualIsSatisfiedBy = enableYesterdayTransferSpec.IsSatisfiedBy(param);

            // Assert
            Assert.IsTrue(actualIsSatisfiedBy);
        }
    }
}