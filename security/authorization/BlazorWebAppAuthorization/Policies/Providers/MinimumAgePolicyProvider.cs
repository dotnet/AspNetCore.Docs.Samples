using BlazorWebAppAuthorization.Policies.Requirements;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;

namespace BlazorWebAppAuthorization.Policies.Providers;

public class MinimumAgePolicyProvider(IOptions<AuthorizationOptions> options) 
    : IAuthorizationPolicyProvider
{
    private const string PolicyPrefix = "MinimumAge";

    private DefaultAuthorizationPolicyProvider DefaultPolicyProvider { get; } = 
        new(options);

    public Task<AuthorizationPolicy?> GetPolicyAsync(string policyName)
    {
        if (policyName.StartsWith(
                PolicyPrefix, StringComparison.OrdinalIgnoreCase) &&
            int.TryParse(policyName.AsSpan(PolicyPrefix.Length), out var age) &&
            age >= 0)
        {
            var policy = new AuthorizationPolicyBuilder(
                IdentityConstants.ApplicationScheme);
            policy.AddRequirements(new MinimumAgeRequirement(age));

            return Task.FromResult<AuthorizationPolicy?>(policy.Build());
        }

        return DefaultPolicyProvider.GetPolicyAsync(policyName);
    }

    public Task<AuthorizationPolicy> GetDefaultPolicyAsync() => 
        DefaultPolicyProvider.GetDefaultPolicyAsync();

    public Task<AuthorizationPolicy?> GetFallbackPolicyAsync() => 
        DefaultPolicyProvider.GetFallbackPolicyAsync();
}
