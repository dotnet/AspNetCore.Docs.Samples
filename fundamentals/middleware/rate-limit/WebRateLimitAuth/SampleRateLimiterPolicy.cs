// <snippet_1>
using System.Threading.RateLimiting;
using Microsoft.AspNetCore.RateLimiting;
using Microsoft.Extensions.Options;
using WebRateLimitAuth.Models;

namespace WebRateLimitAuth;

// <snippet>
public class SampleRateLimiterPolicy : IRateLimiterPolicy<string>
{
    private Func<OnRejectedContext, CancellationToken, ValueTask>? _onRejected;
    private readonly MyRateLimitOptions _options;

    public SampleRateLimiterPolicy(ILogger<SampleRateLimiterPolicy> logger,
                                   IOptions<MyRateLimitOptions> options)
    {
        _onRejected = (context, token) =>
        {
            context.HttpContext.Response.StatusCode = StatusCodes.Status429TooManyRequests;
            logger.LogWarning($"Request rejected by {nameof(SampleRateLimiterPolicy)}");
            return ValueTask.CompletedTask;
        };
        _options = options.Value;
    }

    public Func<OnRejectedContext, CancellationToken, ValueTask>? 
                                                         OnRejected { get => _onRejected; }
    // </snippet>

    public RateLimitPartition<string> GetPartition(HttpContext httpContext)
    {
        return RateLimitPartition.CreateSlidingWindowLimiter<string>(string.Empty, 
            key => new SlidingWindowRateLimiterOptions(
                    permitLimit: _options.permitLimit,
                    queueProcessingOrder: QueueProcessingOrder.OldestFirst,
                    queueLimit: _options.queueLimit,
                    window: TimeSpan.FromSeconds(_options.window),
                    segmentsPerWindow: _options.segmentsPerWindow));
    
    }
}
// </snippet_1>
