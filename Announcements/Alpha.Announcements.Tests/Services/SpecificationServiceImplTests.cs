namespace Alpha.Announcements.Tests.Services
{
    using Announcements.Services;
    using Announcements.Specs;
    using Models;
    using NSubstitute;
    using NUnit.Framework;
    using System.Collections.Generic;
    using System.Linq;

    [TestFixture]
    public class SpecificationServiceImplTests
    {
        private SpecificationService specificationService;
        private IUser user;

        [SetUp]
        public void SetUp()
        {
            specificationService = new SpecificationServiceImpl();
            user = Substitute.For<IUser>();
        }

        [Test]
        public void IsSatisfied_NoSpecificationRequired_ReturnTrue()
        {
            // Act
            var actualIsSatisfied = specificationService.IsSatisfied(Enumerable.Empty<ISpecification<IUser>>(), user);

            // Assert
            Assert.IsTrue(actualIsSatisfied);
        }

        [Test]
        public void IsSatisfied_UserIsNull_ReturnFalse()
        {
            // Act
            var actualIsSatisfied = specificationService.IsSatisfied(
                new List<ISpecification<IUser>>
                {
                    Substitute.For<ISpecification<IUser>>()
                },
                user);

            // Assert
            Assert.False(actualIsSatisfied);
        }

        [Test]
        public void IsSatisfied_ConjuntionIsAnd_AndOneSpecificationIsNotSatisfied_ReturnFalse()
        {
            // Arrange
            var spec1 = Substitute.For<ISpecification<IUser>>();
            spec1.ConjunctionOperator.Returns(ConjunctionOperator.And);
            spec1.IsSatisfiedBy(Arg.Any<IUser>()).Returns(false);

            var spec2 = Substitute.For<ISpecification<IUser>>();
            spec2.ConjunctionOperator.Returns(ConjunctionOperator.And);
            spec2.IsSatisfiedBy(Arg.Any<IUser>()).Returns(true);

            var specs = new List<ISpecification<IUser>>
                {
                    spec1,
                    spec2
                };

            // Act
            var actualIsSatisfied = specificationService.IsSatisfied(specs, user);

            // Assert
            Assert.IsFalse(actualIsSatisfied);
        }

        [Test]
        public void IsSatisfied_ConjuntionIsAnd_AllSpecsAreSatisfied_ReturnTrue()
        {
            // Arrange
            var spec1 = Substitute.ForPartsOf<CompositeSpecification<IUser>>();
            spec1.ConjunctionOperator = ConjunctionOperator.And;
            spec1.IsSatisfiedBy(Arg.Any<IUser>()).Returns(true);

            var spec2 = Substitute.ForPartsOf<CompositeSpecification<IUser>>();
            spec2.ConjunctionOperator = ConjunctionOperator.And;
            spec2.IsSatisfiedBy(Arg.Any<IUser>()).Returns(true);

            var specs = new List<ISpecification<IUser>>
                {
                    spec1,
                    spec2
                };

            // Act
            var actualIsSatisfied = specificationService.IsSatisfied(specs, user);

            // Assert
            Assert.IsTrue(actualIsSatisfied);
        }

        [Test]
        public void IsSatisfied_ConjuntionIsOr_AllSpecsAreSatisfied_ReturnTrue()
        {
            // Arrange
            var spec1 = Substitute.ForPartsOf<CompositeSpecification<IUser>>();
            spec1.ConjunctionOperator = ConjunctionOperator.Or;
            spec1.IsSatisfiedBy(Arg.Any<IUser>()).Returns(true);

            var spec2 = Substitute.ForPartsOf<CompositeSpecification<IUser>>();
            spec2.ConjunctionOperator = ConjunctionOperator.Or;
            spec2.IsSatisfiedBy(Arg.Any<IUser>()).Returns(true);

            var specs = new List<ISpecification<IUser>>
                {
                    spec1,
                    spec2
                };

            // Act
            var actualIsSatisfied = specificationService.IsSatisfied(specs, user);

            // Assert
            Assert.IsTrue(actualIsSatisfied);
        }

        [Test]
        public void IsSatisfied_ConjuntionIsOr_OneOfSpecIsSatisfied_ReturnTrue()
        {
            // Arrange
            var spec1 = Substitute.ForPartsOf<CompositeSpecification<IUser>>();
            spec1.ConjunctionOperator = ConjunctionOperator.Or;
            spec1.IsSatisfiedBy(Arg.Any<IUser>()).Returns(false);

            var spec2 = Substitute.ForPartsOf<CompositeSpecification<IUser>>();
            spec2.ConjunctionOperator = ConjunctionOperator.Or;
            spec2.IsSatisfiedBy(Arg.Any<IUser>()).Returns(true);

            var specs = new List<ISpecification<IUser>>
                {
                    spec1,
                    spec2
                };

            // Act
            var actualIsSatisfied = specificationService.IsSatisfied(specs, user);

            // Assert
            Assert.IsTrue(actualIsSatisfied);
        }

        [TestCase(true, true, false, false)]
        [TestCase(false, false, true, false)]
        [TestCase(false, true, true, true)]
        [TestCase(false, true, false, false)]
        public void IsSatisfied_MixConjuntion_OneOfSpecIsSatisfied_ReturnFalse(
            bool isSpec1Statisfied,
            bool isSpec2Statisfied,
            bool isSpec3Statisfied,
            bool expectedResult)
        {
            // Arrange
            var spec1 = Substitute.ForPartsOf<CompositeSpecification<IUser>>();
            spec1.IsSatisfiedBy(Arg.Any<IUser>()).Returns(isSpec1Statisfied);

            var spec2 = Substitute.ForPartsOf<CompositeSpecification<IUser>>();
            spec2.ConjunctionOperator = ConjunctionOperator.Or;
            spec2.IsSatisfiedBy(Arg.Any<IUser>()).Returns(isSpec2Statisfied);

            var spec3 = Substitute.ForPartsOf<CompositeSpecification<IUser>>();
            spec3.ConjunctionOperator = ConjunctionOperator.And;
            spec3.IsSatisfiedBy(Arg.Any<IUser>()).Returns(isSpec3Statisfied);

            var specs = new List<ISpecification<IUser>>
                {
                    spec1,
                    spec2,
                    spec3
                };

            // Act
            var actualIsSatisfied = specificationService.IsSatisfied(specs, user);

            // Assert
            Assert.AreEqual(actualIsSatisfied, expectedResult);
        }
    }
}