#define RGG2F //   RGG2 RGG2F
#if NEVER
#elif RGG2
// <snippet_1>
using System.Text.Json.Serialization;

var builder = WebApplication.CreateSlimBuilder(args);

builder.Services.ConfigureHttpJsonOptions(options =>
{
    options.SerializerOptions.TypeInfoResolverChain.Insert(0, 
                                       AppJsonSerializerContext.Default);
});

var app = builder.Build();

var del = Wrapper.GetTodos;
app.MapGet("/v1/todos", del);

app.Run();

record Todo(int Id, string Task);
[JsonSerializable(typeof(Todo[]))]
internal partial class AppJsonSerializerContext : JsonSerializerContext
{

}

class Wrapper
{
    public static Func<IResult> GetTodos = () =>
        Results.Ok(new Todo(1, "Write test fix"));
}

// </snippet_1>
#elif RGG2F
// <snippet_1f>
using System.Text.Json.Serialization;

var builder = WebApplication.CreateSlimBuilder(args);

builder.Services.ConfigureHttpJsonOptions(options =>
{
    options.SerializerOptions.TypeInfoResolverChain.Insert(0,
                                 AppJsonSerializerContext.Default);
});

var app = builder.Build();

app.MapGet("/v1/todos", () => Results.Ok(new Todo(1, "Write tests")));

app.Run();

record Todo(int Id, string Task);
[JsonSerializable(typeof(Todo[]))]
internal partial class AppJsonSerializerContext : JsonSerializerContext
{

}
// </snippet_1f>
#endif
