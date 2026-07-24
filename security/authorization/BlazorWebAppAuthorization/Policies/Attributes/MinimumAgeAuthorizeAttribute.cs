using Microsoft.AspNetCore.Authorization;

namespace BlazorWebAppAuthorization.Policies.Attributes;

internal class MinimumAgeAuthorizeAttribute : AuthorizeAttribute
{
    const string POLICY_PREFIX = "MinimumAge";

    public MinimumAgeAuthorizeAttribute(int age) => Age = age;

    public int Age
    {
        get
        {
            if (!string.IsNullOrEmpty(Policy) &&
                Policy.StartsWith(POLICY_PREFIX, 
                    System.StringComparison.OrdinalIgnoreCase) &&
                int.TryParse(Policy.AsSpan(POLICY_PREFIX.Length), out var age))
            {
                return age;
            }

            return default;
        }
        set
        {
            Policy = $"{POLICY_PREFIX}{value}";
        }
    }
}
