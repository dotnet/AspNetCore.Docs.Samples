using BlazorWebAppAuthorization.Models;
using Microsoft.AspNetCore.Authorization;

namespace BlazorWebAppAuthorization.Policies.Handlers;

#region snippet_HandlerAndRequirement
public class DocumentAuthorizationHandler :
    AuthorizationHandler<SameAuthorRequirement, Document>
{
    protected override Task HandleRequirementAsync(AuthorizationHandlerContext context,
        SameAuthorRequirement requirement, Document resource)
    {
        if (context.User.Identity?.Name == resource.Author)
        {
            context.Succeed(requirement);
        }

        return Task.CompletedTask;
    }
}

public class SameAuthorRequirement : IAuthorizationRequirement { }
#endregion
