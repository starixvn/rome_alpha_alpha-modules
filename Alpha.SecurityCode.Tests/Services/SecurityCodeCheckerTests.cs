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
        [SetUp]
        public void SetUp()
        {
            AppSettings.SecurityCodeConfig = new SecurityCodeConfig();
        }

        [Test]
        public void IsRequiredSecurityCode_SecurityCodeCheckingIsInActive_ReturnFalse()
        {
            //// Arrange
            AppSettings.SecurityCodeConfig.IsActive = false;

            //// Act
            var isRequired = SecurityCodeChecker.IsRequiredSecurityCode("/relative-path");

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

            SecurityCodeSessionManager.SecurityCodeSession = Substitute.For<ISecurityCodeSession>();
            SecurityCodeSessionManager.SecurityCodeSession.UserSite.Returns("Site3");

            //// Act
            var isRequired = SecurityCodeChecker.IsRequiredSecurityCode("/relative-path");

            //// Assert
            Assert.IsFalse(isRequired);
        }

        [Test]
        public void IsRequiredSecurityCode_UserAlreadyInputSecurityCode_ReturnTrue()
        {
            AppSettings.SecurityCodeConfig = new SecurityCodeConfig
            {
                IsActive = true,
                Sites = new List<string>
                {
                    "Site1"
                }
            };

            SecurityCodeSessionManager.SecurityCodeSession = Substitute.For<ISecurityCodeSession>();
            SecurityCodeSessionManager.SecurityCodeSession.UserSite.Returns("Site3");
            SecurityCodeSessionManager.SecurityCodeSession.IsAlreadyInputSecurityCode.Returns(true);

            //// Act
            var isRequired = SecurityCodeChecker.IsRequiredSecurityCode("/relative-path");

            //// Assert
            Assert.IsTrue(isRequired);
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
                Urls = new List<string>
                {
                    "/relative-path1",
                    "/relative-path2"
                }
            };

            SecurityCodeSessionManager.SecurityCodeSession = Substitute.For<ISecurityCodeSession>();
            SecurityCodeSessionManager.SecurityCodeSession.UserSite.Returns("Site1");
            SecurityCodeSessionManager.SecurityCodeSession.IsAlreadyInputSecurityCode.Returns(false);
            var currentRelativePath = "/relative-path3";

            //// Act
            var isRequired = SecurityCodeChecker.IsRequiredSecurityCode(currentRelativePath);

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
                Urls = new List<string>
                {
                    "/relative-path1",
                    "/relative-path2"
                }
            };

            var currentRelativePath = "/relative-path1";

            SecurityCodeSessionManager.SecurityCodeSession = Substitute.For<ISecurityCodeSession>();
            SecurityCodeSessionManager.SecurityCodeSession.UserSite.Returns("site1");
            SecurityCodeSessionManager.SecurityCodeSession.IsAlreadyInputSecurityCode.Returns(false);
            SecurityCodeSessionManager.SecurityCodeSession.SecurityCode.Returns("securitycode");

            //// Act
            var isRequired = SecurityCodeChecker.IsRequiredSecurityCode(currentRelativePath);

            //// Assert
            Assert.IsTrue(isRequired);
        }

        [Test]
        public void IsRequiredSecurityCode_SiteIsNotConfigAndPathIsRequiredSecurityCode_ReturnTrue()
        {
            AppSettings.SecurityCodeConfig = new SecurityCodeConfig
            {
                IsActive = true,
                Sites = Enumerable.Empty<string>(),
                Urls = new List<string>
                {
                    "/relative-path1",
                    "/relative-path2"
                }
            };

            var currentRelativePath = "/relative-path1";

            SecurityCodeSessionManager.SecurityCodeSession = Substitute.For<ISecurityCodeSession>();
            SecurityCodeSessionManager.SecurityCodeSession.UserSite.Returns("site1");
            SecurityCodeSessionManager.SecurityCodeSession.IsAlreadyInputSecurityCode.Returns(false);
            SecurityCodeSessionManager.SecurityCodeSession.SecurityCode.Returns("securitycode");

            //// Act
            var isRequired = SecurityCodeChecker.IsRequiredSecurityCode(currentRelativePath);

            //// Assert
            Assert.IsTrue(isRequired);
        }
    }
}