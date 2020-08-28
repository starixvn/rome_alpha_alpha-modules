namespace Alpha.NetThrottling.Tests
{
    using Alpha.NetThrottling.Tests.Utils;
    using Alpha.NetThrottling.Utils;
    using Fanex.Caching;
    using NSubstitute;
    using System.Web;
    using Xunit;

    public class NetThrottlingHttpModuleTests
    {
        private readonly CacheService cacheService;
        private readonly Clock clock;
        private readonly HttpApplication httpApplication;

        public NetThrottlingHttpModuleTests()
        {
            cacheService = Substitute.For<CacheService>();
            clock = Substitute.For<Clock>();
            httpApplication = Substitute.For<HttpApplication>();
        }

        [Fact]
        public void ThrottleRequest_Should_Invoke_AddOnBeginRequestAsync()
        {
            // Arrange
            var configuration = ConfigurationUtil.Load<NetThrottlingConfiguration>("NetThrottlingHttpModuleTests.config", "Alpha.NetThrottling");
            var module = new NetThrottlingHttpModule(configuration, cacheService, clock);

            // Act
            module.Init(httpApplication);

            // Assert
            httpApplication.Received().AddOnBeginRequestAsync(Arg.Any<BeginEventHandler>(), Arg.Any<EndEventHandler>());
        }
    }
}
