using Microsoft.AspNetCore.Authorization;

namespace AuthRequirementsData.Authorization;

class MinimumAgeAttribute(int age) : Attribute,
    IAuthorizationRequirement, IAuthorizationRequirementData
{
    public int Age { get; set; } = age;

    public IEnumerable<IAuthorizationRequirement> GetRequirements()
    {
        yield return this;
    }
}
