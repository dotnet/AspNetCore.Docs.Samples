using Microsoft.EntityFrameworkCore;
using Contacts.Data;
var builder = WebApplication.CreateBuilder(args);
builder.Services.AddDbContext<ModelStateErrorContext>(options =>
   options.UseInMemoryDatabase("Contacts"));

builder.Services.AddControllersWithViews();

var app = builder.Build();

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Contacts}/{action=Index}/{id?}");

app.Run();
