using Microsoft.AspNetCore.Authorization;

namespace AuthRequirementsDataBWA.Authorization;

class MinimumAgeAttribute(int age) : Attribute,
    IAuthorizationRequirement, IAuthorizationRequirementData
{
    public int Age { get; set; } = age;

    public IEnumerable<IAuthorizationRequirement> GetRequirements()
    {
        yield return this;
    }
}
