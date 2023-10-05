using System.Text.Json.Serialization;

var builder = WebApplication.CreateSlimBuilder();
var todos = new[]
{
    new Todo(1, "Write tests"),
    new Todo(2, "Fix tests")
};

builder.Services.AddSingleton(todos);

builder.Services.ConfigureHttpJsonOptions(options =>
{
    options.SerializerOptions.TypeInfoResolverChain.Insert(0, AppJsonSerializerContext.Default);
});

var app = builder.Build();


app.MapGet("/v1/todos/{id?}", (int? Id, Todo[] todos) =>
{
    if (Id.HasValue)
    {
        return todos.ToList().Find(todoItem => todoItem.Id == Id)
   is Todo todo
       ? Results.Ok(todo)
       : Results.NotFound();
    }
    else
    {
        return Results.Ok(todos);
    }

});

app.Run();


public class Todo
{
    public DateTime DueDate { get; }
    public int Id { get; private set; }
    public string Task { get; private set; }

    // Additional constructors
    public Todo(int Id, string Task, DateTime DueDate)

    {
        this.Id = Id;
        this.Task = Task;
        this.DueDate = DueDate;
    }

    public Todo(int Id, string Task)
        : this(Id, Task, default)
    {
    }
}

[JsonSerializable(typeof(Todo[]))]
internal partial class AppJsonSerializerContext : JsonSerializerContext
{
}

