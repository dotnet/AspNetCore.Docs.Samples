using Microsoft.AspNetCore.Authorization;

namespace BlazorWebAppAuthorization.Policies.Requirements;

public class MinimumAgeRequirement(int minimumAge) : IAuthorizationRequirement
{
    public int MinimumAge { get; } = minimumAge;
}
