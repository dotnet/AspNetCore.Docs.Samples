#define ADMIN // FIRST ADMIN FIXED SLIDING CONCUR TOKEN FIXED2
#if NEVER
#elif FIXED
// <snippet_fixed>
using Microsoft.AspNetCore.RateLimiting;
using System.Threading.RateLimiting;

var builder = WebApplication.CreateBuilder(args);
var app = builder.Build();

static string GetTicks() => (DateTime.Now.Ticks & 0x1111).ToString("0000");

app.UseRateLimiter(new RateLimiterOptions()
    .AddFixedWindowLimiter(policyName: "fixed",
          new FixedWindowRateLimiterOptions(permitLimit: 4,
          window: TimeSpan.FromSeconds(12),
          queueProcessingOrder: QueueProcessingOrder.OldestFirst,
          queueLimit: 2)));

app.MapGet("/", () => Results.Ok($"Hello {GetTicks()}"))
                           .RequireRateLimiting(fixedPolicy);

app.Run();
// </snippet_fixed>
#elif FIXED2
// <snippet_fixed2>
using System.Threading.RateLimiting;
using Microsoft.AspNetCore.RateLimiting;
using WebRateLimitAuth.Models;

var builder = WebApplication.CreateBuilder(args);
builder.Services.Configure<MyRateLimitOptions>(
    builder.Configuration.GetSection(MyRateLimitOptions.MyRateLimit));
var app = builder.Build();

static string GetTicks() => (DateTime.Now.Ticks & 0x1111).ToString("0000");

var myOptions = new MyRateLimitOptions();
app.Configuration.GetSection(MyRateLimitOptions.MyRateLimit).Bind(myOptions);
var fixedPolicy = "fixed";

app.UseRateLimiter(new RateLimiterOptions()
    .AddFixedWindowLimiter(policyName: fixedPolicy,
          new FixedWindowRateLimiterOptions(permitLimit: myOptions.permitLimit,
          window: TimeSpan.FromSeconds(myOptions.window),
          queueProcessingOrder: QueueProcessingOrder.OldestFirst,
          queueLimit: myOptions.queueLimit)));

app.MapGet("/", () => Results.Ok($"Hello {GetTicks()}"))
                           .RequireRateLimiting(fixedPolicy);

app.Run();
// </snippet_fixed2>
#elif SLIDING
// <snippet_slide>
using Microsoft.AspNetCore.RateLimiting;
using System.Threading.RateLimiting;
using WebRateLimitAuth.Models;

var builder = WebApplication.CreateBuilder(args);
var app = builder.Build();

static string GetTicks() => (DateTime.Now.Ticks & 0x1111).ToString("0000");

var myOptions = new MyRateLimitOptions();
app.Configuration.GetSection(MyRateLimitOptions.MyRateLimit).Bind(myOptions);
var slidingPolicy = "sliding";

app.UseRateLimiter(new RateLimiterOptions()
    .AddSlidingWindowLimiter(policyName: slidingPolicy,
          new SlidingWindowRateLimiterOptions(permitLimit: myOptions.permitLimit,
          window: TimeSpan.FromSeconds(myOptions.window),
          segmentsPerWindow: myOptions.segmentsPerWindow,
          queueProcessingOrder: QueueProcessingOrder.OldestFirst,
          queueLimit: myOptions.queueLimit)));

app.MapGet("/", () => Results.Ok($"Hello {GetTicks()}"))
                           .RequireRateLimiting(slidingPolicy);

app.Run();
// </snippet_slide>
#elif CONCUR
// Quicktest 10 users, 9 seconds -> 982 requests, 900 errors
// <snippet_concur>
using Microsoft.AspNetCore.RateLimiting;
using System.Threading.RateLimiting;
using WebRateLimitAuth.Models;

var builder = WebApplication.CreateBuilder(args);
var app = builder.Build();

static string GetTicks() => (DateTime.Now.Ticks & 0x1111).ToString("0000");

var concurrencyPolicy = "Concurrency";
var myOptions = new MyRateLimitOptions();
app.Configuration.GetSection(MyRateLimitOptions.MyRateLimit).Bind(myOptions);

app.UseRateLimiter(new RateLimiterOptions()
    .AddConcurrencyLimiter(policyName: concurrencyPolicy,
          new ConcurrencyLimiterOptions(permitLimit: myOptions.permitLimit,
          queueProcessingOrder: QueueProcessingOrder.OldestFirst,
          queueLimit: myOptions.queueLimit)));

app.MapGet("/", async () =>
{
    await Task.Delay(500);
    return Results.Ok($"Concurrency Limiter {GetTicks()}");
                              
}).RequireRateLimiting(concurrencyPolicy);

app.Run();
// </snippet_token>
#elif TOKEN
// Quicktest 20 users, 20 seconds -> 8965 requests 2,250 errors
// <snippet_concur>
using Microsoft.AspNetCore.RateLimiting;
using System.Threading.RateLimiting;
using WebRateLimitAuth.Models;

var builder = WebApplication.CreateBuilder(args);
var app = builder.Build();

