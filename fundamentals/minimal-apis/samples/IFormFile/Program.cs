// <snippet_1>
using Microsoft.AspNetCore.Antiforgery;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;

var builder = WebApplication.CreateBuilder();

builder.Services.AddAntiforgery();

var app = builder.Build();
app.UseAntiforgery();

var MyHtml = new MyUtils();

// Generate a form with an anti-forgery token and an /upload endpoint.
app.MapGet("/", (HttpContext context, IAntiforgery antiforgery) =>
{
    var token = antiforgery.GetAndStoreTokens(context);
    var html = MyHtml.GenerateHtmlForm(token.FormFieldName, token.RequestToken!);
    return Results.Content(html, "text/html");
});

app.MapPost("/upload", async Task<Results<Ok<string>,BadRequest<string>>>
    ([FromForm] FileUpload file, HttpContext context, IAntiforgery antiforgery) =>
{
    try
    {
        await antiforgery.ValidateRequestAsync(context);
        await MyHtml.UploadFileWithName(file.FileDocument!, file.Name!,
                                       app.Environment.ContentRootPath);
        return TypedResults.Ok($"Your file with the description:" +
            $" {file.Description} has been uploaded successfully");
    }
    catch (AntiforgeryValidationException e)
    {
        return TypedResults.BadRequest("Invalid anti-forgery token" + e.HResult);
    }
});

app.Run();
// </snippet_1>

public class FileUpload
{
    public string? Name { get; set; }
    public string? Description { get; set; }
    public IFormFile? FileDocument { get; set; }
}

public  class MyUtils
{
     string GetOrCreateFilePath(string fileName, string contentRootPath,
         string filesDirectory = "uploadFiles")
    {
        var directoryPath = Path.Combine(contentRootPath, filesDirectory);
        Directory.CreateDirectory(directoryPath);
        return Path.Combine(directoryPath, fileName);
    }

     public async Task UploadFileWithName(IFormFile file, string fileSaveName, string contentRootPath)
    {
        var filePath = GetOrCreateFilePath(fileSaveName, contentRootPath);
        await using var fileStream = new FileStream(filePath, FileMode.Create);
        await file.CopyToAsync(fileStream);
    }

    public string GenerateHtmlForm(string formFieldName, string requestToken)
    {
        return $"""
      <html>
        <body>
          <form action="/upload" method="POST" enctype="multipart/form-data">
            <input name="{formFieldName}" type="hidden" value="{requestToken}" required/>
            <br/>
            <input name="Name" type="text" placeholder="Name of file" pattern=".*\.(jpg|jpeg|png)$" title="Please enter a valid name ending with .jpg, .jpeg, or .png" required/>
            <br/>
            <input name="Description" type="text" placeholder="Description of file" required/>
            <br/>
            <input type="file" name="FileDocument" placeholder="Upload an image..." accept=".jpg, 
                                                                            .jpeg, .png" />
            <input type="submit" />
          </form> 
        </body>
      </html>
    """;
    }
}
