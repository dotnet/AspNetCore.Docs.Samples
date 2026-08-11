using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using BlazorWebAppAuthorization.Components;
using BlazorWebAppAuthorization.Components.Account;
using BlazorWebAppAuthorization.Data;
using BlazorWebAppAuthorization.Identity;
using BlazorWebAppAuthorization.Policies.Handlers;
using BlazorWebAppAuthorization.Policies.Providers;
using BlazorWebAppAuthorization.Policies.Requirements;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents();

builder.Services.AddCascadingAuthenticationState();
builder.Services.AddScoped<IdentityRedirectManager>();
builder.Services.AddScoped<AuthenticationStateProvider, IdentityRevalidatingAuthenticationStateProvider>();

builder.Services.AddAuthentication();

// Add the database (in memory for the sample).
builder.Services.AddDbContext<ApplicationDbContext>(
    options =>
    {
        options.UseInMemoryDatabase("AppDb");
        //For debugging only: options.EnableDetailedErrors(true);
        //For debugging only: options.EnableSensitiveDataLogging(true);
    });

// Add Identity services and configure to use the default UI and the database context.
builder.Services.AddDefaultIdentity<ApplicationUser>()
    .AddRoles<IdentityRole>()
    .AddEntityFrameworkStores<ApplicationDbContext>();

// Alternatively, Identity can be built up from the core services, which would look like this:
/*
builder.Services.AddIdentityCore<ApplicationUser>()
    .AddSignInManager()
    .AddUserManager<UserManager<ApplicationUser>>()
    .AddRoles<IdentityRole>()
    .AddEntityFrameworkStores<ApplicationDbContext>();
*/

// Application cookie redirect mapping
//
// Anonymous Users: When an unauthenticated user hits a protected endpoint, the cookie handler
// issues a challenge and redirects to 'LoginPath' with a 'ReturnUrl' parameter.
//
// Underage Users: When an authenticated user fails the custom minimum age policy, the handler
// issues a forbid response and redirects to 'AccessDeniedPath' instead of the login page.
builder.Services.ConfigureApplicationCookie(options =>
{
    options.LoginPath = "/Account/Login";
    options.AccessDeniedPath = "/Account/AccessDenied";
});

// Add the AtLeast21 authorization policy
builder.Services.AddAuthorizationBuilder()
    .AddPolicy("AtLeast21", policy =>
        policy.Requirements.Add(new MinimumAgeRequirement(21)));

// Add authorization policies for the Admin and SuperUser roles.
builder.Services.AddAuthorizationBuilder()
    .AddPolicy("RequireAdminRole",
         policy => policy.RequireRole("Admin"));

builder.Services.AddAuthorizationBuilder()
    .AddPolicy("RequireSuperUserRole",
         policy => policy.RequireRole("SuperUser"));

// Add authorization policies for the EmployeeNumber claim.
builder.Services.AddAuthorizationBuilder()
    .AddPolicy("EmployeeOnly", policy => policy.RequireClaim("EmployeeNumber"));

builder.Services.AddAuthorizationBuilder()
    .AddPolicy("Founder", policy =>
        policy.RequireClaim("EmployeeNumber", "1", "2", "3", "4", "5"));

// Add authorization policies for the Department claim, one requiring the user to be in
// the Customer Service department and the other requiring the user to be in the
// Human Resources department.
builder.Services.AddAuthorizationBuilder()
    .AddPolicy("CustomerServiceMember", policy =>
        policy.RequireClaim("Department", "Customer Service"))
    .AddPolicy("HumanResourcesMember", policy =>
        policy.RequireClaim("Department", "Human Resources"));

// Add services for resource-based authorization.
builder.Services.AddAuthorizationBuilder()
    .AddPolicy("SameAuthorPolicy", policy =>
        policy.Requirements.Add(new SameAuthorRequirement()));

builder.Services.AddSingleton<IAuthorizationHandler, DocumentAuthorizationHandler>();
builder.Services.AddSingleton<IAuthorizationHandler, DocumentAuthorizationCrudHandler>();
builder.Services.AddSingleton<IAuthorizationHandler, MinimumAgeHandler>();
builder.Services.AddScoped<IDocumentRepository, DocumentRepository>();
builder.Services.AddSingleton<IAuthorizationPolicyProvider, MinimumAgePolicyProvider>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    // seed the database
    await using var scope = app.Services.CreateAsyncScope();
    await SeedData.InitializeAsync(scope.ServiceProvider);
}
else
{
    app.UseExceptionHandler("/Error", createScopeForErrors: true);
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}
app.UseStatusCodePagesWithReExecute("/not-found", createScopeForStatusCodePages: true);
app.UseHttpsRedirection();

app.UseAntiforgery();

app.MapStaticAssets();
app.MapRazorComponents<App>()
    .AddInteractiveServerRenderMode();

// Add additional endpoints required by the Identity /Account Razor components.
app.MapAdditionalIdentityEndpoints();

app.Run();
