using Microsoft.AspNetCore.Authorization;

namespace BlazorWebAppAuthorization.Policies.Attributes;

internal class MinimumAgeAuthorizeAttribute : AuthorizeAttribute
{
    private const string PolicyPrefix = "MinimumAge";

    public MinimumAgeAuthorizeAttribute(int age) => Age = age;

    public int Age
    {
        get
        {
            if (!string.IsNullOrEmpty(Policy) &&
                Policy.StartsWith(PolicyPrefix, 
                    StringComparison.OrdinalIgnoreCase) &&
                int.TryParse(Policy.AsSpan(PolicyPrefix.Length), out var age))
            {
                return age;
            }

            return default;
        }
        set
        {
            ArgumentOutOfRangeException.ThrowIfNegative(value);
            Policy = $"{PolicyPrefix}{value}";
        }
    }
}
