namespace Alpha.Announcements.Tests.Specs
{
    using Announcements.Specs;
    using Models;
    using NSubstitute;
    using NUnit.Framework;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;

    [TestFixture]
    public class SiteSpecificationTests
    {
        [Test]
        public void IsSatisfiedBy_UserIsAllowedToViewAnnouncementBySite_ReturnTrue()
        {
            // Arrange
            var siteSpec = new SiteSpecification(new List<string> { "Site1", "Site2" });
            siteSpec.SpecificationOperator = SpecificationOperator.In;
            var user = Substitute.For<IUser>();
            user.Site.Returns("Site1");

            // Act
            var actualIsSatisfiedBy = siteSpec.IsSatisfiedBy(user);

            // Assert
            Assert.IsTrue(actualIsSatisfiedBy);
        }

        [Test]
        public void IsSatisfiedBy_UserIsNotAllowedToViewAnnouncementBySite_ReturnFalse()
        {
            // Arrange
            var siteSpec = new SiteSpecification(new List<string> { "Site1", "Site2" });
            siteSpec.SpecificationOperator = SpecificationOperator.In;
            var user = Substitute.For<IUser>();
            user.Site.Returns("Site3");

            // Act
            var actualIsSatisfiedBy = siteSpec.IsSatisfiedBy(user);

            // Assert
            Assert.IsFalse(actualIsSatisfiedBy);
        }

        [TestCase("1")]
        [TestCase("1343434")]
        public void IsSatisfiedBy_OperatorIsNotIn_UserSiteIsNotInList_ReturnTrue(string userSite)
        {
            // Arrange
            var siteSpec = new SiteSpecification(new List<string> { "Site1", "Site2" });
            siteSpec.SpecificationOperator = SpecificationOperator.NotIn;
            var user = Substitute.For<IUser>();
            user.Site.Returns(userSite);

            // Act
            var actualIsSatisfiedBy = siteSpec.IsSatisfiedBy(user);

            // Assert
            Assert.IsTrue(actualIsSatisfiedBy);
        }

        [TestCase("Site1")]
        [TestCase("Site2")]
        public void IsSatisfiedBy_OperatorIsNotIn_UserSiteIsInList_ReturnFalse(string userSite)
        {
            // Arrange
            var siteSpec = new SiteSpecification(new List<string> { "Site1", "Site2" });
            siteSpec.SpecificationOperator = SpecificationOperator.NotIn;
            var user = Substitute.For<IUser>();
            user.Site.Returns(userSite);

            // Act
            var actualIsSatisfiedBy = siteSpec.IsSatisfiedBy(user);

            // Assert
            Assert.IsFalse(actualIsSatisfiedBy);
        }
    }
}