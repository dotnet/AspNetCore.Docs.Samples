using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authorization.Infrastructure;
using BlazorWebAppAuthorization.Models;

namespace BlazorWebAppAuthorization.Services;

public class DocumentAuthorizationCrudHandler :
    AuthorizationHandler<OperationAuthorizationRequirement, Document>
{
    protected override Task HandleRequirementAsync(AuthorizationHandlerContext context,
        OperationAuthorizationRequirement requirement, Document resource)
    {
        if (requirement.Name == Operations.Create.Name &&
            context.User.IsInRole("Admin"))
        {
            context.Succeed(requirement);
        }

        if (requirement.Name == Operations.Delete.Name &&
            context.User.IsInRole("SuperUser"))
        {
            context.Succeed(requirement);
        }

        if (requirement.Name == Operations.Read.Name)
        {
            context.Succeed(requirement);
        }

        if (requirement.Name == Operations.Update.Name &&
            context.User.IsInRole("Admin"))
        {
            context.Succeed(requirement);
        }

        return Task.CompletedTask;
    }
}

public static class Operations
{
    public static readonly OperationAuthorizationRequirement Create =
        new() { Name = nameof(Create) };
    public static readonly OperationAuthorizationRequirement Delete =
        new() { Name = nameof(Delete) };
    public static readonly OperationAuthorizationRequirement Read =
        new() { Name = nameof(Read) };
    public static readonly OperationAuthorizationRequirement Update =
        new() { Name = nameof(Update) };
}
