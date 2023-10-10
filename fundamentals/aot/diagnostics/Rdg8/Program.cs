#define RDG008 //RDG008 RDG008F
#if NEVER
#elif RDG008
// Sample code requires removing https from properties/launchsettings.json
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
    return request.Todos.ToList().Find(todoItem => todoItem.Id == request.Id)
is Todo todo
    ? Results.Ok(todo)
    : Results.NotFound();
});

app.Run();

public class TodoItemRequest
{
    public int Id { get; set; }
    public Todo[] Todos { get; set; }

    public TodoItemRequest(int id, Todo[] todos)
    {
        Id = id;
        Todos = todos;
    }
    
    // Additional Constructor
    public TodoItemRequest()
    {
        Id = 1;
        Todos = [new Todo(1, "Write tests", DateTime.UtcNow.AddDays(2))];
    }

}

public class Todo
{
    public DateTime DueDate { get; }
    public int Id { get; private set; }
    public string Task { get; private set; }

    public Todo(int id, string task, DateTime dueDate)

    {
        Id = id;
        Task = task;
        DueDate = dueDate;
    }
}

[JsonSerializable(typeof(Todo[]))]
internal partial class AppJsonSerializerContext : JsonSerializerContext
{
}
// </snippet_1>
#elif RDG008F
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
    return request.Todos.ToList().Find(todoItem => todoItem.Id == request.Id)
is Todo todo
    ? Results.Ok(todo)
    : Results.NotFound();
});

app.Run();

public class TodoItemRequest
{
    public int Id { get; set; }
    public Todo[] Todos { get; set; }

    public TodoItemRequest(int id, Todo[] todos)
    {
        Id = id;
        Todos = todos;
    }
}

public class Todo
{
    public DateTime DueDate { get; }
    public int Id { get; private set; }
    public string Task { get; private set; }

    public Todo(int id, string task, DateTime dueDate)

    {
        Id = id;
        Task = task;
        DueDate = dueDate;
    }
}

[JsonSerializable(typeof(Todo[]))]
internal partial class AppJsonSerializerContext : JsonSerializerContext
{
}
// </snippet_1f>
#endif
