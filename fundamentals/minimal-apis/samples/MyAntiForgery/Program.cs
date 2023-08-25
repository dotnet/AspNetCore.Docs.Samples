#define FIRST // FIRST SHORT
#if NEVER
#elif FIRST
// <snippet_all>
using Microsoft.AspNetCore.Antiforgery;
using Microsoft.AspNetCore.Mvc;

var builder = WebApplication.CreateBuilder();

builder.Services.AddAntiforgery();

var app = builder.Build();

// <snippet_get>
// Pass token
app.MapGet("/", (HttpContext context, IAntiforgery antiforgery) =>
{
    var token = antiforgery.GetAndStoreTokens(context);
    return Results.Content(MyHtml.GenerateForm("/todo", token), "text/html");
});

// Don't pass a token, fails
app.MapGet("/SkipToken", (HttpContext context, IAntiforgery antiforgery) =>
{
    var token = antiforgery.GetAndStoreTokens(context);
    return Results.Content(MyHtml.GenerateForm("/todo",token, false ), "text/html");
});

// Post to /todo2. DisableAntiforgery on that endpoint so no token needed.
app.MapGet("/DisableAntiforgery", (HttpContext context, IAntiforgery antiforgery) =>
{
    var token = antiforgery.GetAndStoreTokens(context);
    return Results.Content(MyHtml.GenerateForm("/todo2", token, false), "text/html");
});

// <snippet_post>
app.MapPost("/todo", ([FromForm] Todo todo) => Results.Ok(todo));

app.MapPost("/todo2", ([FromForm] Todo todo) => Results.Ok(todo))
                                                .DisableAntiforgery();
// </snippet_post>
// </snippet_get>

app.Run();

class Todo
{
    public required string Name { get; set; }
    public bool IsCompleted { get; set; }
    public DateTime DueDate { get; set; }
}

public static class MyHtml
{
    // <snippet_html>
    public static string GenerateForm(string action, 
        AntiforgeryTokenSet token, bool UseToken=true)
    {
        string tokenInput = "";
        if (UseToken)
        {
            tokenInput = $@"<input name=""{token.FormFieldName}""
                             type=""hidden"" value=""{token.RequestToken}"" />";
        }

        return $@"
        <html><body>
            <form action=""{action}"" method=""POST"" enctype=""multipart/form-data"">
                {tokenInput}
                <input type=""text"" name=""name"" />
                <input type=""date"" name=""dueDate"" />
                <input type=""checkbox"" name=""isCompleted"" />
                <input type=""submit"" />
            </form>
        </body></html>
    ";
    }
    // </snippet_html>
}
// </snippet_all>
#elif SHORT
// <snippet_short>
var builder = WebApplication.CreateBuilder();

builder.Services.AddAntiforgery();

var app = builder.Build();

// Implicitly added by WebApplicationBuilder
// app.UseAntiforgery();

app.MapGet("/", () => "Hello World!");

app.Run();
// </snippet_short>
#endif
