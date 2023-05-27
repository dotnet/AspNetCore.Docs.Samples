using Microsoft.AspNetCore.Authorization;

namespace AuthRequirementsData.Authorization;

class MinimumAgeRequirement : IAuthorizationRequirement
{
    public int Age { get; private set; }

    public MinimumAgeRequirement(int age) { Age = age; }
}