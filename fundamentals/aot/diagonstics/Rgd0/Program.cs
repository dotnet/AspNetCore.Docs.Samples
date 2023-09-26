#define RGG0 // RGG0 RGG0F RGG1
#if NEVER
#elif RGG0
// <snippet_0>
using System.Text.Json.Serialization;

var builder = WebApplication.CreateSlimBuilder(args);

builder.Services.ConfigureHttpJsonOptions(options =>
{
    options.SerializerOptions.TypeInfoResolverChain.Insert(0, AppJsonSerializerContext.Default);
});

var app = builder.Build();

var version = "v1";
var route = $"/{version}/todos";

app.MapGet("/v1/todos", () => Results.Ok(new Todo(1, "Write tests")));

app.Run();

record Todo(int Id, string Task);
[JsonSerializable(typeof(Todo[]))]
internal partial class AppJsonSerializerContext : JsonSerializerContext
{

}
// </snippet_0>
#elif RGG0F
// <snippet_0f>
var app = WebApplication.Create();

app.MapGet("/v1/todos", () => Results.Ok(new Todo(1, "Write tests")));

app.Run();

record Todo(int Id, string Task);
// </snippet_0f>
#elif RGG1
// <snippet_1>

// </snippet_1>
#endif