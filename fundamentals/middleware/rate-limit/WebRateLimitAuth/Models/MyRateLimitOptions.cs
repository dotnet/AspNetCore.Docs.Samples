namespace WebRateLimitAuth.Models;

public class MyRateLimitOptions
{
    public const string MyRateLimit = "MyRateLimit";

    public int permitLimit { get; set; } = 100;
    public int window { get; set; } = 10;
    public int replenishmentPeriod { get; set; } = 2;
    public int queueLimit { get; set; } = 100;
    public int segmentsPerWindow { get; set; } = 8;
    public int tokenLimit { get; set; } = 10;
    public int tokenLimit2 { get; set; } = 20;
    public int tokensPerPeriod { get; set; } = 4;
    public bool autoReplenishment { get; set; } = false;
}
