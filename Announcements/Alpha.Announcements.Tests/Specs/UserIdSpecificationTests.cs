namespace Alpha.Announcements.Tests.Specs
{
    using System.Collections.Generic;
    using Announcements.Specs;
    using Models;
    using NSubstitute;
    using NUnit.Framework;

    [TestFixture]
    public class UserIdSpecificationTests
    {
        [Test]
        public void IsSatisfiedBy_UserIsAllowedToViewAnnouncementByUserId_ReturnTrue()
        {
            // Arrange
            var userIdsSpec = new UserIdSpecification(new List<int> { 22, 30 });
            userIdsSpec.SpecificationOperator = SpecificationOperator.In;
            var param = Substitute.For<IParam>();
            param.UserId.Returns(22);

            // Act
            var actualIsSatisfiedBy = userIdsSpec.IsSatisfiedBy(param);

            // Assert
            Assert.IsTrue(actualIsSatisfiedBy);
        }

        [Test]
        public void IsSatisfiedBy_UserIsNotAllowedToViewAnnouncementByUserId_ReturnFalse()
        {
            // Arrange
            var userIdsSpec = new UserIdSpecification(new List<int> { 22, 30 });
            userIdsSpec.SpecificationOperator = SpecificationOperator.In;
            var param = Substitute.For<IParam>();
            param.UserId.Returns(55);

            // Act
            var actualIsSatisfiedBy = userIdsSpec.IsSatisfiedBy(param);

            // Assert
            Assert.IsFalse(actualIsSatisfiedBy);
        }

        [TestCase(5)]
        [TestCase(7)]
        public void IsSatisfiedBy_OperatorIsNotIn_UserIdIsNotInList_ReturnTrue(int userId)
        {
            // Arrange
            var userIdsSpec = new UserIdSpecification(new List<int> { 22, 30 });
            userIdsSpec.SpecificationOperator = SpecificationOperator.NotIn;
            var param = Substitute.For<IParam>();
            param.UserId.Returns(userId);

            // Act
            var actualIsSatisfiedBy = userIdsSpec.IsSatisfiedBy(param);

            // Assert
            Assert.IsTrue(actualIsSatisfiedBy);
        }

        [TestCase(22)]
        [TestCase(30)]
        public void IsSatisfiedBy_OperatorIsNotIn_UserIdIsInList_ReturnFalse(int userId)
        {
            // Arrange
            var userIdsSpec = new UserIdSpecification(new List<int> { 22, 30 });
            userIdsSpec.SpecificationOperator = SpecificationOperator.NotIn;
            var param = Substitute.For<IParam>();
            param.UserId.Returns(userId);

            // Act
            var actualIsSatisfiedBy = userIdsSpec.IsSatisfiedBy(param);

            // Assert
            Assert.IsFalse(actualIsSatisfiedBy);
        }
    }
}