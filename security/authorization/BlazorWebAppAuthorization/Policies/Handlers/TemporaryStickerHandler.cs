using Microsoft.AspNetCore.Authorization;
using BlazorWebAppAuthorization.Policies.Requirements;

namespace BlazorWebAppAuthorization.Policies.Handlers;

public class TemporaryStickerHandler : AuthorizationHandler<BuildingEntryRequirement>
{
    protected override Task HandleRequirementAsync(
        AuthorizationHandlerContext context, BuildingEntryRequirement requirement)
    {
        if (context.User.HasClaim(c => 
            c.Type == "TemporaryBadgeId" &&
            c.Issuer == "https://contososecurity"))
        {
            // Code to check expiration date omitted for brevity.
            context.Succeed(requirement);
        }

        return Task.CompletedTask;
    }
}
