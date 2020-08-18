namespace Alpha.NetThrottling.Tests
{
    using Alpha.NetThrottling.Models;
    using Alpha.NetThrottling.Utils;
    using Fanex.Caching;
    using NSubstitute;
    using System;
    using System.Configuration;
    using System.IO;
    using System.Threading.Tasks;
    using Xunit;

    public class NetThrottlerTests
    {
        private readonly NetThrottlingConfiguration configuration;
        private readonly CacheService cacheService;
        private readonly Clock clock;
        private readonly NetThrottler netThrottler;

        public NetThrottlerTests()
        {
            var configFileMap = new ExeConfigurationFileMap()
            {
                ExeConfigFilename = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "NetThrottlerTests.config")
            };
            var config = ConfigurationManager.OpenMappedExeConfiguration(configFileMap, ConfigurationUserLevel.None);
            configuration = config.GetSection("Alpha.NetThrottling") as NetThrottlingConfiguration;
            cacheService = Substitute.For<CacheService>();
            clock = Substitute.For<Clock>();
            netThrottler = new NetThrottler(configuration, cacheService, clock);
        }

        [Fact]
        public async Task ThrottleAsync_ReturnsBlocked_IfBlacklistAndBlockDurationGreaterThanZero()
        {
            // Arrange
            cacheService.GetAsync<DateTime>(Arg.Any<string>()).Returns(Task.FromResult(DateTime.Now));

            // Act
            var throttleResult = await netThrottler.ThrottleAsync("FilterKey1", 10, 10, 10);

            // Assert
            Assert.Equal(ThrottleResult.Blocked, throttleResult);
            await cacheService.Received().GetAsync<DateTime>(Arg.Is("NetThrottlerTests_Block_FilterKey1"));
        }

        [Fact]
        public async Task ThrottleAsync_SetBlacklist_IfBlockDurationIsGreaterThanZero()
        {
            // Arrange
            cacheService.GetAsync<DateTime>(Arg.Any<string>()).Returns(Task.FromResult(DateTime.MinValue));
            cacheService.GetAsync<ThrottleCounter>(Arg.Any<string>()).Returns(Task.FromResult(new ThrottleCounter
            {
                Timestamp = new DateTime(2020, 8, 8, 8, 8, 8),
                TotalRequests = 10
            }));
            clock.Now.Returns(new DateTime(2020, 8, 8, 8, 8, 9));

            // Act
            var throttleResult = await netThrottler.ThrottleAsync("FilterKey1", 10, 10, 10);

            // Assert
            Assert.Equal(ThrottleResult.Blocked, throttleResult);
            await cacheService.Received().GetAsync<ThrottleCounter>(Arg.Is("NetThrottlerTests_Throttle_FilterKey1"));
            await cacheService.Received().SetAsync(
                Arg.Is("NetThrottlerTests_Block_FilterKey1"),
                Arg.Is(new DateTime(2020, 8, 8, 8, 8, 9)),
                Arg.Is<CacheItemOptions>(x => x.RelativeExpiration == TimeSpan.FromSeconds(10)));
        }

        [Fact]
        public async Task ThrottleAsync_NotSetBlacklist_IfBlockDurationIsLessOrEqualZero()
        {
            // Arrange
            cacheService.GetAsync<ThrottleCounter>(Arg.Any<string>()).Returns(Task.FromResult(new ThrottleCounter
            {
                Timestamp = DateTime.Now,
                TotalRequests = 10
            }));

            // Act
            var throttleResult = await netThrottler.ThrottleAsync("FilterKey1", 10, 10, 0);

            // Assert
            Assert.Equal(ThrottleResult.Blocked, throttleResult);
            await cacheService.Received().GetAsync<ThrottleCounter>(Arg.Is("NetThrottlerTests_Throttle_FilterKey1"));
            await cacheService.DidNotReceive().SetAsync(
                Arg.Is("NetThrottlerTests_Block_FilterKey1"),
                Arg.Any<ThrottleCounter>(),
                Arg.Any<CacheItemOptions>());
        }

        [Fact]
        public async Task ThrottleAsync_ReturnsAllowed_IfNotReachMaxRequests()
        {
            // Arrange
            cacheService.GetAsync<ThrottleCounter>(Arg.Any<string>()).Returns(Task.FromResult<ThrottleCounter>(null));

            // Act
            var throttleResult = await netThrottler.ThrottleAsync("FilterKey1", 10, 10, 10);

            // Assert
            Assert.Equal(ThrottleResult.Allowed, throttleResult);
            await cacheService.Received().GetAsync<ThrottleCounter>(Arg.Is("NetThrottlerTests_Throttle_FilterKey1"));
            await cacheService.Received().SetAsync(
                Arg.Is("NetThrottlerTests_Throttle_FilterKey1"),
                Arg.Is<ThrottleCounter>(x => x.TotalRequests == 1),
                Arg.Is<CacheItemOptions>(o => o.SlidingExpiration == TimeSpan.FromSeconds(10)));
        }
    }
}
