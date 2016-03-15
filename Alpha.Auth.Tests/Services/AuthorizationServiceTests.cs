namespace Alpha.Auth.Tests.Services
{
    using System;
    using System.Configuration;
    using Auth.Services;
    using Auth.Services.Impl;
    using Configurations;
    using DataAccesses;
    using Models;
    using NSubstitute;
    using NUnit.Framework;

    [TestFixture]
    public class AuthorizationServiceTests
    {
        private IAuthorizationService _authorizationService;
        private IAuthorizationDataAccess _authorizationDataAccess;
        private IRequestContext _requestContext;
        private IStatisticService _statisticService;
        private ICachingService _cachingService;
        private AuthorizationSection _authorizationSection;

        private AuthorizationSection ReadConfigurations()
        {
            ExeConfigurationFileMap fileMap = new ExeConfigurationFileMap { ExeConfigFilename = "AuthorizationSection.config" };
            Configuration config = ConfigurationManager.OpenMappedExeConfiguration(fileMap, ConfigurationUserLevel.None);

            return config.GetSection("AuthorizationSection") as AuthorizationSection;
        }

        [SetUp]
        public void SetUp()
        {
            _authorizationDataAccess = Substitute.For<IAuthorizationDataAccess>();
            _statisticService = Substitute.For<IStatisticService>();
            _requestContext = Substitute.For<IRequestContext>();
            _cachingService = Substitute.For<ICachingService>();
            _authorizationSection = ReadConfigurations();

            _authorizationService = new AuthorizationService(
                _statisticService,
                _requestContext,
                _cachingService,
                _authorizationDataAccess,
                _authorizationSection);
        }

        [Test]
        public void IsAccountActive_ExecuteGetOrAdd()
        {
            //// Arrange
            _requestContext.UserSession.CustId.Returns(1);
            _requestContext.UserSession.SubAccId.Returns(2);

            //// Act
            _authorizationService.IsAccountActive();

            //// Assert
            _cachingService.Received().GetOrAdd<bool>(
                "Alpha.Auth.CustContext.IsCustomerActiveCacheKey_1_2",
                Arg.Any<Func<bool>>(),
                50);
        }

        [Test]
        public void IsAccountActive_ReturnCachedData()
        {
            //// Arrange
            _cachingService.GetOrAdd<bool>(
                Arg.Any<string>(),
                Arg.Any<Func<bool>>(),
                Arg.Any<int>()).Returns(true);

            //// Act
            var isActive = _authorizationService.IsAccountActive();

            //// Assert
            Assert.IsTrue(isActive);
        }

        [Test]
        public void CheckBelonging_UrlDoesNotExist_ReturnTrue()
        {
            //// Arrange
            _requestContext.Url.Returns("aaaaaa");

            //// Act
            var isBelong = _authorizationService.IsBelonging();

            //// Assert
            Assert.IsTrue(isBelong);
        }

        [Test]
        public void CheckBelonging_UrlDoesNotNeedCheckBelonging_ReturnTrue()
        {
            //// Arrange
            _requestContext.Url.Returns("_MemberInfo/SearchAndCopy/DoNotCheckBelonging.aspx");

            //// Act
            var isBelong = _authorizationService.IsBelonging();

            //// Assert
            Assert.IsTrue(isBelong);
        }

        [Test]
        public void CheckBelonging_UrlNeedBeCheckedBelonging_ExecuteIsBelonging()
        {
            //// Arrange
            _requestContext.Url.Returns("_MemberInfo/SearchAndCopy/Header.aspx");
            _requestContext.CustId.Returns(100);
            _requestContext.TransId.Returns(2222);
            _requestContext.UserSession.RoleId.Returns((byte)3);
            _requestContext.UserSession.CustId.Returns(1);
            _requestContext.UserSession.IsInternal.Returns(true);
            _requestContext.UserSession.SubAccId.Returns(2);

            //// Act
            var isBelong = _authorizationService.IsBelonging();

            //// Assert
            _authorizationDataAccess.Received().IsBelonging(100, 1, 2, 2222, true);
        }
    }
}