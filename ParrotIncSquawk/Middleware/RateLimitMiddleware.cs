using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Caching.Memory;
using System;
using System.Threading.Tasks;

namespace ParrotIncSquawk.Middleware;

public class RateLimitMiddleware
{
	private const int _totalSeconds = 20;
	private const string _userId = "userId";

	private readonly RequestDelegate _next;
	private readonly IMemoryCache _cache;

	public RateLimitMiddleware(RequestDelegate next, IMemoryCache cache)
	{
		_next = next;
		_cache = cache;
	}

	public async Task Invoke(HttpContext context)
	{
		string? userId = context?.Request?.RouteValues[_userId]?.ToString();

		if (string.IsNullOrEmpty(userId))
		{
			// 401 unauthorized
			await SetUpResponse(context, code: StatusCodes.Status401Unauthorized, text: "invalid user.");
			return;
		}

		if (_cache.TryGetValue(userId, out DateTime lastRequestTime))
		{
			TimeSpan timeElapsed = DateTime.UtcNow - lastRequestTime;

			if (timeElapsed.TotalSeconds < _totalSeconds)
			{
				// 429 Too Many Requests
				await SetUpResponse(context, code: StatusCodes.Status429TooManyRequests, text: "\"Rate limit exceeded. Please try again later.\"");
				return;
			}
		}

		_cache.Set(userId, DateTime.UtcNow, TimeSpan.FromSeconds(_totalSeconds));
		await _next(context);
	}

	private static async Task SetUpResponse(HttpContext context, int code, string text)
	{
		context.Response.StatusCode = code;
		await context.Response.WriteAsync(text);
	}
}
