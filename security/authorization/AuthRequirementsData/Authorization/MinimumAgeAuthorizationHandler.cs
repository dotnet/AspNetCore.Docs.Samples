using System.Globalization;
using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;

namespace AuthRequirementsData.Authorization;

class MinimumAgeAuthorizationHandler(ILogger<MinimumAgeAuthorizationHandler> logger) 
    : AuthorizationHandler<MinimumAgeAuthorizeAttribute>
{
    // Check whether a given minimum age requirement is satisfied.
    protected override Task HandleRequirementAsync(
        AuthorizationHandlerContext context, 
        MinimumAgeAuthorizeAttribute requirement)
    {
        logger.LogInformation(
            "Evaluating authorization requirement for age >= {age}", 
            requirement.Age);

        // Get the user's birth date claim.
        var dateOfBirthClaim = 
            context.User.FindFirst(c => c.Type == ClaimTypes.DateOfBirth);

        if (dateOfBirthClaim != null)
        {
            // If the user has a date of birth claim, obtain their age.
            var dateOfBirth = Convert.ToDateTime(dateOfBirthClaim.Value, 
                CultureInfo.InvariantCulture);
            var age = DateTime.Now.Year - dateOfBirth.Year;

            // Adjust age if the user hasn't had a birthday yet this year.
            if (dateOfBirth > DateTime.Now.AddYears(-age))
            {
                age--;
            }

            // If the user meets the age requirement, mark the authorization
            // requirement succeeded.
            if (age >= requirement.Age)
            {
                logger.LogInformation(
                    "Minimum age authorization requirement {age} satisfied", 
                    requirement.Age);
                context.Succeed(requirement);
            }
            else
            {
                logger.LogInformation(
                    "Current user's DateOfBirth claim ({dateOfBirth}) doesn't " +
                    "satisfy the minimum age authorization requirement {age}",
                    dateOfBirthClaim.Value,
                    requirement.Age);
            }
        }
        else
        {
            logger.LogInformation("No DateOfBirth claim present");
        }

        return Task.CompletedTask;
    }
}
