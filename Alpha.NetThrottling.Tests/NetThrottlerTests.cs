namespace Alpha.NetThrottling.Tests
{
    using System;
    using System.Threading.Tasks;
    using System.Web;
    using Alpha.NetThrottling.Filters;
    using Alpha.NetThrottling.Models;
    using Alpha.NetThrottling.Tests.Utils;
    using Alpha.NetThrottling.Utils;
    using Fanex.Caching;
    using NSubstitute;
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

        [Fact]
        public void IsBlacklist_ReturnTrue_IfHaveCache()
        {
            // Arrange
            cacheService.Contain(Arg.Any<string>()).Returns(true);
            var cacheIdentity = new CacheIdentity("throttleKey", "blockKey");

            // Act
            var result = netThrottler.IsBlacklist(cacheIdentity);

            // Assert
            Assert.True(result);
            cacheService.Received().Contain(Arg.Is("blockKey"));
        }

        [Fact]
        public void IsBlacklist_ReturnFalse_IfHaveNoCache()
        {
            // Arrange
            cacheService.Contain(Arg.Any<string>()).Returns(false);
            var cacheIdentity = new CacheIdentity("throttleKey", "blockKey");

            // Act
            var result = netThrottler.IsBlacklist(cacheIdentity);

            // Assert
            Assert.False(result);
            cacheService.Received().Contain(Arg.Is("blockKey"));
        }

        [Fact]
        public void RemoveBlacklist_ShouldWork()
        {
            // Arrange
            var cacheIdentity = new CacheIdentity("throttleKey", "blockKey");

            // Act
            netThrottler.RemoveBlacklist(cacheIdentity);

            // Assert
            cacheService.Received().Remove(Arg.Is("blockKey"));
        }

        [Fact]
        public void RemoveThrottling_ShouldWork()
        {
            // Arrange
            var cacheIdentity = new CacheIdentity("throttleKey", "blockKey");

            // Act
            netThrottler.RemoveThrottling(cacheIdentity);

            // Assert
            cacheService.Received().Remove(Arg.Is("throttleKey"));
        }

        [Fact]
        public void AddBlacklist_ShouldWork()
        {
            // Arrange
            var cacheIdentity = new CacheIdentity("throttleKey", "blockKey");
            var throttlePolicy = Substitute.For<IThrottlePolicy>();
            throttlePolicy.BlockDuration.Returns(3);
            clock.UtcNow.Returns(new DateTime(2020, 1, 1));

            // Act
            netThrottler.AddBlacklist(throttlePolicy, cacheIdentity);

            // Assert
            cacheService.Received().Set(
                Arg.Is("blockKey"),
                Arg.Is(new DateTime(2020, 1, 1)),
                Arg.Is<CacheItemOptions>(x => x.RelativeExpiration == TimeSpan.FromSeconds(3)));
        }

        [Fact]
        public void GetBlockRemainingTime_ReturnsZero_IfNotBlocked()
        {
            // Arrange
            var cacheIdentity = new CacheIdentity("throttleKey", "blockKey");
            var throttlePolicy = Substitute.For<IThrottlePolicy>();
            cacheService.TryGet(Arg.Any<string>(), out DateTime dateTime).Returns(false);

            // Act
            var blockRemainingTime = netThrottler.GetBlockRemainingTime(throttlePolicy, cacheIdentity);

            // Assert
            Assert.Equal(TimeSpan.Zero, blockRemainingTime);
        }

        [Fact]
        public void GetBlockRemainingTime_ReturnsNotZero_IfBlocked()
        {
            // Arrange
            var cacheIdentity = new CacheIdentity("throttleKey", "blockKey");
            var throttlePolicy = Substitute.For<IThrottlePolicy>();
            throttlePolicy.BlockDuration.Returns(10);
            clock.UtcNow.Returns(new DateTime(2020, 10, 10, 10, 10, 10));
            cacheService.TryGet(Arg.Any<string>(), out DateTime dateTime).Returns(x =>
            {
                x[1] = new DateTime(2020, 10, 10, 10, 10, 5);
                return true;
            });

            // Act
            var blockRemainingTime = netThrottler.GetBlockRemainingTime(throttlePolicy, cacheIdentity);

            // Assert
            Assert.Equal(TimeSpan.FromSeconds(5), blockRemainingTime);
        }

        [Fact]
        public async Task IsBlacklistAsync_ReturnTrue_IfHaveCache()
        {
            // Arrange
            cacheService.ContainAsync(Arg.Any<string>()).Returns(true);
            var cacheIdentity = new CacheIdentity("throttleKey", "blockKey");

            // Act
            var result = await netThrottler.IsBlacklistAsync(cacheIdentity);

            // Assert
            Assert.True(result);
            await cacheService.Received().ContainAsync(Arg.Is("blockKey"));
        }

        [Fact]
        public async Task IsBlacklistAsync_ReturnFalse_IfHaveNoCache()
        {
            // Arrange
            cacheService.ContainAsync(Arg.Any<string>()).Returns(false);
            var cacheIdentity = new CacheIdentity("throttleKey", "blockKey");

            // Act
            var result = await netThrottler.IsBlacklistAsync(cacheIdentity);

            // Assert
            Assert.False(result);
            await cacheService.Received().ContainAsync(Arg.Is("blockKey"));
        }

        [Fact]
        public async Task RemoveBlacklistAsync_ShouldWork()
        {
            // Arrange
            var cacheIdentity = new CacheIdentity("throttleKey", "blockKey");

            // Act
            await netThrottler.RemoveBlacklistAsync(cacheIdentity);

            // Assert
            await cacheService.Received().RemoveAsync(Arg.Is("blockKey"));
        }

        [Fact]
        public async Task RemoveThrottlingAsync_ShouldWork()
        {
            // Arrange
            var cacheIdentity = new CacheIdentity("throttleKey", "blockKey");

            // Act
            await netThrottler.RemoveThrottlingAsync(cacheIdentity);

            // Assert
            await cacheService.Received().RemoveAsync(Arg.Is("throttleKey"));
        }

        [Fact]
        public async Task AddBlacklistAsync_ShouldWork()
        {
            // Arrange
            var cacheIdentity = new CacheIdentity("throttleKey", "blockKey");
            var throttlePolicy = Substitute.For<IThrottlePolicy>();
            throttlePolicy.BlockDuration.Returns(3);
            clock.UtcNow.Returns(new DateTime(2020, 1, 1));

            // Act
            await netThrottler.AddBlacklistAsync(throttlePolicy, cacheIdentity);

            // Assert
            await cacheService.Received().SetAsync(
                Arg.Is("blockKey"),
                Arg.Is(new DateTime(2020, 1, 1)),
                Arg.Is<CacheItemOptions>(x => x.RelativeExpiration == TimeSpan.FromSeconds(3)));
        }

        [Fact]
        public async Task GetBlockRemainingTimeAsync_ReturnsZero_IfNotBlocked()
        {
            // Arrange
            var cacheIdentity = new CacheIdentity("throttleKey", "blockKey");
            var throttlePolicy = Substitute.For<IThrottlePolicy>();
            cacheService.TryGetAsync<DateTime>(Arg.Any<string>()).Returns(Task.FromResult((false, DateTime.Now)));

            // Act
            var blockRemainingTime = await netThrottler.GetBlockRemainingTimeAsync(throttlePolicy, cacheIdentity);

            // Assert
            Assert.Equal(TimeSpan.Zero, blockRemainingTime);
        }

        [Fact]
        public async Task GetBlockRemainingTimeAsync_ReturnsNotZero_IfBlocked()
        {
            // Arrange
            var cacheIdentity = new CacheIdentity("throttleKey", "blockKey");
            var throttlePolicy = Substitute.For<IThrottlePolicy>();
            throttlePolicy.BlockDuration.Returns(10);
            clock.UtcNow.Returns(new DateTime(2020, 10, 10, 10, 10, 10));
            cacheService.TryGetAsync<DateTime>(Arg.Any<string>()).Returns(Task.FromResult((true, new DateTime(2020, 10, 10, 10, 10, 5))));

            // Act
            var blockRemainingTime = await netThrottler.GetBlockRemainingTimeAsync(throttlePolicy, cacheIdentity);

            // Assert
            Assert.Equal(TimeSpan.FromSeconds(5), blockRemainingTime);
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
            var throttleResult = await netThrottler.ThrottleAsync(new HttpContextWrapper(HttpContext.Current), throttlePolicy, filter, false);

            // Assert
            Assert.Equal(ThrottleResult.Allowed, throttleResult);
        }

        [Fact]
        public async Task ThrottleAsync_ReturnsBlocked_IfKeyIsBlacklist()
        {
            // Arrange
            cacheService.ContainAsync(Arg.Any<string>()).Returns(true);
            var throttlePolicy = new ThrottlePolicy("Test", 10, 10, 10);
            var cacheIdentity = new CacheIdentity("throttleKey", "blockKey");

            // Act
            var throttleResult = await netThrottler.ThrottleAsync(throttlePolicy, cacheIdentity, false);

            // Assert
            Assert.Equal(ThrottleResult.Allowed, throttleResult);
            await cacheService.Received().ContainAsync(Arg.Is("blockKey"));
        }

        [Fact]
        public async Task ThrottleAsync_NotBlacklist_FirstRequest()
        {
            // Arrange
            cacheService.ContainAsync(Arg.Any<string>()).Returns(Task.FromResult(false));
            cacheService.TryGetAsync<ThrottleCounter>(Arg.Any<string>()).Returns(Task.FromResult<(bool, ThrottleCounter)>((false, null)));
            clock.UtcNow.Returns(DateTime.SpecifyKind(new DateTime(2020, 8, 8, 8, 8, 9), DateTimeKind.Utc));
            var throttlePolicy = new ThrottlePolicy("Test", 10, 10, 10);
            var cacheIdentity = new CacheIdentity("throttleKey", "blockKey");

            // Act
            var throttleResult = await netThrottler.ThrottleAsync(throttlePolicy, cacheIdentity, false);

            // Assert
            Assert.Equal(ThrottleResult.Allowed, throttleResult);
            await cacheService.Received().ContainAsync(Arg.Is("blockKey"));
            await cacheService.Received().TryGetAsync<ThrottleCounter>(Arg.Is("throttleKey"));
            await cacheService.Received().SetAsync(
                Arg.Is("throttleKey"),
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
            var cacheIdentity = new CacheIdentity("throttleKey", "blockKey");

            // Act
            var throttleResult = await netThrottler.ThrottleAsync(throttlePolicy, cacheIdentity, false);

            // Assert
            Assert.Equal(ThrottleResult.Allowed, throttleResult);
            await cacheService.Received().ContainAsync(Arg.Is("blockKey"));
            await cacheService.Received().TryGetAsync<ThrottleCounter>(Arg.Is("throttleKey"));
            await cacheService.Received().SetAsync(
                Arg.Is("throttleKey"),
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
            var cacheIdentity = new CacheIdentity("throttleKey", "blockKey");

            // Act
            var throttleResult = await netThrottler.ThrottleAsync(throttlePolicy, cacheIdentity, false);

            // Assert
            Assert.Equal(ThrottleResult.Allowed, throttleResult);
            await cacheService.Received().ContainAsync(Arg.Is("blockKey"));
            await cacheService.Received().TryGetAsync<ThrottleCounter>(Arg.Is("throttleKey"));
            await cacheService.Received().SetAsync(
                Arg.Is("blockKey"),
                Arg.Is(DateTime.SpecifyKind(new DateTime(2020, 8, 8, 8, 8, 9), DateTimeKind.Utc)),
                Arg.Is<CacheItemOptions>(x => x.RelativeExpiration == TimeSpan.FromSeconds(10)));
        }
            var cacheIdentity = new CacheIdentity("path_throttle_key", "path_block_key");
            filter.GetCacheIdentity(Arg.Any<HttpContextBase>()).Returns(cacheIdentity);
    }
    */
}
