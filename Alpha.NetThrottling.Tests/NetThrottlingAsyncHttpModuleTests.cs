namespace Alpha.NetThrottling.Tests
{
    using NSubstitute;
    using System.Configuration;
    using System.Web;
    using Xunit;

    public class NetThrottlingAsyncHttpModuleTests
    {
        private readonly INetThrottler netThrottler;
        private readonly HttpApplication httpApplication;

        public NetThrottlingAsyncHttpModuleTests()
        {
            httpApplication = Substitute.For<HttpApplication>();
            netThrottler = Substitute.For<INetThrottler>();
        }

        [Fact]
        public void ThrottleRequest_Should_Invoke_AddOnBeginRequestAsync()
        {
            // Arrange
            ConfigurationManager.AppSettings["RedisConfigurationOptions"] = "123";

            // Act
            var module = new NetThrottlingAsyncHttpModule(netThrottler, isUsedRedis: true);
            module.Init(httpApplication);

            // Assert
            httpApplication.Received().AddOnBeginRequestAsync(Arg.Any<BeginEventHandler>(), Arg.Any<EndEventHandler>());
        }
    }
}