static string GetTicks() => (DateTime.Now.Ticks & 0x1111).ToString("0000");

var tokenPolicy = "token";
var myOptions = new MyRateLimitOptions();
app.Configuration.GetSection(MyRateLimitOptions.MyRateLimit).Bind(myOptions);

app.UseRateLimiter(new RateLimiterOptions()
    .AddTokenBucketLimiter(policyName: tokenPolicy,
          new TokenBucketRateLimiterOptions(tokenLimit: myOptions.tokenLimit,
                     queueProcessingOrder: QueueProcessingOrder.OldestFirst,
                     queueLimit: myOptions.queueLimit,
                     replenishmentPeriod: TimeSpan.FromSeconds(2),
                     tokensPerPeriod: myOptions.tokensPerPeriod,
                     autoReplenishment: myOptions.autoReplenishment)));

app.MapGet("/", () => Results.Ok($"Token Limiter {GetTicks()}"))
                           .RequireRateLimiting(tokenPolicy);

app.Run();
// </snippet_token>
#elif FIRST
// <snippet_1>
using System.Globalization;
using System.Net;
using System.Threading.RateLimiting;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.RateLimiting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging.Abstractions;
using WebRateLimitAuth;
using WebRateLimitAuth.Data;
using WebRateLimitAuth.Models;

var builder = WebApplication.CreateBuilder(args);

var connectionString = builder.Configuration.GetConnectionString("DefaultConnection") ??
    throw new InvalidOperationException("Connection string 'DefaultConnection' not found.");
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(connectionString));
builder.Services.AddDatabaseDeveloperPageExceptionFilter();

builder.Services.AddDefaultIdentity<IdentityUser>(options => options.SignIn.RequireConfirmedAccount = true)
    .AddEntityFrameworkStores<ApplicationDbContext>();

