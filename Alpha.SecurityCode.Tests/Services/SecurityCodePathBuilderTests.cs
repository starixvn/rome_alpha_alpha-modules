namespace Alpha.SecurityCode.Tests.Services
{
    using System.Collections.Generic;
    using Configurations;
    using NSubstitute;
    using NUnit.Framework;
    using SecurityCode.Services;
    using Web;

    [TestFixture]
    public class SecurityCodePathBuilderTests
    {
        private SecurityCodePathBuilder securityCodePathBuilder;

        [SetUp]
        public void SetUp()
        {
            AppSettings.SecurityCodeConfig = new SecurityCodeConfig();
            SecurityCodeSessionManager.SecurityCodeSession = Substitute.For<SecurityCodeSession>();
            securityCodePathBuilder = new SecurityCodePathBuilderImpl();
        }

        [Test]
        public void BuildVerifyingSecurityCodePath_RedirectUrlIsEmpty_ReturnVerifyingSCPathWithoutRedirectUrl()
        {
            // Arrange
            var redirectUrl = string.Empty;
            var rootPath = "/current-microsite/";
            var appVirtualPath = "/current-microsite";
            AppSettings.SecurityCodeConfig.SiteMainApp = "/site-main";
            AppSettings.SecurityCodeConfig.VerifyingSecurityCodePath = "verifySecurityCodePath";

            // Act
            var verifySecurityCodePath = securityCodePathBuilder.BuildVerifyingSecurityCodePath(redirectUrl, null, rootPath, appVirtualPath);

            // Assert
            Assert.AreEqual("/site-main/verifySecurityCodePath", verifySecurityCodePath);
        }

        [Test]
        public void BuildVerifyingSecurityCodePath_RedirectUrlIsNotEmpty_ReturnVerifyingSCPathWithRedirectUrl()
        {
            // Arrange
            var redirectUrl = "redirect-url";
            var rootPath = "/current-microsite/";
            var appVirtualPath = "/current-microsite";
            AppSettings.SecurityCodeConfig.SiteMainApp = "/site-main";
            AppSettings.SecurityCodeConfig.VerifyingSecurityCodePath = "verifySecurityCodePath";

            // Act
            var verifySecurityCodePath = securityCodePathBuilder.BuildVerifyingSecurityCodePath(redirectUrl, null, rootPath, appVirtualPath);

            // Assert
            Assert.AreEqual("/site-main/verifySecurityCodePath?redirectUrl=redirect-url", verifySecurityCodePath);
        }

        [Test]
        public void BuildVerifyingSecurityCodePath_AddExtraQueryStringItem_ReturnVerifyingSCPathWithQueryStringItem()
        {
            // Arrange
            var redirectUrl = "redirect-url";
            var rootPath = "/current-microsite/";
            var appVirtualPath = "/current-microsite";
            var queryStringItems = new Dictionary<string, string>
            {
                { "item1", "1" },
                { "item2", "2" }
            };
            AppSettings.SecurityCodeConfig.SiteMainApp = "/site-main";
            AppSettings.SecurityCodeConfig.VerifyingSecurityCodePath = "verifySecurityCodePath";

            // Act
            var verifySecurityCodePath = securityCodePathBuilder.BuildVerifyingSecurityCodePath(redirectUrl, queryStringItems, rootPath, appVirtualPath);

            // Assert
            Assert.AreEqual("/site-main/verifySecurityCodePath?item1=1&item2=2&redirectUrl=redirect-url", verifySecurityCodePath);
        }

        [Test]
        public void BuildVerifyingSecurityCodePath_SiteMainAppIsEmpty_ReturnVerifyingSCPathWithCurrentSiterootPath()
        {
            // Arrange
            var redirectUrl = string.Empty;
            var rootPath = "/current-microsite/";
            var appVirtualPath = "/current-microsite";
            AppSettings.SecurityCodeConfig.VerifyingSecurityCodePath = "verifySecurityCodePath";

            // Act
            var verifySecurityCodePath = securityCodePathBuilder.BuildVerifyingSecurityCodePath(redirectUrl, null, rootPath, appVirtualPath);

            // Assert
            Assert.AreEqual("/current-microsite/verifySecurityCodePath", verifySecurityCodePath);
        }

        [Test]
        public void BuildResetSecurityCodeUrl_ReturnResetSecurityCodeUrl()
        {
            // Arrange
            var redirectUrl = string.Empty;
            var rootPath = "/current-microsite/";
            var appVirtualPath = "/current-microsite";
            AppSettings.SecurityCodeConfig.SiteMainApp = "/site-main";
            AppSettings.SecurityCodeConfig.ResetSecurityCodePath = "ResetSecurityCodeUrl";

            // Act
            var verifySecurityCodePath = securityCodePathBuilder.BuildResetSecurityCodePath(rootPath, appVirtualPath);

            // Assert
            Assert.AreEqual("/site-main/ResetSecurityCodeUrl", verifySecurityCodePath);
        }

        [Test]
        public void BuildResetSecurityCodeUrl_UserIsUsingOtp_ReturnVerifyingOtpPath()
        {
            // Arrange
            var redirectUrl = string.Empty;
            var rootPath = "/current-microsite/";
            var appVirtualPath = "/current-microsite";
            AppSettings.SecurityCodeConfig.SiteMainApp = "/site-main";
            AppSettings.SecurityCodeConfig.VerifyingOtpPath = "verifyOtpPath";
            SecurityCodeSessionManager.SecurityCodeSession.IsEnabledOtp.Returns(true);

            // Act
            var verifySecurityCodePath = securityCodePathBuilder.BuildVerifyingSecurityCodePath(redirectUrl, null, rootPath, appVirtualPath);

            // Assert
            Assert.AreEqual("/site-main/verifyOtpPath", verifySecurityCodePath);
        }
    }
}