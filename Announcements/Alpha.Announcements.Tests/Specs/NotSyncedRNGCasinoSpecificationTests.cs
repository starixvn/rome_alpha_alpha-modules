namespace Alpha.Announcements.Tests.Specs
{
    using Announcements.Specs;
    using Models;
    using NSubstitute;
    using NUnit.Framework;

    [TestFixture]
    public class NotSyncedRnGCasinoSpecificationTests
    {
        [TestCase(true, false)]
        [TestCase(false, true)]
        public void IsSatisfiedBy_UserSyncedCasinoIsOppositeSpecValue_ReturnFalse(
            bool userSyncedCasino,
            bool specSyncedCasino)
        {
            // Arrange
            var notSyncCasinoSpec = new SyncedRnGCasinoSpecification(specSyncedCasino);
            notSyncCasinoSpec.SpecificationOperator = SpecificationOperator.In;
            var param = Substitute.For<IParam>();
            param.IsSyncedCasino.Returns(userSyncedCasino);

            // Act
            var actualIsSatisfiedBy = notSyncCasinoSpec.IsSatisfiedBy(param);

            // Assert
            Assert.IsFalse(actualIsSatisfiedBy);
        }

        [TestCase(true, true)]
        [TestCase(false, false)]
        public void IsSatisfiedBy_UserSyncedCasinoIsSameSpecValue_ReturnTrue(
            bool userSyncedCasino,
            bool specSyncedCasino)
        {
            // Arrange
            var notSyncCasinoSpec = new SyncedRnGCasinoSpecification(specSyncedCasino);
            notSyncCasinoSpec.SpecificationOperator = SpecificationOperator.In;
            var param = Substitute.For<IParam>();
            param.IsSyncedCasino.Returns(userSyncedCasino);

            // Act
            var actualIsSatisfiedBy = notSyncCasinoSpec.IsSatisfiedBy(param);

            // Assert
            Assert.IsTrue(actualIsSatisfiedBy);
        }
    }
}