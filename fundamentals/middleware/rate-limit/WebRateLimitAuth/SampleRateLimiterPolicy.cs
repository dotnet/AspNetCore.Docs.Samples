using System.Threading.RateLimiting;
using Microsoft.AspNetCore.RateLimiting;

namespace WebRateLimitAuth;

public class SampleRateLimiterPolicy : IRateLimiterPolicy<string>
{
    private Func<OnRejectedContext, CancellationToken, ValueTask>? _onRejected;

    public SampleRateLimiterPolicy(ILogger<SampleRateLimiterPolicy> logger)
    {
        _onRejected = (context, token) =>
        {
            context.HttpContext.Response.StatusCode = 429;
            logger.LogWarning($"Request rejected by {nameof(SampleRateLimiterPolicy)}");
            return ValueTask.CompletedTask;
        };
    }

    public Func<OnRejectedContext, CancellationToken, ValueTask>? 
                                                         OnRejected { get => _onRejected; }

    public RateLimitPartition<string> GetPartition(HttpContext httpContext)
    {
        return RateLimitPartition.CreateSlidingWindowLimiter<string>(string.Empty, 
            key => new SlidingWindowRateLimiterOptions(
                    permitLimit: 1,
                    queueProcessingOrder: QueueProcessingOrder.OldestFirst,
                    queueLimit: 2,
                    window: TimeSpan.FromSeconds(5),
                    segmentsPerWindow: 1));
    }
}
