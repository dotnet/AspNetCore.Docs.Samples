using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using BlazorWebAppAuthorization.Policies.Requirements;

namespace BlazorWebAppAuthorization.Policies.Providers;

internal class MinimumAgePolicyProvider : IAuthorizationPolicyProvider
{
    private readonly DefaultAuthorizationPolicyProvider _fallbackPolicyProvider;
    const string POLICY_PREFIX = "MinimumAge";

    public MinimumAgePolicyProvider(Microsoft.Extensions.Options.IOptions<AuthorizationOptions> options) =>
        _fallbackPolicyProvider = new DefaultAuthorizationPolicyProvider(options);

    public Task<AuthorizationPolicy?> GetPolicyAsync(string policyName)
    {
        if (policyName.StartsWith(POLICY_PREFIX, StringComparison.OrdinalIgnoreCase) &&
            int.TryParse(policyName.AsSpan(POLICY_PREFIX.Length), out var age) &&
            age >= 0)
        {
            var policy = new AuthorizationPolicyBuilder(
                IdentityConstants.ApplicationScheme);
            policy.AddRequirements(new MinimumAgeRequirement(age));

            return Task.FromResult<AuthorizationPolicy?>(policy.Build());
        }

        return Task.FromResult<AuthorizationPolicy?>(null);
    }

    public Task<AuthorizationPolicy> GetDefaultPolicyAsync() =>
        Task.FromResult<AuthorizationPolicy>(
            new AuthorizationPolicyBuilder(
                IdentityConstants.ApplicationScheme)
            .RequireAuthenticatedUser().Build());

    public Task<AuthorizationPolicy?> GetFallbackPolicyAsync() =>
        Task.FromResult<AuthorizationPolicy?>(null);
}
