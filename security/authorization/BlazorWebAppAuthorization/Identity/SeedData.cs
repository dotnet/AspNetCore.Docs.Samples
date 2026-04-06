using System.Security.Claims;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using BlazorWebAppAuthorization.Data;

namespace BlazorWebAppAuthorization.Identity;

public class SeedData
{
    private static readonly IEnumerable<SeedUser> seedUsers =
    [
        new SeedUser()
        {
            Email = "leela@contoso.com",
            NormalizedEmail = "LEELA@CONTOSO.COM",
            NormalizedUserName = "LEELA@CONTOSO.COM",
            RoleList = [ "Admin", "SuperUser" ],
            Claims = [
                new("EmployeeNumber", "1"),
                new("Department", "Customer Service"),
                new("Department", "Human Resources")
            ],
            UserName = "leela@contoso.com"
        },
        new SeedUser()
        {
            Email = "harry@contoso.com",
            NormalizedEmail = "HARRY@CONTOSO.COM",
            NormalizedUserName = "HARRY@CONTOSO.COM",
            RoleList = [ "Admin" ],
            Claims = [
                new("EmployeeNumber", "10"),
                new("Department", "Customer Service")
            ],
            UserName = "harry@contoso.com"
        },
        new SeedUser()
        {
            Email = "sarah@contoso.com",
            NormalizedEmail = "SARAH@CONTOSO.COM",
            NormalizedUserName = "SARAH@CONTOSO.COM",
            RoleList = [ "SuperUser" ],
            UserName = "sarah@contoso.com"
        },
    ];

    public static async Task InitializeAsync(IServiceProvider serviceProvider)
    {
        using var context = new ApplicationDbContext(serviceProvider.GetRequiredService<DbContextOptions<ApplicationDbContext>>());

        if (context.Users.Any())
        {
            return;
        }

        var userStore = new UserStore<SeedUser>(context);
        var password = new PasswordHasher<SeedUser>();

        using var roleManager = serviceProvider.GetRequiredService<RoleManager<IdentityRole>>();

        string[] roles = [ "Admin", "SuperUser" ];

        foreach (var role in roles)
        {
            if (!await roleManager.RoleExistsAsync(role))
            {
                await roleManager.CreateAsync(new IdentityRole(role));
            }
        }

        using var userManager = serviceProvider.GetRequiredService<UserManager<ApplicationUser>>();

        foreach (var user in seedUsers)
        {
            var hashed = password.HashPassword(user, "Passw0rd!");
            user.PasswordHash = hashed;
            await userStore.CreateAsync(user);

            if (user.Email is not null)
            {
                var appUser = await userManager.FindByEmailAsync(user.Email);

                if (appUser is not null)
                { 
                    if (user.RoleList is not null)
                    {
                        await userManager.AddToRolesAsync(appUser, user.RoleList);
                    }

                    if (user.Claims is not null)
                    {
                        foreach (var claim in user.Claims)
                        {
                            await userManager.AddClaimAsync(appUser, new Claim(claim.Key, claim.Value));
                        }
                    }
                }
            }
        }

        await context.SaveChangesAsync();
    }

    private class SeedUser : ApplicationUser
    {
        public string[]? RoleList { get; set; }
        public List<KeyValuePair<string, string>>? Claims { get; set; }
    }
}
