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
    /*
    public class NetThrottlerTests
    {
        private readonly IThrottleSetting throttleSetting;
        private readonly IClock clock;
        private readonly ICacheServiceWrapper cacheService;
        private readonly INetThrottler netThrottler;

        public NetThrottlerTests()
        {
            clock = Substitute.For<IClock>();
            cacheService = Substitute.For<ICacheServiceWrapper>();
            throttleSetting = Substitute.For<IThrottleSetting>();
            throttleSetting.CacheService.Returns(cacheService);
            netThrottler = new NetThrottler();
        }

        [Fact, CurrentHttpContext]
        public async Task ThrottleAsync_ReturnAllowed_IfCacheIdentityIsNull()
        {
            // Arrange
            var filter = Substitute.For<IFilter>();
            var throttlePolicy = new ThrottlePolicy("Test", 10, 10, 10);
            CacheIdentity cacheIdentity = null;
            filter.GetCacheIdentity(Arg.Any<HttpContextBase>(), Arg.Any<IThrottlePolicy>()).Returns(cacheIdentity);

            // Act
            var throttleResult = await netThrottler.ThrottleAsync(new HttpContextWrapper(HttpContext.Current), throttlePolicy, filter);

            // Assert
            Assert.Equal(ThrottleResult.Allowed, throttleResult);
        }

        [Fact]
        public async Task ThrottleAsync_ReturnsBlocked_IfKeyIsBlacklist()
        {
            // Arrange
            cacheService.ContainAsync(Arg.Any<string>()).Returns(true);
            var throttlePolicy = new ThrottlePolicy("Test", 10, 10, 10);
            var cacheIdentity = new CacheIdentity("path_throttle_key", "path_block_key");

            // Act
            var throttleResult = await netThrottler.ThrottleAsync(throttlePolicy, cacheIdentity);

            // Assert
            Assert.Equal(ThrottleResult.Allowed, throttleResult);
            await cacheService.Received().ContainAsync(Arg.Is("path_block_key"));
        }

        [Fact]
        public async Task ThrottleAsync_NotBlacklist_FirstRequest()
        {
            // Arrange
            cacheService.ContainAsync(Arg.Any<string>()).Returns(Task.FromResult(false));
            cacheService.TryGetAsync<ThrottleCounter>(Arg.Any<string>()).Returns(Task.FromResult<(bool, ThrottleCounter)>((false, null)));
            clock.UtcNow.Returns(DateTime.SpecifyKind(new DateTime(2020, 8, 8, 8, 8, 9), DateTimeKind.Utc));
            var throttlePolicy = new ThrottlePolicy("Test", 10, 10, 10);
            var cacheIdentity = new CacheIdentity("path_throttle_key", "path_block_key");

            // Act
            var throttleResult = await netThrottler.ThrottleAsync(throttlePolicy, cacheIdentity);

            // Assert
            Assert.Equal(ThrottleResult.Allowed, throttleResult);
            await cacheService.Received().ContainAsync(Arg.Is("path_block_key"));
            await cacheService.Received().TryGetAsync<ThrottleCounter>(Arg.Is("path_throttle_key"));
            await cacheService.Received().SetAsync(
                Arg.Is("path_throttle_key"),
                Arg.Is<ThrottleCounter>(x => x.TotalRequests == 1 && x.Timestamp == DateTime.SpecifyKind(new DateTime(2020, 8, 8, 8, 8, 9), DateTimeKind.Utc)),
                Arg.Is<CacheItemOptions>(x => x.SlidingExpiration == TimeSpan.FromSeconds(10)));
        }

        [Fact]
        public async Task ThrottleAsync_NotSetBlacklist_SecondRequest()
        {
            // Arrange
            cacheService.ContainAsync(Arg.Any<string>()).Returns(Task.FromResult(false));
            cacheService.TryGetAsync<ThrottleCounter>(Arg.Any<string>()).Returns(Task.FromResult((true, new ThrottleCounter
            {
                Timestamp = DateTime.SpecifyKind(new DateTime(2020, 8, 8, 8, 8, 8), DateTimeKind.Utc),
                TotalRequests = 1
            })));
            clock.UtcNow.Returns(DateTime.SpecifyKind(new DateTime(2020, 8, 8, 8, 8, 9), DateTimeKind.Utc));
            var throttlePolicy = new ThrottlePolicy("Test", 10, 10, 10);
            var cacheIdentity = new CacheIdentity("path_throttle_key", "path_block_key");

            // Act
            var throttleResult = await netThrottler.ThrottleAsync(throttlePolicy, cacheIdentity);

            // Assert
            Assert.Equal(ThrottleResult.Allowed, throttleResult);
            await cacheService.Received().ContainAsync(Arg.Is("path_block_key"));
            await cacheService.Received().TryGetAsync<ThrottleCounter>(Arg.Is("path_throttle_key"));
            await cacheService.Received().SetAsync(
                Arg.Is("path_throttle_key"),
                Arg.Is<ThrottleCounter>(x => x.TotalRequests == 2 && x.Timestamp == DateTime.SpecifyKind(new DateTime(2020, 8, 8, 8, 8, 8), DateTimeKind.Utc)),
                Arg.Is<CacheItemOptions>(x => x.SlidingExpiration == TimeSpan.FromSeconds(10)));
        }

        [Fact]
        public async Task ThrottleAsync_SetBlacklist_IfReachMaxRequests()
        {
            // Arrange
            cacheService.ContainAsync(Arg.Any<string>()).Returns(Task.FromResult(false));
            cacheService.TryGetAsync<ThrottleCounter>(Arg.Any<string>()).Returns(Task.FromResult((true, new ThrottleCounter
            {
                Timestamp = DateTime.SpecifyKind(new DateTime(2020, 8, 8, 8, 8, 8), DateTimeKind.Utc),
                TotalRequests = 10
            })));
            clock.UtcNow.Returns(DateTime.SpecifyKind(new DateTime(2020, 8, 8, 8, 8, 9), DateTimeKind.Utc));
            var throttlePolicy = new ThrottlePolicy("Test", 10, 10, 10);
            var cacheIdentity = new CacheIdentity("path_throttle_key", "path_block_key");

            // Act
            var throttleResult = await netThrottler.ThrottleAsync(throttlePolicy, cacheIdentity);

            // Assert
            Assert.Equal(ThrottleResult.Allowed, throttleResult);
            await cacheService.Received().ContainAsync(Arg.Is("path_block_key"));
            await cacheService.Received().TryGetAsync<ThrottleCounter>(Arg.Is("path_throttle_key"));
            await cacheService.Received().SetAsync(
                Arg.Is("path_block_key"),
                Arg.Is(DateTime.SpecifyKind(new DateTime(2020, 8, 8, 8, 8, 9), DateTimeKind.Utc)),
                Arg.Is<CacheItemOptions>(x => x.RelativeExpiration == TimeSpan.FromSeconds(10)));
        }
            var cacheIdentity = new CacheIdentity("path_throttle_key", "path_block_key");
            filter.GetCacheIdentity(Arg.Any<HttpContextBase>()).Returns(cacheIdentity);
    }
    */
}
