namespace Alpha.Announcements.Tests.Services
{
    using Announcements.Services;
    using NUnit.Framework;
    using System.Linq;

    [TestFixture]
    public class SpecificationFactoryTests
    {
        private SpecificationFactoryImpl specificationFactoryImpl;

        [SetUp]
        public void SetUp()
        {
            specificationFactoryImpl = new SpecificationFactoryImpl();
        }

        [Test]
        public void LoadSpecifications_Always_ReturnCombinableSpecifications()
        {
            // Act
            var actualSpecifications = specificationFactoryImpl.LoadSpecifications();

            // Assert
            Assert.IsTrue(actualSpecifications.Any(spec => spec.Id == "6edc73e0-aa87-4757-96e7-e5ffea0ce06a")
                && actualSpecifications.Any(spec => spec.Id == "e6cacf4d-37d8-4031-a17c-8b02600d4c3f"));
        }
    }
}