builder.Services.AddRazorPages();
builder.Services.AddControllersWithViews();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseMigrationsEndPoint();
}
else
{
    app.UseExceptionHandler("/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

// <snippet>
app.UseAuthentication();
app.UseAuthorization();

var userPolicyName = "user";
var completePolicyName = "complete";
var helloPolicy = "hello";
var myOptions = new MyRateLimitOptions();
app.Configuration.GetSection(MyRateLimitOptions.MyRateLimit).Bind(myOptions);

var options = new RateLimiterOptions()
{
    OnRejected = (context, cancellationToken) =>
    {
        if (context.Lease.TryGetMetadata(MetadataName.RetryAfter, out var retryAfter))
        {
            context.HttpContext.Response.Headers.RetryAfter =
            ((int)retryAfter.TotalSeconds).ToString(NumberFormatInfo.InvariantInfo);
        }

        context.HttpContext.Response.StatusCode = StatusCodes.Status429TooManyRequests;
        context?.HttpContext?.RequestServices?.GetService<ILoggerFactory>()?
                      .CreateLogger("Microsoft.AspNetCore.RateLimitingMiddleware")
                      .LogWarning($"OnRejected: {GetUserEndPoint(context.HttpContext)}");

        return new ValueTask();
    }
}
    .AddPolicy<string>(completePolicyName, 
               new SampleRateLimiterPolicy(NullLogger<SampleRateLimiterPolicy>.Instance))
    .AddPolicy<string, SampleRateLimiterPolicy>(helloPolicy)
    .AddPolicy<string>(userPolicyName, context =>
    {
        if (context.User?.Identity?.IsAuthenticated is not true)
        {
            var username = "anonymous user";

            return RateLimitPartition.CreateSlidingWindowLimiter<string>(username,
                  key => new SlidingWindowRateLimiterOptions(
                  permitLimit: myOptions.permitLimit,
                  queueProcessingOrder: QueueProcessingOrder.OldestFirst,
                  queueLimit: myOptions.queueLimit,
                  window: TimeSpan.FromSeconds(myOptions.window),
                  segmentsPerWindow: myOptions.segmentsPerWindow
                ));
        }
        else
        {
            return RateLimitPartition.CreateNoLimiter<string>(string.Empty);
        }
    });

options.GlobalLimiter = PartitionedRateLimiter.Create<HttpContext, IPAddress>(context =>
{
    IPAddress? remoteIPaddress = context?.Connection?.RemoteIpAddress;

    if (!IPAddress.IsLoopback(remoteIPaddress!))
    {
        return RateLimitPartition.CreateTokenBucketLimiter<IPAddress>
           (remoteIPaddress!, key =>
                 new TokenBucketRateLimiterOptions(tokenLimit: myOptions.tokenLimit2,
                     queueProcessingOrder: QueueProcessingOrder.OldestFirst,
                     queueLimit: myOptions.queueLimit,
                     replenishmentPeriod: TimeSpan.FromSeconds(myOptions.replenishmentPeriod),
                     tokensPerPeriod: myOptions.tokensPerPeriod,
                     autoReplenishment: myOptions.autoReplenishment));
    }
    else
    {
        return RateLimitPartition.CreateNoLimiter<IPAddress>(IPAddress.Loopback);
    }
});

app.UseRateLimiter(options);

app.MapRazorPages().RequireRateLimiting(userPolicyName);
app.MapDefaultControllerRoute();

static string GetUserEndPoint(HttpContext context) =>
    $"User {context.User?.Identity?.Name ?? "Anonymous"}  endpoint: {context.Request.Path}" +
    $" {context.Connection.RemoteIpAddress}";
static string GetTicks() => (DateTime.Now.Ticks & 0x1111).ToString("0000");

app.MapGet("/a", (HttpContext context) => $"{GetUserEndPoint(context)} {GetTicks()}")
    .RequireRateLimiting(userPolicyName);

app.MapGet("/b", (HttpContext context) => $"{GetUserEndPoint(context)} {GetTicks()}")
    .RequireRateLimiting(completePolicyName);

app.MapGet("/c", (HttpContext context) => $"{GetUserEndPoint(context)} {GetTicks()}")
    .RequireRateLimiting(helloPolicy);

app.MapGet("/d", (HttpContext context) => $"{GetUserEndPoint(context)} {GetTicks()}");

app.Run();
// </snippet>
// </snippet_1>
#elif ADMIN
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.RateLimiting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Primitives;
using System.Threading.RateLimiting;
using WebRateLimitAuth.Data;
using WebRateLimitAuth.Models;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddAuthorization(o => o.AddPolicy("AdminsOnly",
                                  b => b.RequireClaim("admin", "true")));

var connectionString = builder.Configuration.GetConnectionString("DefaultConnection") 
    ?? throw new InvalidOperationException("Connection string 'DefaultConnection' not found.");
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(connectionString));
builder.Services.AddDatabaseDeveloperPageExceptionFilter();

builder.Services.AddDefaultIdentity<IdentityUser>(options => 
                  options.SignIn.RequireConfirmedAccount = true)
    .AddEntityFrameworkStores<ApplicationDbContext>();

builder.Services.AddRazorPages();
builder.Services.AddControllersWithViews();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseMigrationsEndPoint();
}
else
{
    app.UseExceptionHandler("/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

// <snippet_adm>
app.UseAuthentication();
app.UseAuthorization();

var getPolicyName = "get";
var adminPolicyName = "admin";
var postPolicyName = "post";
var myOptions = new MyRateLimitOptions();
app.Configuration.GetSection(MyRateLimitOptions.MyRateLimit).Bind(myOptions);

app.UseRateLimiter(new RateLimiterOptions()
    .AddConcurrencyLimiter(policyName: getPolicyName,
          new ConcurrencyLimiterOptions(permitLimit: myOptions.permitLimit,
          queueProcessingOrder: QueueProcessingOrder.OldestFirst,          
          queueLimit: myOptions.queueLimit))
    .AddNoLimiter(policyName: adminPolicyName)
    .AddPolicy(policyName: postPolicyName, partitioner: httpContext =>
    {
        var accessToken = httpContext?.Features?.Get<IAuthenticateResultFeature>()?
        .AuthenticateResult?.Properties?.GetTokenValue("access_token")?.ToString()
                                                                   ?? string.Empty;
        if (!StringValues.IsNullOrEmpty(accessToken))
        {
            return RateLimitPartition.CreateTokenBucketLimiter( accessToken, key =>
                new TokenBucketRateLimiterOptions(tokenLimit: myOptions.tokenLimit2,
                    queueProcessingOrder: QueueProcessingOrder.OldestFirst,
                    queueLimit: myOptions.queueLimit,
                    replenishmentPeriod: TimeSpan.FromSeconds(myOptions.replenishmentPeriod),
                    tokensPerPeriod: myOptions.tokensPerPeriod,                    
                    autoReplenishment: myOptions.autoReplenishment));
        }
        else
        {
            return RateLimitPartition.CreateTokenBucketLimiter("Anon", key =>
                new TokenBucketRateLimiterOptions(tokenLimit: myOptions.tokenLimit,
                    queueProcessingOrder: QueueProcessingOrder.OldestFirst,
                    queueLimit: myOptions.queueLimit,
                    replenishmentPeriod: TimeSpan.FromSeconds(myOptions.replenishmentPeriod),
                    tokensPerPeriod: myOptions.tokensPerPeriod,
                    autoReplenishment: true));
        }
    }));

static string GetUserEndPointMethod(HttpContext context) =>
    $"Hello {context.User?.Identity?.Name ?? "Anonymous"} " +
    $"Endpoint:{context.Request.Path} Method: {context.Request.Method}";


app.MapGet("/test", (HttpContext context) => $"{GetUserEndPointMethod(context)}")
                                        .RequireRateLimiting(getPolicyName);

app.MapGet("/admin", context => context.Response.WriteAsync("/admin"))
                          .RequireRateLimiting(adminPolicyName)
                          .RequireAuthorization("AdminsOnly");

app.MapPost("/post", () => Results.Ok("/post"))
                           .RequireRateLimiting(postPolicyName);

app.MapRazorPages().RequireRateLimiting(getPolicyName)
                   .RequireRateLimiting(postPolicyName);

app.MapDefaultControllerRoute();

app.Run();
// </snippet_adm>
#endif
