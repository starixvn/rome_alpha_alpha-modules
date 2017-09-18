namespace Fanex.BetListDetail.Casino.Tests.Services
{
    using System.Threading.Tasks;
    using Casino.Services;
    using DataAccesses;
    using NSubstitute;
    using NUnit.Framework;
    using Params;

    [TestFixture]
    public class CasinoDetailServiceTests
    {
        private ICasinoDetailDataAccess casinoDetailDataAccess;
        private CasinoDetailService casinoDetailService;

        [SetUp]
        public void SetUp()
        {
            casinoDetailDataAccess = Substitute.For<ICasinoDetailDataAccess>();
            casinoDetailService = new CasinoDetailService(casinoDetailDataAccess);
        }

        [Test]
        public async Task GetCasinoDetail_Always_CallGetCasinoDetailFromDataAccess()
        {
            // Arrange
            var param = new CasinoDetailParam(1, 1, 1, 1, 1, 100);

            // Act
            await casinoDetailService.GetCasinoDetail(param);

            // Assert
            await casinoDetailDataAccess.Received().GetCasinoDetail(
                Arg.Is<CasinoDetailParam>(
                    p => p.RefNo == param.RefNo
                    && p.CustId == param.CustId
                    && p.TransType == param.TransType
                    && p.ProductId == param.ProductId
                    && p.PageIndex == param.PageIndex
                    && p.PageSize == param.PageSize));
        }
    }
}
