namespace Alpha.Announcements.UplineIdRule.Tests
{
    using System.Collections.Generic;
    using Models;
    using NSubstitute;
    using NUnit.Framework;
    using Specs;
    using UplineIds.Specs;

    [TestFixture]
    public class UplineIdsSpecificationTest
    {
        [Test]
        public void IsSatisfiedBy_OperatorIsIn_MasterIsValid_ReturnTrue()
        {
            // Arrange
            var uplineIdsRule = new UplineIdsSpecification(new List<int> { 22, 33 });
            uplineIdsRule.SpecificationOperator = SpecificationOperator.In;
            var param = Substitute.For<IParam>();
            param.MRecommendId.Returns(22);

            // Act
            var actualIsSatisfiedBy = uplineIdsRule.IsSatisfiedBy(param);

            // Assert
            Assert.IsTrue(actualIsSatisfiedBy);
        }

        [Test]
        public void IsSatisfiedBy_OperatorIsIn_SuperIsValid_ReturnTrue()
        {
            // Arrange
            var uplineIdsRule = new UplineIdsSpecification(new List<int> { 22, 33 });
            uplineIdsRule.SpecificationOperator = SpecificationOperator.In;
            var param = Substitute.For<IParam>();
            param.SRecommendId.Returns(33);

            // Act
            var actualIsSatisfiedBy = uplineIdsRule.IsSatisfiedBy(param);

            // Assert
            Assert.IsTrue(actualIsSatisfiedBy);
        }

        [Test]
        public void IsSatisfiedBy_OperatorIsNotIn_MasterIsValid_ReturnFalse()
        {
            // Arrange
            var uplineIdsRule = new UplineIdsSpecification(new List<int> { 22, 33 });
            uplineIdsRule.SpecificationOperator = SpecificationOperator.NotIn;
            var param = Substitute.For<IParam>();
            param.MRecommendId.Returns(22);

            // Act
            var actualIsSatisfiedBy = uplineIdsRule.IsSatisfiedBy(param);

            // Assert
            Assert.IsFalse(actualIsSatisfiedBy);
        }

        [Test]
        public void IsSatisfiedBy_OperatorIsNotIn_SuperIsValid_ReturnFalse()
        {
            // Arrange
            var uplineIdsRule = new UplineIdsSpecification(new List<int> { 22, 33 });
            uplineIdsRule.SpecificationOperator = SpecificationOperator.NotIn;
            var param = Substitute.For<IParam>();
            param.SRecommendId.Returns(33);

            // Act
            var actualIsSatisfiedBy = uplineIdsRule.IsSatisfiedBy(param);

            // Assert
            Assert.IsFalse(actualIsSatisfiedBy);
        }


        [Test]
        public void IsSatisfiedBy_OperatorIsNotIn_MasterIsInvalid_ReturnTrue()
        {
            // Arrange
            var uplineIdsRule = new UplineIdsSpecification(new List<int> { 22, 33 });
            uplineIdsRule.SpecificationOperator = SpecificationOperator.NotIn;
            var param = Substitute.For<IParam>();
            param.MRecommendId.Returns(5);

            // Act
            var actualIsSatisfiedBy = uplineIdsRule.IsSatisfiedBy(param);

            // Assert
            Assert.IsTrue(actualIsSatisfiedBy);
        }

        [Test]
        public void IsSatisfiedBy_OperatorIsNotIn_SuperIsInvalid_ReturnTrue()
        {
            // Arrange
            var uplineIdsRule = new UplineIdsSpecification(new List<int> { 22, 33 });
            uplineIdsRule.SpecificationOperator = SpecificationOperator.NotIn;
            var param = Substitute.For<IParam>();
            param.SRecommendId.Returns(100);

            // Act
            var actualIsSatisfiedBy = uplineIdsRule.IsSatisfiedBy(param);

            // Assert
            Assert.IsTrue(actualIsSatisfiedBy);
        }
    }
}