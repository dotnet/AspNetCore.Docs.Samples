using BlazorWebAppMovies.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace BlazorWebAppMovies.Data;

public class SeedData
{
    private static readonly IEnumerable<SeedUser> seedUsers =
    [
        new SeedUser()
        {
            Email = "leela@contoso.com",
            NormalizedEmail = "LEELA@CONTOSO.COM",
            NormalizedUserName = "LEELA@CONTOSO.COM",
            Roles = [ "Administrator" ],
            Claims = [],
            UserName = "leela@contoso.com"
        },
        new SeedUser()
        {
            Email = "harry@contoso.com",
            NormalizedEmail = "HARRY@CONTOSO.COM",
            NormalizedUserName = "HARRY@CONTOSO.COM",
            Roles = [ "Manager" ],
            Claims = [],
            UserName = "harry@contoso.com"
        },
    ];

    public static async Task Initialize(IServiceProvider serviceProvider)
    {
        using var context = new BlazorWebAppMoviesContext(
            serviceProvider.GetRequiredService<
                DbContextOptions<BlazorWebAppMoviesContext>>());

        if (context == null || context.Movie == null)
        {
            throw new NullReferenceException(
                "Null BlazorWebAppMoviesContext or Movie DbSet");
        }

        if (context.Movie.Any())
        {
            return;
        }

        context.Movie.AddRange(
            new Movie
            {
                Title = "Mad Max",
                ReleaseDate = new DateOnly(1979, 4, 12),
                Genre = "Sci-fi (Cyberpunk)",
                Price = 2.51M,
                Rating = "R",
            },
            new Movie
            {
                Title = "The Road Warrior",
                ReleaseDate = new DateOnly(1981, 12, 24),
                Genre = "Sci-fi (Cyberpunk)",
                Price = 2.78M,
                Rating = "R",
            },
            new Movie
            {
                Title = "Mad Max: Beyond Thunderdome",
                ReleaseDate = new DateOnly(1985, 7, 10),
                Genre = "Sci-fi (Cyberpunk)",
                Price = 3.55M,
                Rating = "PG-13",
            },
            new Movie
            {
                Title = "Mad Max: Fury Road",
                ReleaseDate = new DateOnly(2015, 5, 15),
                Genre = "Sci-fi (Cyberpunk)",
                Price = 8.43M,
                Rating = "R",
            },
            new Movie
            {
                Title = "Furiosa: A Mad Max Saga",
                ReleaseDate = new DateOnly(2024, 5, 24),
                Genre = "Sci-fi (Cyberpunk)",
                Price = 13.49M,
                Rating = "R",
            });

        var userStore = new UserStore<SeedUser>(context);
        var password = new PasswordHasher<SeedUser>();

        using var roleManager = serviceProvider.GetRequiredService<RoleManager<IdentityRole>>();

        string[] roles = ["Administrator", "Manager"];

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
                    if (user.Roles is not null)
                    {
                        await userManager.AddToRolesAsync(appUser, user.Roles);
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

        context.SaveChanges();
    }

    private class SeedUser : ApplicationUser
    {
        public string[]? Roles { get; set; }
        public List<KeyValuePair<string, string>>? Claims { get; set; }
    }
}
