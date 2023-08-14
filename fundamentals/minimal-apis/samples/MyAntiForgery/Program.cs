using Microsoft.AspNetCore.Antiforgery;
using Microsoft.AspNetCore.Mvc;

var builder = WebApplication.CreateBuilder();

builder.Services.AddAntiforgery();

var app = builder.Build();

app.MapGet("/", (HttpContext context, IAntiforgery antiforgery) =>
{
    var token = antiforgery.GetAndStoreTokens(context);
    var html = $"""
        <html><body>
            <form action="/todo" method="POST" enctype="multipart/form-data">
                <input name="{token.FormFieldName}" 
                              type="hidden" value="{token.RequestToken}" />
                <input type="text" name="name" />
                <input type="date" name="dueDate" />
                <input type="checkbox" name="isCompleted" />
                <input type="submit" />
            </form>
         </body></html>
    """;
    return Results.Content(html, "text/html");
});


app.MapGet("/DisableAntiforgery", () =>
{
    return Results.Content(MyHtml.html("/todo"), "text/html");
});

app.MapGet("/post2", () =>
{
    return Results.Content(MyHtml.html("/todo2"), "text/html");
});

app.MapPost("/todo", ([FromForm] Todo todo) => Results.Ok(todo));

app.MapPost("/todo2", ([FromForm] Todo todo) => Results.Ok(todo))
                                                .DisableAntiforgery();

app.Run();

class Todo
{
    public required string Name { get; set; }
    public bool IsCompleted { get; set; }
    public DateTime DueDate { get; set; }
}

public static class MyHtml
{
    public static string html(string action) => $"""
        <html><body>
            <form action="{action}" method="POST" enctype="multipart/form-data">
                <input type="text" name="name" />
                <input type="date" name="dueDate" />
                <input type="checkbox" name="isCompleted" />
                <input type="submit" />
            </form>
        </body></html>
    """;
}