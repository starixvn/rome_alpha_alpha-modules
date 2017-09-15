using Fanex.BetList.Core.Entities;
using Fanex.BetListDetail.ColossusBets.Api;
using Fanex.BetListDetail.ColossusBets.Configurations;
using Fanex.BetListDetail.ColossusBets.Criteria;
using Fanex.BetListDetail.ColossusBets.DataAccesses;
using Fanex.BetListDetail.ColossusBets.Services;
using NSubstitute;
using NUnit.Framework;
using Ploeh.AutoFixture;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fanex.BetListDetail.ColossusBets.Tests.Services
{
    public class ColossusBetsServiceTests
    {
        private ColossusBetsService service;
        private ColossusBetsRepository repository;
        private ColossusBetsWebClient webClient;

        private Fixture fixture;
        [SetUp]
        public void SetUp()
        {
            webClient = Substitute.For<ColossusBetsWebClient>();
            repository = Substitute.For<ColossusBetsRepository>();
            service = new ColossusBetsServiceImpl(repository, webClient);
            AppSettings.ColossusBetsConfig = new ColossusBets.Configurations.ColossusBets();
            fixture = new Fixture();
            FixtureData.Init();
        }

        [Test]
        public void GetBetListDetail_Always_CallRepository()
        {
            //// Arrange
            repository.BetDetails(Arg.Any<GetColossusBetsBetDetailsParameter>()).Returns(fixture.CreateMany<Ticket>());
            webClient.DownloadString(Arg.Any<Uri>()).Returns("{'data':{'Authorization':'testing1'},'errorCode':6,'errorMsg':'No permission'}");
            repository.GetSiteIdByCustomerId(Arg.Any<int>()).Returns("123");
            var parameter = new GetColossusBetsBetDetailsParameterImpl { RoleId = 4, CustId = 234, AccId = 343 };

            //// Act
            service.GetBetListDetail(parameter);

            //// Assert
            repository.Received().BetDetails(Arg.Any<GetColossusBetsBetDetailsParameter>());
            repository.Received().GetSiteIdByCustomerId(Arg.Any<int>());
            webClient.Received().DownloadString(Arg.Any<Uri>());
        }

        [Test]
        public void GetBetListDetail_Always_ReturnExpectedViewModel()
        {
            //// Arrange
            repository.BetDetails(Arg.Any<GetColossusBetsBetDetailsParameter>()).Returns(fixture.CreateMany<Ticket>());
            webClient.DownloadString(Arg.Any<Uri>()).Returns("{'data':{'Authorization':'testing2'},'errorCode':0,'errorMsg':''}");
            repository.GetSiteIdByCustomerId(Arg.Any<int>()).Returns("37");
            var parameter = new GetColossusBetsBetDetailsParameterImpl { RoleId = 3, CustId = 234, AccId = 343 };

            //// Act
            var act = service.GetBetListDetail(parameter);

            //// Assert
            Assert.AreEqual(act.MemberChoice.Iframe, "http://spondemo.staging-colossusbets.com/api/tickets/ref/0/selection?productToken=testing2&amp;siteID=3200100");
        }
    }
}
