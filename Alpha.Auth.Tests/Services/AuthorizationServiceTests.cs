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
    using Sunplus.Agent.Model.Entities;
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

        #region Check Cust Status

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

        #endregion Check Cust Status

        #region Check Belonging

        [Test]
        public void IsBelonging_UrlDoesNotExist_ReturnTrue()
        {
            //// Arrange
            _requestContext.Url.Returns("aaaaaa");

            //// Act
            var isBelong = _authorizationService.IsBelonging();

            //// Assert
            Assert.IsTrue(isBelong);
        }

        [Test]
        public void IsBelonging_UrlDoesNotNeedCheckBelonging_ReturnTrue()
        {
            //// Arrange
            _requestContext.Url.Returns("_MemberInfo/SearchAndCopy/DoNotCheckBelonging.aspx");

            //// Act
            var isBelong = _authorizationService.IsBelonging();

            //// Assert
            Assert.IsTrue(isBelong);
        }

        [Test]
        public void IsBelonging_UrlNeedBeCheckedBelonging_ExecuteIsBelonging()
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
            _authorizationService.IsBelonging();

            //// Assert
            _authorizationDataAccess.Received().IsBelonging(100, 1, 2, 2222, true);
        }

        [TestCase(true)]
        [TestCase(false)]
        public void IsBelonging_UrlNeedBeCheckedBelonging_ReturnIsBelongingFromDatabase(bool expectedIsBelonging)
        {
            //// Arrange
            _requestContext.Url.Returns("_MemberInfo/SearchAndCopy/Header.aspx");
            _requestContext.CustId.Returns(100);
            _requestContext.TransId.Returns(2222);
            _requestContext.UserSession.RoleId.Returns((byte)3);
            _requestContext.UserSession.CustId.Returns(1);
            _requestContext.UserSession.IsInternal.Returns(true);
            _requestContext.UserSession.SubAccId.Returns(2);
            _authorizationDataAccess
                .IsBelonging(Arg.Any<int>(), Arg.Any<int>(), Arg.Any<int>(), Arg.Any<long>(), Arg.Any<bool>())
                .Returns(expectedIsBelonging);

            //// Act
            var isBelong = _authorizationService.IsBelonging();

            ////
            Assert.AreEqual(expectedIsBelonging, isBelong);
        }

        [Test]
        public void IsBelonging_UrlNeedBeCheckedViewByGroupHeadLine_ExecuteIsCustomerConsolidatedGroupBelonging()
        {
            //// Arrange
            _requestContext.Url.Returns("_MemberInfo/GroupHeadLine.aspx");
            _requestContext.CustId.Returns(100);
            _requestContext.TransId.Returns(2222);
            _requestContext.UserSession.RoleId.Returns((byte)3);
            _requestContext.UserSession.CustId.Returns(1);
            _requestContext.UserSession.IsInternal.Returns(true);
            _requestContext.UserSession.SubAccId.Returns(2);

            //// Act
            _authorizationService.IsBelonging();

            //// Assert
            _authorizationDataAccess.Received().IsCustomerConsolidatedGroupBelonging(100, 1, 2, 2222, true);
        }

        [Test]
        public void IsBelonging_UserCanViewGroupHeadline_ReturnTrue()
        {
            //// Arrange
            _requestContext.Url.Returns("_MemberInfo/GroupHeadLine.aspx");
            _requestContext.CustId.Returns(100);
            _requestContext.TransId.Returns(2222);
            _requestContext.UserSession.RoleId.Returns((byte)3);
            _requestContext.UserSession.CustId.Returns(1);
            _requestContext.UserSession.IsInternal.Returns(true);
            _requestContext.UserSession.SubAccId.Returns(2);
            _authorizationDataAccess
                .IsCustomerConsolidatedGroupBelonging(100, 1, 2, 2222, true)
                .Returns(true);

            //// Act
            var isBelonging = _authorizationService.IsBelonging();

            //// Assert
            Assert.IsTrue(isBelonging);
        }

        [Test]
        public void IsBelonging_ViewByGroupHeadLineAndIsNotBelonging_ReturnFalse()
        {
            //// Arrange
            _requestContext.Url.Returns("_MemberInfo/GroupHeadLine.aspx");
            _requestContext.CustId.Returns(100);
            _requestContext.TransId.Returns(2222);
            _requestContext.UserSession.RoleId.Returns((byte)3);
            _requestContext.UserSession.CustId.Returns(1);
            _requestContext.UserSession.IsInternal.Returns(true);
            _requestContext.UserSession.SubAccId.Returns(2);
            _authorizationDataAccess
                .IsBelonging(Arg.Any<int>(), Arg.Any<int>(), Arg.Any<int>(), Arg.Any<long>(), Arg.Any<bool>())
                .Returns(false);

            //// Act
            var isBelonging = _authorizationService.IsBelonging();

            //// Assert
            Assert.IsFalse(isBelonging);
        }

        [Test]
        public void IsBelonging_UrlNeedBeCheckedOnDatawareHouseDatabase_ReturnExecuteIsBelongingOld()
        {
            //// Arrange
            _requestContext.Url.Returns("_MemberInfo/SearchAndCopy/Header.aspx");
            _requestContext.CustId.Returns(100);
            _requestContext.TransId.Returns(2222);
            _requestContext.IsDataStoreInDataWarehouse.Returns(true);
            _requestContext.UserSession.RoleId.Returns((byte)3);
            _requestContext.UserSession.CustId.Returns(1);
            _requestContext.UserSession.IsInternal.Returns(true);
            _requestContext.UserSession.SubAccId.Returns(2);

            //// Act
            _authorizationService.IsBelonging();

            //// Assert
            _authorizationDataAccess.Received().IsBelongingOld(100, 1, 2222);
            _authorizationDataAccess.DidNotReceive().IsBelonging(100, 1, 2, 2222, true);
        }

        [Test]
        public void IsBelonging_UrlOfLowerLevelNeedBeCheckedForHigherLevelAlso()
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
            _authorizationService.IsBelonging();

            //// Assert
            _authorizationDataAccess.Received().IsBelonging(100, 1, 2, 2222, true);
        }

        [Test]
        public void IsBelonging_UrlIsNotAllowedCurrentLoginUserToAccess_ReturnFalse()
        {
            //// Arrange
            _requestContext.Url.Returns("IsNotAllowByThemselve.aspx");
            _requestContext.CustId.Returns(100);
            _requestContext.UserSession.RoleId.Returns((byte)3);
            _requestContext.UserSession.CustId.Returns(100);

            //// Act
            var isBelonging = _authorizationService.IsBelonging();

            //// Assert
            Assert.IsFalse(isBelonging);
        }

        [Test]
        public void IsBelonging_UrlIsAllowedCurrentLoginUserToAccess_ReturnTrue()
        {
            //// Arrange
            _requestContext.Url.Returns("IsAllowByThemselve.aspx");
            _requestContext.CustId.Returns(100);
            _requestContext.UserSession.RoleId.Returns((byte)3);
            _requestContext.UserSession.CustId.Returns(100);

            //// Act
            var isBelonging = _authorizationService.IsBelonging();

            //// Assert
            Assert.IsTrue(isBelonging);
        }

        #endregion Check Belonging

        #region Check Authorized

        ////[Test]
        ////public void IsAuthorized_CurrentAccIsSubAccount_ExecuteGetOrAddSubPermission()
        ////{
        ////    //// Arrange
        ////    var url = "IsAllowByThemselve.aspx";
        ////    _requestContext.CustId.Returns(103);
        ////    _requestContext.UserSession.RoleId.Returns((byte)3);
        ////    _requestContext.UserSession.CustId.Returns(100);
        ////    _requestContext.UserSession.SubAccId.Returns(101);
        ////    _requestContext.UserSession.SubPageAccess.Returns(new PageAccess
        ////    {
        ////        CreateAccount = true
        ////    });

        ////    //// Act
        ////    _authorizationService.IsAuthorized(url);

        ////    //// Assert
        ////    _cachingService.Received().GetOrAdd(
        ////        "Alpha.Auth.CustContext.SubAccountPermissionCacheKey_100_101",
        ////        Arg.Any<Func<string>>(),
        ////        50);
        ////}

        [Test]
        public void IsAuthorized_SubAccountHasFullPermission_ReturnTrue()
        {
            //// Arrange
            var url = "IsAllowByThemselve.aspx";
            _requestContext.CustId.Returns(103);
            _requestContext.UserSession.RoleId.Returns((byte)3);
            _requestContext.UserSession.CustId.Returns(100);
            _requestContext.UserSession.SubAccId.Returns(101);
            _requestContext.UserSession.SubPageAccess.Returns(new PageAccess
            {
                CreateAccount = true,
                Transfer = true,
                UpdateAccount = true,
                ViewLog = true,
                ViewMemberInfo = true,
                ViewReports = true,
                ViewTotalBets = true
            });

            //// Act
            var isAuthorized = _authorizationService.IsAuthorized(url);

            //// Assert
            Assert.IsTrue(isAuthorized);
        }

        [Test]
        public void IsAuthorized_SubAccountHasPermissionOnPage_ReturnTrue()
        {
            //// Arrange
            var url = "IsAllowByThemselve.aspx";
            _requestContext.CustId.Returns(103);
            _requestContext.UserSession.RoleId.Returns((byte)3);
            _requestContext.UserSession.CustId.Returns(100);
            _requestContext.UserSession.SubAccId.Returns(101);
            var fullPermission = "[A]";
            _requestContext.UserSession.SubPageAccess.Returns(new PageAccess {
                CreateAccount = true
            });

            _cachingService.GetOrAdd(
                Arg.Any<string>(),
                Arg.Any<Func<string>>(),
                Arg.Any<int>()).Returns(fullPermission);

            //// Act
            var isAuthorized = _authorizationService.IsAuthorized(url);

            //// Assert
            Assert.IsTrue(isAuthorized);
        }

        [Test]
        public void IsAuthorized_SubAccountDoesNotHasPermissionOnPage_ReturnFalse()
        {
            //// Arrange
            var url = "IsAllowByThemselve.aspx";
            _requestContext.CustId.Returns(103);
            _requestContext.UserSession.RoleId.Returns((byte)3);
            _requestContext.UserSession.CustId.Returns(100);
            _requestContext.UserSession.SubAccId.Returns(101);
            _requestContext.UserSession.SubPageAccess.Returns(new PageAccess
            {
                CreateAccount = false
            });

            //// Act
            var isAuthorized = _authorizationService.IsAuthorized(url);

            //// Assert
            Assert.IsFalse(isAuthorized);
        }

        [Test]
        public void IsAuthorized_SubAccountHasPermissionOnPage_ExecuteUpdateFunctionUsage()
        {
            //// Arrange
            var url = "IsAllowByThemselve.aspx";
            _requestContext.CustId.Returns(103);
            _requestContext.UserSession.RoleId.Returns((byte)3);
            _requestContext.UserSession.CustId.Returns(100);
            _requestContext.UserSession.SubAccId.Returns(101);
            _requestContext.UserSession.Site.Returns("Demo Site");
            _requestContext.UserSession.SubPageAccess.Returns(new PageAccess
            {
                CreateAccount = true
            });

            //// Act
            _authorizationService.IsAuthorized(url);

            //// Assert
            _statisticService.Received().UpdateFunctionUsage(
                                url,
                                "Is Allow By Themselve",
                                "SC - Agent Info",
                                "Demo Site");
        }

        #endregion
    }
}