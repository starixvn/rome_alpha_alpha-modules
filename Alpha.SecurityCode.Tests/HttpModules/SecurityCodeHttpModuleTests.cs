namespace Alpha.SecurityCode.Tests.HttpModules
{
    using NSubstitute;
    using NUnit.Framework;
    using SecurityCode.HttpModules;
    using SecurityCode.Services;
    using Web;

    [TestFixture]
    public class SecurityCodeHttpModuleTests
    {
        private SecurityCodeHttpModule securityCodeHttpModule;
        private SecurityCodeChecker securityCodeChecker;
        private SecurityCodeHttpContext securityCodeHttpContext;

        [SetUp]
        public void SetUp()
        {
            securityCodeChecker = Substitute.For<SecurityCodeChecker>();
            securityCodeHttpContext = Substitute.For<SecurityCodeHttpContext>();
            securityCodeHttpModule = new SecurityCodeHttpModule(securityCodeChecker, securityCodeHttpContext);
        }

        [Test]
        public void AcquireRequestState_RequestIsNotValid_DontExecuteIsRequiredSecurityCode()
        {
            // Arrange
            securityCodeHttpContext.IsValidRequest().Returns(false);

            // Act
            securityCodeHttpModule.AcquireRequestState(null, null);

            // Assert
            securityCodeChecker.DidNotReceive().IsRequiredSecurityCode(Arg.Any<string>());
        }

        [Test]
        public void AcquireRequestState_RequestIsValid_ExecuteIsRequiredSecurityCode()
        {
            // Arrange
            securityCodeHttpContext.IsValidRequest().Returns(true);
            securityCodeHttpContext.GetRelativePath().Returns("/RelativePath");

            // Act
            securityCodeHttpModule.AcquireRequestState(null, null);

            // Assert
            securityCodeChecker.Received().IsRequiredSecurityCode("/RelativePath");
        }

        [Test]
        public void AcquireRequestState_GetActionIsRequiredCheck_ExecuteRedirectToVerifySecurityCodePage()
        {
            // Arrange
            securityCodeHttpContext.IsValidRequest().Returns(true);
            securityCodeHttpContext.IsAjaxRequest().Returns(false);
            securityCodeChecker.IsRequiredSecurityCode(Arg.Any<string>()).Returns(true);

            // Act
            securityCodeHttpModule.AcquireRequestState(null, null);

            // Assert
            securityCodeHttpContext.Received().RedirectToVerifySecurityCodePage();
        }

        [Test]
        public void AcquireRequestState_PostAjaxActionIsRequiredCheck_ExecuteDenyAjaxRequest()
        {
            // Arrange
            securityCodeHttpContext.IsValidRequest().Returns(true);
            securityCodeHttpContext.IsAjaxRequest().Returns(true);
            securityCodeChecker.IsRequiredSecurityCode(Arg.Any<string>()).Returns(true);

            // Act
            securityCodeHttpModule.AcquireRequestState(null, null);

            // Assert
            securityCodeHttpContext.Received().DenyAjaxRequest();
        }
    }
}