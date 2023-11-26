using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Caching.Memory;
using Moq;
using ParrotIncSquawk.Middleware;
using System;
using System.Threading.Tasks;
using Xunit;

namespace ParrotIncSquawk.Tests;

public class RateLimitMiddlewareTests
{
	public static class MockMemoryCacheService
	{
		public static Mock<IMemoryCache> GetMemoryCache(string key, object expectedValue)
		{
			Mock<IMemoryCache> mockMemoryCache = new();
			mockMemoryCache
				.Setup(x => x.TryGetValue(key, out expectedValue))
				.Returns(true);
			return mockMemoryCache;
		}
	}

	[Fact]
	public async Task Middleware_AllowsRequestWhenBelowRateLimit()
	{
		// Arrange
		string userId_cacheKey = "FBF26574-9DB6-4C85-CF68-08DBDF13762C";
		DefaultHttpContext context = new DefaultHttpContext();
		context.Request.RouteValues.Add("userId", userId_cacheKey);

		IMemoryCache memoryCache = Mock.Of<IMemoryCache>();
		ICacheEntry cachEntry = Mock.Of<ICacheEntry>();

		Mock<IMemoryCache> mockMemoryCache = Mock.Get(memoryCache);
		mockMemoryCache
			.Setup(m => m.CreateEntry(It.IsAny<object>()))
			.Returns(cachEntry);

		RateLimitMiddleware middleware = new((innerHttpContext) => Task.FromResult(0), mockMemoryCache.Object);

		// Act
		await middleware.Invoke(context);

		// Assert
		Assert.Equal(200, context.Response.StatusCode);
	}

	[Fact]
	public async Task Middleware_Returns429WhenRateLimitExceeded()
	{
		// Arrange
		string userId_cacheKey = "FBF26574-9DB6-4C85-CF68-08DBDF13762C";
		DefaultHttpContext context = new();
		context.Request.RouteValues.Add("userId", userId_cacheKey);

		Mock<IMemoryCache> memoryCache = MockMemoryCacheService.GetMemoryCache(userId_cacheKey, DateTime.UtcNow.AddSeconds(5));
		RateLimitMiddleware middleware = new((innerHttpContext) => Task.FromResult(0), memoryCache.Object);

		// Act
		await middleware.Invoke(context);

		// Assert
		Assert.Equal(429, context.Response.StatusCode);
	}
}