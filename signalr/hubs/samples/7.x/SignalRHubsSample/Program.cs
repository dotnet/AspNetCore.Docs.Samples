using SignalRHubsSample.Hubs;

// <snippet_AddSignalR>
var builder = WebApplication.CreateBuilder(args);

builder.Services.AddRazorPages();
builder.Services.AddSignalR();
// </snippet_AddSignalR>

var app = builder.Build();

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

// <snippet_MapHub>
app.MapRazorPages();
app.MapHub<ChatHub>("/Chat");

app.Run();
// </snippet_MapHub>
