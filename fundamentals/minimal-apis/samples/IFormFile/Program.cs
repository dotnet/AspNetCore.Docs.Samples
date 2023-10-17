// <snippet_1>
using Microsoft.AspNetCore.Antiforgery;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;

var builder = WebApplication.CreateBuilder();

builder.Services.AddAntiforgery();

var app = builder.Build();
app.UseAntiforgery();

// Generate a form with an anti-forgery token and an /upload endpoint.
app.MapGet("/", (HttpContext context, IAntiforgery antiforgery) =>
{
    var token = antiforgery.GetAndStoreTokens(context);
    var html = MyUtils.GenerateHtmlForm(token.FormFieldName, token.RequestToken!);
    return Results.Content(html, "text/html");
});

app.MapPost("/upload", async Task<Results<Ok<string>,BadRequest<string>>>
    ([FromForm] FileUploadForm fileUploadForm, HttpContext context, IAntiforgery antiforgery) =>
{
    try
    {
        await antiforgery.ValidateRequestAsync(context);
        await MyUtils.SaveFileWithName(fileUploadForm.FileDocument!, fileUploadForm.Name!,
                                       app.Environment.ContentRootPath);
        return TypedResults.Ok($"Your file with the description:" +
            $" {fileUploadForm.Description} has been uploaded successfully");
    }
    catch (AntiforgeryValidationException e)
    {
        return TypedResults.BadRequest("Invalid anti-forgery token" + e.HResult);
    }
});

app.Run();
// </snippet_1>

public class FileUploadForm
{
    public string? Name { get; set; }
    public string? Description { get; set; }
    public IFormFile? FileDocument { get; set; }
}
