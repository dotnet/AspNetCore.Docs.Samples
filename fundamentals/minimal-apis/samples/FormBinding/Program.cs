// <snippet_top>;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TodoApi.Models;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddDatabaseDeveloperPageExceptionFilter();
builder.Services.AddDbContext<TodoDb>(opt => opt.UseInMemoryDatabase("TodoList"));
var app = builder.Build();

// <snippet_get>
app.MapGet("/todos", async (TodoDb db) =>
    await db.Todos.Select(x => new TodoDto(x)).ToListAsync());
// </snippet_get>

// <snippet_id>
app.MapGet("/todos/{id}",
                             async (int Id, TodoDb Db) =>
    await Db.Todos.FindAsync(Id)
        is Todo todo
            ? Results.Ok(new TodoDto(todo))
            : Results.NotFound());
// </snippet_id>

// Remaining code removed for brevity.

// </snippet_top>

//Avoid reading the incoming file stream directly into memory all at once.
//For example, don't copy all of the file's bytes into a System.IO.MemoryStream
//or read the entire stream into a byte array all at once. These approaches can result in performance and security problems
//Instead, consider adopting either of the following approaches:

// * On the server of a server app, copy the stream directly to a file on disk without reading it into memory.
// * Upload files from the client directly to an external service.

// <snippet_post_put_delete_as_parameters>
app.MapPost("/ap/todos", async ([AsParameters] NewTodoRequest request, TodoDb db) =>
{
    var todo = new Todo
    {
        Name = request.Name,
        Visibility = request.Visibility
    };

    if (request.Attachment is not null)
    {
        var attachmentName = Path.GetRandomFileName();

        using var stream = File.Create(Path.Combine("wwwroot", attachmentName));
        await request.Attachment.CopyToAsync(stream);

        todo.Attachment = attachmentName;
    }

    db.Todos.Add(todo);
    await db.SaveChangesAsync();

    return Results.Ok();
});

// Remaining code removed for brevity.

// </snippet_post_put_delete_as_parameters>

// <snippet_post_put_delete>
app.MapPost("/todos", async ([FromForm] string name, [FromForm] Visibility visibility, IFormFile? attachment, TodoDb db) =>
{
    var todo = new Todo
    {
        Name = name,
        Visibility = visibility
    };

    if (attachment is not null)
    {
        var attachmentName = Path.GetRandomFileName();

        using var stream = File.Create(Path.Combine("wwwroot", attachmentName));
        await attachment.CopyToAsync(stream);
    }

    db.Todos.Add(todo);
    await db.SaveChangesAsync();

    return Results.Ok();
});

// Remaining code removed for brevity.

// </snippet_post_put_delete>

app.UseStaticFiles();
app.Run();

// <snippet_argumentlist_record>
public record struct NewTodoRequest([FromForm] string Name, [FromForm] Visibility Visibility, IFormFile? Attachment);
// </snippet_argumentlist_record>
