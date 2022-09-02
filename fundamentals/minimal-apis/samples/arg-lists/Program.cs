// <snippet_top>
using Microsoft.EntityFrameworkCore;
using TodoApi.Models;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddDatabaseDeveloperPageExceptionFilter();
builder.Services.AddDbContext<TodoDb>(opt => opt.UseInMemoryDatabase("TodoList"));
var app = builder.Build();

app.MapGet("/todoitems", async (TodoDb db) =>
    await db.Todos.Select(x => new TodoItemDTO(x)).ToListAsync());

// <snippet_id>
app.MapGet("/todoitems/{id}", async (int Id, TodoDb Db) =>
    await Db.Todos.FindAsync(Id)
        is Todo todo
            ? Results.Ok(new TodoItemDTO(todo))
            : Results.NotFound());
// </snippet_id>
// Remaining code removed for brevity.
// </snippet_top>

// <snippet_post>
app.MapPost("/todoitems", async (TodoItemDTO Dto, TodoDb Db) =>
{
    var todoItem = new Todo
    {
        IsComplete = Dto.IsComplete,
        Name = Dto.Name
    };

    Db.Todos.Add(todoItem);
    await Db.SaveChangesAsync();

    return Results.Created($"/todoitems/{todoItem.Id}", new TodoItemDTO(todoItem));
});
// </snippet_post>

// <snippet_put>
app.MapPut("/todoitems/{id}", async (int Id, TodoItemDTO Dto, TodoDb Db) =>
{
    var todo = await Db.Todos.FindAsync(Id);

    if (todo is null) return Results.NotFound();

    todo.Name = Dto.Name;
    todo.IsComplete = Dto.IsComplete;

    await Db.SaveChangesAsync();

    return Results.NoContent();
});
// </snippet_put>

// <snippet_delete>
app.MapDelete("/todoitems/{id}", async (int Id, TodoDb Db) =>
{
    if (await Db.Todos.FindAsync(Id) is Todo todo)
    {
        Db.Todos.Remove(todo);
        await Db.SaveChangesAsync();
        return Results.Ok(new TodoItemDTO(todo));
    }

    return Results.NotFound();
});
// </snippet_delete>

// --- [AsParameters] go here ----------------------
// <snippet_ap_id>
app.MapGet("/ap/todoitems/{id}", async ([AsParameters] TodoItemRequest request) =>
    await request.Db.Todos.FindAsync(request.Id)
        is Todo todo
            ? Results.Ok(new TodoItemDTO(todo))
            : Results.NotFound());
// </snippet_ap_id>

// <snippet_ap_post>
app.MapPost("/ap/todoitems", async ([AsParameters] CreateTodoItemRequest request) =>
{
    var todoItem = new Todo
    {
        IsComplete = request.Dto.IsComplete,
        Name = request.Dto.Name
    };

    request.Db.Todos.Add(todoItem);
    await request.Db.SaveChangesAsync();

    return Results.Created($"/todoitems/{todoItem.Id}", new TodoItemDTO(todoItem));
});
// </snippet_ap_post>

// <snippet_ap_put>
app.MapPut("/ap/todoitems/{id}", async ([AsParameters] EditTodoItemRequest request) =>
{
    var todo = await request.Db.Todos.FindAsync(request.Id);

    if (todo is null) return Results.NotFound();

    todo.Name = request.Dto.Name;
    todo.IsComplete = request.Dto.IsComplete;

    await request.Db.SaveChangesAsync();

    return Results.NoContent();
});
// </snippet_ap_put>

// <snippet_ap_delete>
app.MapDelete("/ap/todoitems/{id}", async ([AsParameters] TodoItemRequest request) =>
{
    if (await request.Db.Todos.FindAsync(request.Id) is Todo todo)
    {
        request.Db.Todos.Remove(todo);
        await request.Db.SaveChangesAsync();
        return Results.Ok(new TodoItemDTO(todo));
    }

    return Results.NotFound();
});
// </snippet_ap_delete>

app.Run();
