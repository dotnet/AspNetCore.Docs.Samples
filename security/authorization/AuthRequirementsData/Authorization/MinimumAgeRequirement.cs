using Microsoft.AspNetCore.Authorization;

namespace AuthRequirementsData.Authorization;

class MinimumAgeRequirement(int age) : IAuthorizationRequirement
{
    public int Age { get; private set; } = age;
}
