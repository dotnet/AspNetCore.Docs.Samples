#define RDG6 //   RDG6 RDG6F
#if NEVER
#elif RDG6
// <snippet_1>
using System.Text.Json.Serialization;

var builder = WebApplication.CreateSlimBuilder(args);

builder.Services.ConfigureHttpJsonOptions(options =>
{
    options.SerializerOptions.TypeInfoResolverChain.Insert(0,
                                       AppJsonSerializerContext.Default);
});

var app = builder.Build();

app.MapPut("/v1/todos/{id}", ([AsParameters] TodoRequest todoRequest) => Results.Ok(todoRequest.Todo));

app.Run();

class TodoRequest(int id, string name)
{
    public int Id { get; set; } = id;
    public Todo? Todo { get; set; }
}

record Todo(int Id, string Task);

[JsonSerializable(typeof(Todo[]))]
internal partial class AppJsonSerializerContext : JsonSerializerContext
{

}
// </snippet_1>
#elif RDG6F
// <snippet_1f>
using System.Text.Json.Serialization;

var builder = WebApplication.CreateSlimBuilder(args);

builder.Services.ConfigureHttpJsonOptions(options =>
{
    options.SerializerOptions.TypeInfoResolverChain.Insert(0,
                                 AppJsonSerializerContext.Default);
});

var app = builder.Build();

app.MapPut("/v1/todos/{id}", ([AsParameters] TodoRequest todoRequest) => Results.Ok(todoRequest.Todo));

app.Run();

class TodoRequest(int id, Todo? todo)
{
    public int Id { get; set; } = id;
    public Todo? Todo { get; set; } = todo;
}

record Todo(int Id, string Task);

[JsonSerializable(typeof(Todo[]))]
internal partial class AppJsonSerializerContext : JsonSerializerContext
{

}
// </snippet_1f>
#endif
