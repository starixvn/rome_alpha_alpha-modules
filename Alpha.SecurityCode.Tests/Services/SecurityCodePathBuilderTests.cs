namespace Alpha.SecurityCode.Tests.Services
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using Configurations;
    using NUnit.Framework;
    using SecurityCode.Services;

    [TestFixture]
    public class SecurityCodePathBuilderTests
    {
        private SecurityCodePathBuilder securityCodePathBuilder;

        [SetUp]
        public void SetUp()
        {
            AppSettings.SecurityCodeConfig = new SecurityCodeConfig();
            securityCodePathBuilder = new SecurityCodePathBuilderImpl();
        }

        [Test]
        public void BuildVerifyingSecurityCodePath_RedirectUrlIsEmpty_ReturnVerifyingSCPathWithoutRedirectUrl()
        {
            //// Arrange
            var redirectUrl = string.Empty;
            var rootPath = "/current-microsite/";
            var appVirtualPath = "/current-microsite";
            AppSettings.SecurityCodeConfig.SiteMainApp = "/site-main";
            AppSettings.SecurityCodeConfig.VerifyingSecurityCodePath = "verifySecurityCodePath";

            //// Act
            var verifySecurityCodePath = securityCodePathBuilder.BuildVerifyingSecurityCodePath(redirectUrl, null, rootPath, appVirtualPath);

            //// Assert
            Assert.AreEqual("/site-main/verifySecurityCodePath", verifySecurityCodePath);
        }

        [Test]
        public void BuildVerifyingSecurityCodePath_RedirectUrlIsNotEmpty_ReturnVerifyingSCPathWithRedirectUrl()
        {
            //// Arrange
            var redirectUrl = "redirect-url";
            var rootPath = "/current-microsite/";
            var appVirtualPath = "/current-microsite";
            AppSettings.SecurityCodeConfig.SiteMainApp = "/site-main";
            AppSettings.SecurityCodeConfig.VerifyingSecurityCodePath = "verifySecurityCodePath";

            //// Act
            var verifySecurityCodePath = securityCodePathBuilder.BuildVerifyingSecurityCodePath(redirectUrl, null, rootPath, appVirtualPath);

            //// Assert
            Assert.AreEqual("/site-main/verifySecurityCodePath?redirectUrl=redirect-url", verifySecurityCodePath);
        }

        [Test]
        public void BuildVerifyingSecurityCodePath_AddExtraQueryStringItem_ReturnVerifyingSCPathWithQueryStringItem()
        {
            //// Arrange
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

            //// Act
            var verifySecurityCodePath = securityCodePathBuilder.BuildVerifyingSecurityCodePath(redirectUrl, queryStringItems, rootPath, appVirtualPath);

            //// Assert
            Assert.AreEqual("/site-main/verifySecurityCodePath?item1=1&item2=2&redirectUrl=redirect-url", verifySecurityCodePath);
        }

        [Test]
        public void BuildVerifyingSecurityCodePath_SiteMainAppIsEmpty_ReturnVerifyingSCPathWithCurrentSiterootPath()
        {
            //// Arrange
            var redirectUrl = string.Empty;
            var rootPath = "/current-microsite/";
            var appVirtualPath = "/current-microsite";
            AppSettings.SecurityCodeConfig.VerifyingSecurityCodePath = "verifySecurityCodePath";

            //// Act
            var verifySecurityCodePath = securityCodePathBuilder.BuildVerifyingSecurityCodePath(redirectUrl, null, rootPath, appVirtualPath);

            //// Assert
            Assert.AreEqual("/current-microsite/verifySecurityCodePath", verifySecurityCodePath);
        }

        [Test]
        public void BuildResetSecurityCodeUrl_ReturnResetSecurityCodeUrl()
        {
            //// Arrange
            var redirectUrl = string.Empty;
            var rootPath = "/current-microsite/";
            var appVirtualPath = "/current-microsite";
            AppSettings.SecurityCodeConfig.SiteMainApp = "/site-main";
            AppSettings.SecurityCodeConfig.ResetSecurityCodePath = "ResetSecurityCodeUrl";

            //// Act
            var verifySecurityCodePath = securityCodePathBuilder.BuildResetSecurityCodePath(rootPath, appVirtualPath);

            //// Assert
            Assert.AreEqual("/site-main/ResetSecurityCodeUrl", verifySecurityCodePath);
        }
    }
}
