using Microsoft.AspNetCore.Authorization;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Contoso.API.Authorization;

public class SecurityPolicy
{
    [JsonPropertyName("canGetWeather")]
    public bool CanGetWeather { get; set; }
}

public class CanGetWeatherAuthorizationHandler(IHttpClientFactory httpClientFactory) 
    : AuthorizationHandler<CanGetWeatherRequirement>
{
    protected override async Task HandleRequirementAsync(
        AuthorizationHandlerContext context, CanGetWeatherRequirement requirement)
    {
        var userId = context.User.Identities.First();

        if (userId.IsAuthenticated)
        {
            var appid = userId.Claims.SingleOrDefault(x => x.Type == "appid");

            if (appid is not null)
            {
                var value = appid.Value;
                var client = 
                    httpClientFactory.CreateClient(AppConstants.SecurityAPIClient);
                var response = await client.GetAsync($"SecurityPolicy/{value}");

                if (response.IsSuccessStatusCode)
                {
                    var content = await response.Content.ReadAsStringAsync();
                    var policy = JsonSerializer.Deserialize<SecurityPolicy>(content);

                    if (policy is not null && policy.CanGetWeather)
                    {
                        context.Succeed(requirement);
                    }
                }
            }
        }
    }
}

