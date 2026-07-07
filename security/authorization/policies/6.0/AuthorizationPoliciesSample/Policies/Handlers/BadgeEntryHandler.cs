using Microsoft.AspNetCore.Authorization;

public class BadgeEntryHandler : AuthorizationHandler<BuildingEntryRequirement>
{
    protected override Task HandleRequirementAsync(
        AuthorizationHandlerContext context, BuildingEntryRequirement requirement)
    {
        if (context.User.HasClaim(c => 
            c.Type == "BadgeId" && 
            c.Issuer == "https://contososecurity"))
        {
            context.Succeed(requirement);
        }

        return Task.CompletedTask;
    }
}
