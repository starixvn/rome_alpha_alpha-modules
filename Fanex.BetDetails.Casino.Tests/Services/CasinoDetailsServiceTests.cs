namespace Fanex.BetDetails.Casino.Tests.Services
{
    using Casino.Services;
    using DataAccesses;
    using NSubstitute;
    using NUnit.Framework;
    using Params;

    [TestFixture]
    public class CasinoDetailsServiceTests
    {
        private ICasinoDetailsDataAccess casinoDetailDataAccess;
        private CasinoDetailsService casinoDetailService;

        [SetUp]
        public void SetUp()
        {
            casinoDetailDataAccess = Substitute.For<ICasinoDetailsDataAccess>();
            casinoDetailService = new CasinoDetailsService(casinoDetailDataAccess);
        }

        [Test]
        public void GetCasinoDetail_Always_CallGetCasinoDetailFromDataAccess()
        {
            // Arrange
            var param = new CasinoDetailsParam(1, 1, 1, 1, 1, 100);

            // Act
            casinoDetailService.GetCasinoDetails(param);

            // Assert
            casinoDetailDataAccess.Received().GetCasinoDetails(
                Arg.Is<CasinoDetailsParam>(
                    p => p.RefNo == param.RefNo
                    && p.CustId == param.CustId
                    && p.TransType == param.TransType
                    && p.ProductId == param.ProductId
                    && p.PageIndex == param.PageIndex
                    && p.PageSize == param.PageSize));
        }
    }
}
