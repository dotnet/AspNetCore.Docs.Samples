using Microsoft.AspNetCore.Mvc;

var builder = WebApplication.CreateBuilder();

builder.Services.AddAntiforgery();

var app = builder.Build();

//app.UseAntiforgery();

app.MapPost("/upload", ([FromForm] DocumentUpload document) =>
{
    return Results.Ok();
});

app.Run();

public class DocumentUpload
{
    public string Name { get; set; } = "Uploaded Document";
    public string? Description { get; set; }
    public IFormFile? Document { get; set; }
}
