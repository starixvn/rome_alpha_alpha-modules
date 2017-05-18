using Fanex.BetListDetail.Lucky3.Criteria;
using Fanex.BetListDetail.Lucky3.DataAccesses;
using Fanex.BetListDetail.Lucky3.Services;
using NSubstitute;
using NUnit.Framework;
using System.Linq;

namespace Fanex.BetListDetail.Lucky3.Tests.Services
{
    [TestFixture]
    public class Lucky3ServiceTests
    {
        private ILucky3DataAccess repository;
        private ILucky3Service service;
        [SetUp]
        public void SetUp()
        {
            repository = Substitute.For<ILucky3DataAccess>();
            service = new Lucky3Service(repository);
        }

        [Test]
        public void GetBetListDetail_InvalidParameter_ReturnEmptyList()
        {
            //// Arrange                        
            var parameter = Substitute.For<GetLucky3DetailParameter>();
            parameter.IsValid().Returns(false);

            //// Act
            var act = service.GetBetListDetail(parameter);            

            //// Assert
            Assert.AreEqual(0, act.Count());
        }

        [Test]
        public void GetBetListDetail_ValidParameter_CallDataAccess()
        {
            //// Arrange                        
            var parameter = Substitute.For<GetLucky3DetailParameter>();
            parameter.IsValid().Returns(true);

            //// Act
            service.GetBetListDetail(parameter);

            //// Assert
            repository.Received().GetLucky3Detail(parameter);
        }
    }
}
