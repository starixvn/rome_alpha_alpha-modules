namespace Alpha.SecurityCode.Tests.Services
{
    using System.Collections.Generic;
    using System.Linq;
    using Configurations;
    using NSubstitute;
    using NUnit.Framework;
    using SecurityCode.Services;
    using Web;

    [TestFixture]
    public class SecurityCodeCheckerTests
    {
        private SecurityCodeChecker securityCodeChecker;

        [SetUp]
        public void SetUp()
        {
            AppSettings.SecurityCodeConfig = new SecurityCodeConfig();
            SecurityCodeSessionManager.SecurityCodeSession = Substitute.For<SecurityCodeSession>();
            securityCodeChecker = new SecurityCodeCheckerImpl();
        }

        #region Is Required Security Code Test cases

        [Test]
        public void IsRequiredSecurityCode_SecurityCodeCheckingIsInActive_ReturnFalse()
        {
            //// Arrange
            AppSettings.SecurityCodeConfig.IsActive = false;

            //// Act
            var isRequired = securityCodeChecker.IsRequiredSecurityCode("/relative-path");

            //// Assert
            Assert.IsFalse(isRequired);
        }

        [Test]
        public void IsRequiredSecurityCode_UserSiteIsNotRequiredToCheckSecurityCode_ReturnFalse()
        {
            //// Arrange
            AppSettings.SecurityCodeConfig = new SecurityCodeConfig
            {
                IsActive = true,
                Sites = new List<string>
                {
                    "Site1",
                    "Site2"
                }
            };

            SecurityCodeSessionManager.SecurityCodeSession.UserSite.Returns("Site3");

            //// Act
            var isRequired = securityCodeChecker.IsRequiredSecurityCode("/relative-path");

            //// Assert
            Assert.IsFalse(isRequired);
        }

        [Test]
        public void IsRequiredSecurityCode_UserAlreadyInputSecurityCode_ReturnFalse()
        {
            AppSettings.SecurityCodeConfig = new SecurityCodeConfig
            {
                IsActive = true,
                Sites = new List<string>
                {
                    "Site1"
                }
            };

            SecurityCodeSessionManager.SecurityCodeSession.UserSite.Returns("Site3");
            SecurityCodeSessionManager.SecurityCodeSession.IsAlreadyInputSecurityCode.Returns(true);

            //// Act
            var isRequired = securityCodeChecker.IsRequiredSecurityCode("/relative-path");

            //// Assert
            Assert.IsFalse(isRequired);
        }

        [Test]
        public void IsRequiredSecurityCode_CurrentRelativePathIsNotRequiredToCheck_ReturnFalse()
        {
            AppSettings.SecurityCodeConfig = new SecurityCodeConfig
            {
                IsActive = true,
                Sites = new List<string>
                {
                    "Site1"
                },
                Paths = new List<string>
                {
                    "/relative-path1",
                    "/relative-path2"
                }
            };

            SecurityCodeSessionManager.SecurityCodeSession.UserSite.Returns("Site1");
            SecurityCodeSessionManager.SecurityCodeSession.IsAlreadyInputSecurityCode.Returns(false);
            var currentRelativePath = "/relative-path3";

            //// Act
            var isRequired = securityCodeChecker.IsRequiredSecurityCode(currentRelativePath);

            //// Assert
            Assert.IsFalse(isRequired);
        }

        [Test]
        public void IsRequiredSecurityCode_CurrentRelativePathIsRequiredToCheck_ReturnTrue()
        {
            AppSettings.SecurityCodeConfig = new SecurityCodeConfig
            {
                IsActive = true,
                Sites = new List<string>
                {
                    "Site1"
                },
                Paths = new List<string>
                {
                    "/relative-path1",
                    "/relative-path2"
                }
            };

            var currentRelativePath = "/relative-path1";

            SecurityCodeSessionManager.SecurityCodeSession.UserSite.Returns("site1");
            SecurityCodeSessionManager.SecurityCodeSession.IsAlreadyInputSecurityCode.Returns(false);
            SecurityCodeSessionManager.SecurityCodeSession.SecurityCode.Returns("securitycode");

            //// Act
            var isRequired = securityCodeChecker.IsRequiredSecurityCode(currentRelativePath);

            //// Assert
            Assert.IsTrue(isRequired);
        }

        [Test]
        public void IsRequiredSecurityCode_SiteIsNotInConfigAndPathIsRequiredSecurityCode_ReturnTrue()
        {
            AppSettings.SecurityCodeConfig = new SecurityCodeConfig
            {
                IsActive = true,
                Sites = Enumerable.Empty<string>(),
                Paths = new List<string>
                {
                    "/relative-path1",
                    "/relative-path2"
                }
            };

            var currentRelativePath = "/relative-path1";

            SecurityCodeSessionManager.SecurityCodeSession.IsAlreadyInputSecurityCode.Returns(false);
            SecurityCodeSessionManager.SecurityCodeSession.SecurityCode.Returns("securitycode");

            //// Act
            var isRequired = securityCodeChecker.IsRequiredSecurityCode(currentRelativePath);

            //// Assert
            Assert.IsTrue(isRequired);
        }

        [Test]
        public void IsRequiredSecurityCode_AdminInternalLogin_ReturnFalse()
        {
            AppSettings.SecurityCodeConfig = new SecurityCodeConfig
            {
                IsActive = true,
                Sites = Enumerable.Empty<string>(),
                Paths = new List<string>
                {
                    "/relative-path1",
                    "/relative-path2"
                }
            };

            var currentRelativePath = "/relative-path1";

            SecurityCodeSessionManager.SecurityCodeSession.IsInternal.Returns(true);

            //// Act
            var isRequired = securityCodeChecker.IsRequiredSecurityCode(currentRelativePath);

            //// Assert
            Assert.IsFalse(isRequired);
        }

        [Test]
        public void IsRequiredSecurityCode_UserIsBeingForcedChangeSecurityCode_ReturnTrue()
        {
            AppSettings.SecurityCodeConfig = new SecurityCodeConfig
            {
                IsActive = true,
                Sites = Enumerable.Empty<string>(),
                Paths = new List<string>
                {
                    "/relative-path1",
                    "/relative-path2"
                }
            };

            var currentRelativePath = "/relative-path1";

            SecurityCodeSessionManager.SecurityCodeSession.IsAlreadyInputSecurityCode.Returns(false);
            SecurityCodeSessionManager.SecurityCodeSession.SecurityCode.Returns("securitycode");
            SecurityCodeSessionManager.SecurityCodeSession.IsInternal.Returns(false);
            SecurityCodeSessionManager.SecurityCodeSession.IsForcedChangeSecurityCode.Returns(true);

            //// Act
            var isRequired = securityCodeChecker.IsRequiredSecurityCode(currentRelativePath);

            //// Assert
            Assert.IsTrue(isRequired);
        }

        [Test]
        public void IsRequiredSecurityCode_SiteIsOfferedSCButUserDoesNotHaveSC_ReturnFalse()
        {
            AppSettings.SecurityCodeConfig = new SecurityCodeConfig
            {
                IsActive = true,
                Sites = Enumerable.Empty<string>(),
                Paths = new List<string>
                {
                    "/relative-path1",
                    "/relative-path2"
                }
            };

            var currentRelativePath = "/relative-path1";

            SecurityCodeSessionManager.SecurityCodeSession.IsAlreadyInputSecurityCode.Returns(false);
            SecurityCodeSessionManager.SecurityCodeSession.SecurityCode.Returns(string.Empty);
            SecurityCodeSessionManager.SecurityCodeSession.IsInternal.Returns(false);

            //// Act
            var isRequired = securityCodeChecker.IsRequiredSecurityCode(currentRelativePath);

            //// Assert
            Assert.IsFalse(isRequired);
        }

        [Test]
        public void IsRequiredSecurityCode_SiteIsNotOfferedSC_ReturnFalse()
        {
            AppSettings.SecurityCodeConfig = new SecurityCodeConfig
            {
                IsActive = true,
                Sites = Enumerable.Empty<string>(),
                Paths = new List<string>
                {
                    "/relative-path1",
                    "/relative-path2"
                }
            };

            var currentRelativePath = "/relative-path3";

            SecurityCodeSessionManager.SecurityCodeSession.IsAlreadyInputSecurityCode.Returns(false);
            SecurityCodeSessionManager.SecurityCodeSession.SecurityCode.Returns("SecurityCode");
            SecurityCodeSessionManager.SecurityCodeSession.IsInternal.Returns(false);

            //// Act
            var isRequired = securityCodeChecker.IsRequiredSecurityCode(currentRelativePath);

            //// Assert
            Assert.IsFalse(isRequired);
        }

        #endregion Is Required Security Code Test cases

        #region Is Forced Change Security Code Test cases

        [Test]
        public void IsForcedChangeSecurityCode_AdminInternalLogin_ReturnFalse()
        {
            //// Arrange
            SecurityCodeSessionManager.SecurityCodeSession.IsInternal.Returns(true);

            //// Act
            var isForced = securityCodeChecker.IsForcedChangeSecurityCode();

            //// Assert

            Assert.IsFalse(isForced);
        }

        [Test]
        public void IsForcedChangeSecurityCode_UserAreBeingForcedChange_ReturnTrue()
        {
            //// Arrange
            SecurityCodeSessionManager.SecurityCodeSession.IsInternal.Returns(false);
            SecurityCodeSessionManager.SecurityCodeSession.IsForcedChangeSecurityCode.Returns(true);

            //// Act
            var isForced = securityCodeChecker.IsForcedChangeSecurityCode();

            //// Assert

            Assert.IsTrue(isForced);
        }

        #endregion Is Forced Change Security Code Test cases

        #region Is Enable Security Code Test cases

        [Test]
        public void IsEnableSecurityCode_SecurityCodeCheckingIsInActive_ReturnFalse()
        {
            //// Arrange
            AppSettings.SecurityCodeConfig.IsActive = false;

            //// Act
            var isEnbabled = securityCodeChecker.IsEnableSecurityCode();

            //// Assert
            Assert.IsFalse(isEnbabled);
        }

        [Test]
        public void IsEnableSecurityCode_UserAreBeingForcedChange_ReturnTrue()
        {
            //// Arrange
            AppSettings.SecurityCodeConfig.IsActive = true;
            SecurityCodeSessionManager.SecurityCodeSession.IsForcedChangeSecurityCode.Returns(true);

            //// Act
            var isEnbabled = securityCodeChecker.IsEnableSecurityCode();

            //// Assert
            Assert.True(isEnbabled);
        }

        [Test]
        public void IsEnableSecurityCode_UserSiteIsNotOfferedSC_ReturnFalse()
        {
            AppSettings.SecurityCodeConfig = new SecurityCodeConfig
            {
                IsActive = true,
                Sites = new List<string>
                {
                    "Site1"
                }
            };

            SecurityCodeSessionManager.SecurityCodeSession.UserSite.Returns("Site3");

            //// Act
            var isEnbabled = securityCodeChecker.IsEnableSecurityCode();

            //// Assert
            Assert.IsFalse(isEnbabled);
        }

        [Test]
        public void IsEnableSecurityCode_UserDoesNotHaveSecurityCode_ReturnFalse()
        {
            AppSettings.SecurityCodeConfig = new SecurityCodeConfig
            {
                IsActive = true,
                Sites = new List<string>
                {
                    "Site1"
                }
            };

            SecurityCodeSessionManager.SecurityCodeSession.UserSite.Returns("Site1");

            //// Act
            var isEnbabled = securityCodeChecker.IsEnableSecurityCode();

            //// Assert
            Assert.IsFalse(isEnbabled);
        }

        #endregion Is Enable Security Code Test cases
    }
}