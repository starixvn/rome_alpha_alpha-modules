namespace Alpha.Announcements.Tests.Specs
{
    using System.Collections.Generic;
    using Announcements.Specs;
    using Models;
    using NSubstitute;
    using NUnit.Framework;

    [TestFixture]
    public class UserLevelSpecificationTests
    {
        [TestCase(1)]
        [TestCase(2)]
        public void IsSatisfiedBy_UserIsAllowedToViewAnnouncementByUserLevel_ReturnTrue(byte userLevel)
        {
            // Arrange
            var userLevelSpec = new UserLevelSpecification(new List<byte> { 1, 2 });
            userLevelSpec.SpecificationOperator = SpecificationOperator.In;
            var param = Substitute.For<IParam>();
            param.LevelId.Returns(userLevel);

            // Act
            var actualIsSatisfiedBy = userLevelSpec.IsSatisfiedBy(param);

            // Assert
            Assert.IsTrue(actualIsSatisfiedBy);
        }

        [TestCase(1)]
        [TestCase(2)]
        [TestCase(3)]
        public void IsSatisfiedBy_AllowAllUserLevelsViewAnnouncement_ReturnTrue(byte userLevel)
        {
            // Arrange
            var userLevelSpec = new UserLevelSpecification(new List<byte> { 0 });
            userLevelSpec.SpecificationOperator = SpecificationOperator.In;
            var param = Substitute.For<IParam>();
            param.LevelId.Returns(userLevel);

            // Act
            var actualIsSatisfiedBy = userLevelSpec.IsSatisfiedBy(param);

            // Assert
            Assert.IsTrue(actualIsSatisfiedBy);
        }

        [TestCase(4)]
        [TestCase(2)]
        public void IsSatisfiedBy_UserIsNotAllowedToViewAnnouncementByUserLevel_ReturnFalse(byte userLevel)
        {
            // Arrange
            var userLevelSpec = new UserLevelSpecification(new List<byte> { 1 });
            userLevelSpec.SpecificationOperator = SpecificationOperator.In;
            var param = Substitute.For<IParam>();
            param.LevelId.Returns(userLevel);

            // Act
            var actualIsSatisfiedBy = userLevelSpec.IsSatisfiedBy(param);

            // Assert
            Assert.IsFalse(actualIsSatisfiedBy);
        }

        [TestCase(4)]
        [TestCase(2)]
        public void IsSatisfiedBy_OperatorIsNotIn_UserLevelIsNotInUserLevelList_ReturnTrue(byte userLevel)
        {
            // Arrange
            var userLevelSpec = new UserLevelSpecification(new List<byte> { 1 });
            userLevelSpec.SpecificationOperator = SpecificationOperator.NotIn;
            var param = Substitute.For<IParam>();
            param.LevelId.Returns(userLevel);

            // Act
            var actualIsSatisfiedBy = userLevelSpec.IsSatisfiedBy(param);

            // Assert
            Assert.IsTrue(actualIsSatisfiedBy);
        }

        [TestCase(4)]
        [TestCase(2)]
        [TestCase(1)]
        public void IsSatisfiedBy_NotAllowAllUserLevel_ReturnFalse(byte userLevel)
        {
            // Arrange
            var userLevelSpec = new UserLevelSpecification(new List<byte> { 0 });
            userLevelSpec.SpecificationOperator = SpecificationOperator.NotIn;
            var param = Substitute.For<IParam>();
            param.LevelId.Returns(userLevel);

            // Act
            var actualIsSatisfiedBy = userLevelSpec.IsSatisfiedBy(param);

            // Assert
            Assert.IsFalse(actualIsSatisfiedBy);
        }
    }
}