#define RDG13 //   RDG13 RDG13F
#if NEVER
#elif RDG13
// <snippet_1>
using System.Text.Json.Serialization;
using Microsoft.AspNetCore.Mvc;

var builder = WebApplication.CreateSlimBuilder(args);

builder.Services.ConfigureHttpJsonOptions(options =>
{
    options.SerializerOptions.TypeInfoResolverChain.Insert(0,
                                       AppJsonSerializerContext.Default);
});

var app = builder.Build();

app.MapGet("/v1/todos", 
    ([FromKeyedServices("primary")][FromServices] ITodoService todoService)
   => Results.Ok(todoService.GetTodos()));

app.Run();

record Todo(int Id, string Task);
interface ITodoService
{
    Todo[] GetTodos();
}

[JsonSerializable(typeof(Todo[]))]
internal partial class AppJsonSerializerContext : JsonSerializerContext
{

}
// </snippet_1>
#elif RDG13F
// <snippet_1f>
using System.Text.Json.Serialization;
using Microsoft.AspNetCore.Mvc;

var builder = WebApplication.CreateSlimBuilder(args);

builder.Services.AddKeyedSingleton<ITodoService, TodoService>("primary");
builder.Services.AddScoped<ITodoService, TodoService>();

builder.Services.ConfigureHttpJsonOptions(options =>
{
    options.SerializerOptions.TypeInfoResolverChain.Insert(0,
                                       AppJsonSerializerContext.Default);
});


var app = builder.Build();

app.MapGet("/v1/todos",
    ([FromKeyedServices("primary")] ITodoService todoService)
   => Results.Ok(todoService.GetTodos()));
// OR
app.MapGet("/v2/todos",
    ([FromServices] ITodoService todoService)
   => Results.Ok(todoService.GetTodos()));

app.Run();

public record Todo(int Id, string Task);
interface ITodoService
{
    Todo[] GetTodos();
}

public class TodoService : ITodoService
{
    // A sample in-memory list of Todo items. In a real-world application, you might fetch these from a database.
    private readonly Todo[] todos =
    {
        new Todo(1, "Write unit tests"),
        new Todo(2, "Implement authentication"),
        new Todo(3, "Refactor code")
    };

    public Todo[] GetTodos() => todos;
}


[JsonSerializable(typeof(Todo[]))]
internal partial class AppJsonSerializerContext : JsonSerializerContext
{

}
// </snippet_1f>
#endif
