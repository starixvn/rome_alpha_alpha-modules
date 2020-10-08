namespace Alpha.NetThrottling.Tests
{
    using Alpha.NetThrottling.Tests.Utils;
    using Alpha.NetThrottling.Utils;
    using Fanex.Caching;
    using NSubstitute;
    using System.Configuration;
    using System.Web;
    using Xunit;

    public class NetThrottlingHttpModuleTests
    {
        private readonly ICacheService cacheService;
        private readonly IClock clock;
        private readonly ILogger logger;
        private readonly HttpApplication httpApplication;

        public NetThrottlingHttpModuleTests()
        {
            cacheService = Substitute.For<ICacheService>();
            clock = Substitute.For<IClock>();
            logger = Substitute.For<ILogger>();
            httpApplication = Substitute.For<HttpApplication>();
        }

        [Fact]
        public void ThrottleRequest_Should_Invoke_AddOnBeginRequestAsync()
        {
            // Arrange
            ConfigurationManager.AppSettings["RedisConfigurationOptions"] = "123";
            var configuration = ConfigurationUtil.Load<NetThrottlingConfiguration>("NetThrottlingHttpModuleTests.config", "Alpha.NetThrottling");
            var module = new NetThrottlingAsyncHttpModule(configuration, cacheService, clock, logger);

            // Act
            module.Init(httpApplication);

            // Assert
            httpApplication.Received().AddOnBeginRequestAsync(Arg.Any<BeginEventHandler>(), Arg.Any<EndEventHandler>());
        }
    }
}
