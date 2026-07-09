using Microsoft.AspNetCore.Authorization;
using BlazorWebAppAuthorization.Policies.Requirements;

namespace BlazorWebAppAuthorization.Policies.Handlers;

public class BadgeEntryHandler : AuthorizationHandler<BuildingEntryRequirement>
{
    protected override Task HandleRequirementAsync(
        AuthorizationHandlerContext context, BuildingEntryRequirement requirement)
    {
        if (context.User.HasClaim(c => c.Type == "BadgeId"))
        {
            context.Succeed(requirement);
        }

        return Task.CompletedTask;
    }
}
