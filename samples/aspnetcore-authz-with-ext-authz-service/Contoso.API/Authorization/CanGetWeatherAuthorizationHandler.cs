using Microsoft.AspNetCore.Authorization;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Contoso.API.Authorization;

public class SecurityPolicy
{
    [JsonPropertyName("canGetWeather")]
    public bool CanGetWeather { get; set; }
}

public class CanGetWeatherAuthorizationHandler : AuthorizationHandler<CanGetWeatherRequirement>
{
    private readonly IHttpClientFactory _httpClientFactory;

    public CanGetWeatherAuthorizationHandler(IHttpClientFactory httpClientFactory)
    {
        _httpClientFactory = httpClientFactory;
    }

    protected override async Task HandleRequirementAsync(AuthorizationHandlerContext context, CanGetWeatherRequirement requirement)
    {
        var userId = context.User.Identities.First();

        if (userId.IsAuthenticated)
        {
            var appid = userId.Claims.SingleOrDefault(x => x.Type == "appid");
            if (appid != null)
            {
                var value = appid.Value;

                var client = _httpClientFactory.CreateClient(AppConstants.SecurityAPIClient);
                var response = await client.GetAsync($"SecurityPolicy/{value}");

                if (response.IsSuccessStatusCode)
                {
                    var content = await response.Content.ReadAsStringAsync();
                    var policy = JsonSerializer.Deserialize<SecurityPolicy>(content);

                    if (policy != null && policy.CanGetWeather) context.Succeed(requirement);
                }
            }
        }
    }
}

