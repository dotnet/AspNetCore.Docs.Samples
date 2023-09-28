#define RDG5F //   RDG5 RDG5F
#if NEVER
#elif RDG5
// <snippet_1>
using System.Text.Json.Serialization;

var builder = WebApplication.CreateSlimBuilder(args);

builder.Services.ConfigureHttpJsonOptions(options =>
{
    options.SerializerOptions.TypeInfoResolverChain.Insert(0,
                                       AppJsonSerializerContext.Default);
});

var app = builder.Build();
app.MapPut("/v1/todos/{id}",
    ([AsParameters] TodoRequest todoRequest) => Results.Ok(todoRequest.Todo));

app.Run();

abstract class TodoRequest
{
    public int Id { get; set; }
    public Todo? Todo { get; set; }
}

record Todo(int Id, string Task);

[JsonSerializable(typeof(Todo[]))]
internal partial class AppJsonSerializerContext : JsonSerializerContext
{

}
// </snippet_1>
#elif RDG5F
// <snippet_1f>
using System.Text.Json.Serialization;

var builder = WebApplication.CreateSlimBuilder(args);

builder.Services.ConfigureHttpJsonOptions(options =>
{
    options.SerializerOptions.TypeInfoResolverChain.Insert(0,
                                 AppJsonSerializerContext.Default);
});

var app = builder.Build();

app.MapPut("/v1/todos/{id}",
           ([AsParameters] TodoRequest todoRequest) => Results.Ok(todoRequest.Todo));

app.Run();

class TodoRequest
{
    public int Id { get; set; }
    public Todo? Todo { get; set; }
}

record Todo(int Id, string Task);

[JsonSerializable(typeof(Todo[]))]
internal partial class AppJsonSerializerContext : JsonSerializerContext
{

}
// </snippet_1f>
#endif
