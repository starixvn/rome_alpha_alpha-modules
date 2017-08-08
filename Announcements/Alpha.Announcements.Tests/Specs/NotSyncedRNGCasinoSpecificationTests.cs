namespace Alpha.Announcements.Tests.Specs
{
    using Announcements.Specs;
    using Models;
    using NSubstitute;
    using NUnit.Framework;

    [TestFixture]
    public class NotSyncedRNGCasinoSpecificationTests
    {
        [Test]
        public void IsSatisfiedBy_UserIsSyncCasino_ReturnFalse()
        {
            // Arrange
            var notSyncCasinoSpec = new NotSyncedRNGCasinoSpecification();
            notSyncCasinoSpec.SpecificationOperator = SpecificationOperator.In;
            var param = Substitute.For<IParam>();
            param.IsSyncedCasino.Returns(true);

            // Act
            var actualIsSatisfiedBy = notSyncCasinoSpec.IsSatisfiedBy(param);

            // Assert
            Assert.IsFalse(actualIsSatisfiedBy);
        }

        [Test]
        public void IsSatisfiedBy_UserIsNotSyncCasino_ReturnTrue()
        {
            // Arrange
            var notSyncCasinoSpec = new NotSyncedRNGCasinoSpecification();
            notSyncCasinoSpec.SpecificationOperator = SpecificationOperator.In;
            var param = Substitute.For<IParam>();
            param.IsSyncedCasino.Returns(false);

            // Act
            var actualIsSatisfiedBy = notSyncCasinoSpec.IsSatisfiedBy(param);

            // Assert
            Assert.IsTrue(actualIsSatisfiedBy);
        }
    }
}