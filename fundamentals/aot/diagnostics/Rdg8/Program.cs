#define RDG008F //RDG008F
#if NEVER
#elif RDG008
// <snippet_1>
using System.Text.Json.Serialization;

var builder = WebApplication.CreateSlimBuilder(args);

builder.Services.ConfigureHttpJsonOptions(options =>
{
    options.SerializerOptions.TypeInfoResolverChain.Insert(0,
                                 AppJsonSerializerContext.Default);
});

var app = builder.Build();

app.MapPut("/v1/todos/{id}", ([AsParameters] TodoRequest request)
    => Results.Ok(request.Id));

app.Run();

public class TodoRequest
{
    public DateTime DueDate { get; }
    public int Id { get; private set; }
    public string Task { get; private set; }

    // Additional constructors
    public TodoRequest(int Id, string Task, DateTime DueDate)

    {
        this.Id = Id;
        this.Task = Task;
        this.DueDate = DueDate;
    }

    public TodoRequest(int Id, string Task)
        : this(Id, Task, default)
    {
    }
}

[JsonSerializable(typeof(TodoRequest[]))]
internal partial class AppJsonSerializerContext : JsonSerializerContext
{

}
// </snippet_1>
#elif RDG008F
// <snippet_1f>
using System.Text.Json.Serialization;

var builder = WebApplication.CreateSlimBuilder(args);

builder.Services.ConfigureHttpJsonOptions(options =>
{
    options.SerializerOptions.TypeInfoResolverChain.Insert(0,
                                 AppJsonSerializerContext.Default);
});

var app = builder.Build();

app.MapPut("/v1/todos/{id}", ([AsParameters] TodoRequest request)
    => Results.Ok(request.Id));

app.Run();

public class TodoRequest
{
    public DateTime DueDate { get; }
    public int Id { get; private set; }
    public string Task { get; private set; }

    public TodoRequest(int Id, string Task, DateTime DueDate)

    {
        this.Id = Id;
        this.Task = Task;
        this.DueDate = DueDate;
    }
}

[JsonSerializable(typeof(TodoRequest[]))]
internal partial class AppJsonSerializerContext : JsonSerializerContext
{

}
// </snippet_1f>
#endif
