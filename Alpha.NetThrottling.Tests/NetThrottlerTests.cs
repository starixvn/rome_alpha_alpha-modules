namespace Alpha.NetThrottling.Tests
{
    using Alpha.NetThrottling.Filters;
    using Alpha.NetThrottling.Models;
    using Alpha.NetThrottling.Tests.Utils;
    using Alpha.NetThrottling.Utils;
    using Fanex.Caching;
    using NSubstitute;
    using System;
    using System.Threading.Tasks;
    using System.Web;
    using Xunit;

    public class NetThrottlerTests
    {
        private readonly IThrottleSetting throttleSetting;
        private readonly ICacheService cacheService;
        private readonly Clock clock;

        public NetThrottlerTests()
        {
            cacheService = Substitute.For<ICacheService>();
            clock = Substitute.For<Clock>();
            throttleSetting = Substitute.For<IThrottleSetting>();
        }

        [Fact]
        public async Task ThrottleAsync_ReturnsBlocked_IfKeyIsBlacklist()
        {
            // Arrange
            cacheService.GetAsync<DateTime>(Arg.Any<string>()).Returns(Task.FromResult(new DateTime(2020, 8, 8, 8, 8, 8)));
            var throttlePolicy = new ThrottlePolicy("Test", 10, 10, 10);
            var cacheIdentity = new CacheIdentity("path_throttle_key", "path_block_key");

            // Act
            var netThrottler = new NetThrottler(throttleSetting, cacheService, clock);
            var throttleResult = await netThrottler.ThrottleAsync(throttlePolicy, cacheIdentity);

            // Assert
            Assert.Equal(ThrottleResult.Blocked, throttleResult);
            await cacheService.Received().GetAsync<DateTime>(Arg.Is("NetThrottlerTests_Block_FilterKey1"));
        }

        [Fact]
        public async Task ThrottleAsync_NotBlacklist_FirstRequest()
        {
            // Arrange
            cacheService.GetAsync<DateTime>(Arg.Any<string>()).Returns(Task.FromResult(DateTime.MinValue));
            cacheService.GetAsync<ThrottleCounter>(Arg.Any<string>()).Returns(Task.FromResult<ThrottleCounter>(null));
            clock.Now.Returns(new DateTime(2020, 8, 8, 8, 8, 9));
            var throttlePolicy = new ThrottlePolicy("Test", 10, 10, 10);
            var cacheIdentity = new CacheIdentity("path_throttle_key", "path_block_key");

            // Act
            var netThrottler = new NetThrottler(throttleSetting, cacheService, clock);
            var throttleResult = await netThrottler.ThrottleAsync(throttlePolicy, cacheIdentity);

            // Assert
            Assert.Equal(ThrottleResult.Allowed, throttleResult);
            await cacheService.Received().GetAsync<DateTime>(Arg.Is("NetThrottlerTests_Block_FilterKey1"));
            await cacheService.Received().GetAsync<ThrottleCounter>(Arg.Is("NetThrottlerTests_Throttle_FilterKey1"));
            await cacheService.Received().SetAsync(
                Arg.Is("NetThrottlerTests_Throttle_FilterKey1"),
                Arg.Is<ThrottleCounter>(x => x.TotalRequests == 1 && x.Timestamp == new DateTime(2020, 8, 8, 8, 8, 9)),
                Arg.Is<CacheItemOptions>(x => x.SlidingExpiration == TimeSpan.FromSeconds(10)));
        }

        [Fact]
        public async Task ThrottleAsync_NotSetBlacklist_SecondRequest()
        {
            // Arrange
            cacheService.GetAsync<DateTime>(Arg.Any<string>()).Returns(Task.FromResult(DateTime.MinValue));
            cacheService.GetAsync<ThrottleCounter>(Arg.Any<string>()).Returns(Task.FromResult(new ThrottleCounter
            {
                Timestamp = new DateTime(2020, 8, 8, 8, 8, 8),
                TotalRequests = 1
            }));
            clock.Now.Returns(new DateTime(2020, 8, 8, 8, 8, 9));
            var throttlePolicy = new ThrottlePolicy("Test", 10, 10, 10);
            var cacheIdentity = new CacheIdentity("path_throttle_key", "path_block_key");

            // Act
            var netThrottler = new NetThrottler(throttleSetting, cacheService, clock);
            var throttleResult = await netThrottler.ThrottleAsync(throttlePolicy, cacheIdentity);

            // Assert
            Assert.Equal(ThrottleResult.Allowed, throttleResult);
            await cacheService.Received().GetAsync<DateTime>(Arg.Is("NetThrottlerTests_Block_FilterKey1"));
            await cacheService.Received().GetAsync<ThrottleCounter>(Arg.Is("NetThrottlerTests_Throttle_FilterKey1"));
            await cacheService.Received().SetAsync(
                Arg.Is("NetThrottlerTests_Throttle_FilterKey1"),
                Arg.Is<ThrottleCounter>(x => x.TotalRequests == 2 && x.Timestamp == new DateTime(2020, 8, 8, 8, 8, 8)),
                Arg.Is<CacheItemOptions>(x => x.SlidingExpiration == TimeSpan.FromSeconds(10)));
        }

        [Fact]
        public async Task ThrottleAsync_SetBlacklist_IfReachMaxRequests()
        {
            // Arrange
            cacheService.GetAsync<DateTime>(Arg.Any<string>()).Returns(Task.FromResult(DateTime.MinValue));
            cacheService.GetAsync<ThrottleCounter>(Arg.Any<string>()).Returns(Task.FromResult(new ThrottleCounter
            {
                Timestamp = new DateTime(2020, 8, 8, 8, 8, 8),
                TotalRequests = 10
            }));
            clock.Now.Returns(new DateTime(2020, 8, 8, 8, 8, 9));
            var throttlePolicy = new ThrottlePolicy("Test", 10, 10, 10);
            var cacheIdentity = new CacheIdentity("path_throttle_key", "path_block_key");

            // Act
            var netThrottler = new NetThrottler(throttleSetting, cacheService, clock);
            var throttleResult = await netThrottler.ThrottleAsync(throttlePolicy, cacheIdentity);

            // Assert
            Assert.Equal(ThrottleResult.Blocked, throttleResult);
            await cacheService.Received().GetAsync<DateTime>(Arg.Is("NetThrottlerTests_Block_FilterKey1"));
            await cacheService.Received().GetAsync<ThrottleCounter>(Arg.Is("NetThrottlerTests_Throttle_FilterKey1"));
            await cacheService.Received().SetAsync(
                Arg.Is("NetThrottlerTests_Block_FilterKey1"),
                Arg.Is(new DateTime(2020, 8, 8, 8, 8, 9)),
                Arg.Is<CacheItemOptions>(x => x.RelativeExpiration == TimeSpan.FromSeconds(10)));
        }

        [Theory, CurrentHttpContext]
        [InlineData("")]
        [InlineData(null)]
        public async Task ThrottleAsync_ReturnAllowed_IfFilterIdentityIsNullOrEmpty(string filterIdentity)
        {
            // Arrange
            var filter = Substitute.For<IFilter>();
            var throttlePolicy = new ThrottlePolicy("Test", 10, 10, 10);
            var cacheIdentity = new CacheIdentity("path_throttle_key", "path_block_key");
            filter.GetCacheIdentity(Arg.Any<HttpContextBase>()).Returns(cacheIdentity);

            // Act
            var netThrottler = new NetThrottler(throttleSetting, cacheService, clock);
            var throttleResult = await netThrottler.ThrottleAsync(new HttpContextWrapper(HttpContext.Current), throttlePolicy, filter);

            // Assert
            Assert.Equal(ThrottleResult.Allowed, throttleResult);
        }
    }
}
