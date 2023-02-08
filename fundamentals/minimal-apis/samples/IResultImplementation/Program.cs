using IResultImplementation;
using IResultImplementation.Data;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddDbContext<IResultImplementationContext>(options =>
 options.UseInMemoryDatabase("Contacts"));
// Add services to the container.
builder.Services.AddControllers();
var app = builder.Build();

// Configure the HTTP request pipeline.

app.UseHttpsRedirection();
app.MapControllers();

app.MapGet("/api/contacts", ContactsHandler.GetContacts);
app.MapGet("/api/contacts/{id}", ContactsHandler.GetContact);
app.MapPost("/api/contacts", ContactsHandler.PostContact);
app.MapPut("/api/contacts/{id}", ContactsHandler.PutContact);
app.MapDelete("/api/contacts/{id}", ContactsHandler.DeleteContact);




app.Run();
