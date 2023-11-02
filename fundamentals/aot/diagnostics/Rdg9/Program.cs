#define RDG009 // RDG009 RDG009F
#if NEVER
#elif RDG009
// <snippet_1>
using System.Text.Json.Serialization;

var builder = WebApplication.CreateSlimBuilder();
var todos = new[]
{
    new Todo(1, "Write tests", DateTime.UtcNow.AddDays(2)),
    new Todo(2, "Fix tests",DateTime.UtcNow.AddDays(1))
};
builder.Services.AddSingleton(todos);
builder.Services.ConfigureHttpJsonOptions(options =>
{
    options.SerializerOptions.TypeInfoResolverChain.Insert(0, AppJsonSerializerContext.Default);
});

var app = builder.Build();

app.MapGet("/v1/todos/{id}", ([AsParameters] TodoItemRequest request) =>
{
    return request.todos.ToList().Find(todoItem => todoItem.Id == request.Id)
is Todo todo
    ? Results.Ok(todo)
    : Results.NotFound();
});

app.Run();

struct TodoItemRequest
{
    public int Id { get; set; }
    //[AsParameters]
    public Todo[] todos { get; set; }
}

internal record Todo(int Id, string Task, DateTime DueDate);

[JsonSerializable(typeof(Todo[]))]
internal partial class AppJsonSerializerContext : JsonSerializerContext
{
}
// </snippet_1>
#elif RDG009F
// <snippet_1f>
using System.Text.Json.Serialization;

var builder = WebApplication.CreateSlimBuilder();

var todos = new[]
{
    new Todo(1, "Write tests", DateTime.UtcNow.AddDays(2)),
    new Todo(2, "Fix tests",DateTime.UtcNow.AddDays(1))
};

builder.Services.AddSingleton(todos);
builder.Services.ConfigureHttpJsonOptions(options =>
{
    options.SerializerOptions.TypeInfoResolverChain.Insert(0, AppJsonSerializerContext.Default);
});

var app = builder.Build();

app.MapGet("/v1/todos/{id}", ([AsParameters] TodoItemRequest request) =>
{
     return request.todos.ToList().Find(todoItem => todoItem.Id == request.Id) is Todo todo
    ? Results.Ok(todo)
    : Results.NotFound();
});

app.Run();

struct TodoItemRequest
{
    public int Id { get; set; }
    [AsParameters]
    public Todo[] todos { get; set; }
}

internal record Todo(int Id, string Task, DateTime DueDate);

[JsonSerializable(typeof(Todo[]))]
internal partial class AppJsonSerializerContext : JsonSerializerContext
{
}
// </snippet_1f>
#endif
