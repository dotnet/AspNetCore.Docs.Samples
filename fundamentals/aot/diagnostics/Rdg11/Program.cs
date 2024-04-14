#define RDG11F //   RDG11 RDG11F
#if NEVER
#elif RDG11
// <snippet_1>
using System.Text.Json.Serialization;

var builder = WebApplication.CreateSlimBuilder(args);
builder.Services.ConfigureHttpJsonOptions(options =>
{
    options.SerializerOptions.TypeInfoResolverChain.Insert(0,
                                 AppJsonSerializerContext.Default);
});

var app = builder.Build();
app.MapEndpoints<Todo>();
app.Run();

public static class RouteBuilderExtensions
{
    public static IEndpointRouteBuilder MapEndpoints<T>(this IEndpointRouteBuilder app) where T : class, new()
    {
        app.MapPost("/input", (T value) => value);
        app.MapGet("/result", () => new T());
        app.MapPost("/input-with-wrapper", (Wrapper<T> value) => value);
        app.MapGet("/async", async () =>
        {
            await Task.CompletedTask;
            return new T();
        });
        return app;
    }
}

record Wrapper<T>(T Value);
class Todo
{
    public int Id { get; set; }
    public string Task { get; set; }
}
[JsonSerializable(typeof(Wrapper<Todo>[]))]
internal partial class AppJsonSerializerContext : JsonSerializerContext
{

}
// </snippet_1>
#elif RDG11F
// <snippet_1f>
using System.Text.Json.Serialization;

var builder = WebApplication.CreateSlimBuilder(args);
builder.Services.ConfigureHttpJsonOptions(options =>
{
    options.SerializerOptions.TypeInfoResolverChain.Insert(0,
                                 AppJsonSerializerContext.Default);
});

var app = builder.Build();
app.MapTodoEndpoints();
app.Run();

public static class TodoRouteBuilderExtensions
{
    public static IEndpointRouteBuilder MapTodoEndpoints(this IEndpointRouteBuilder app)
    {
        app.MapPost("/input", (Todo value) => value);
        app.MapGet("/result", () => new Todo(1, "Walk the dog"));
        app.MapPost("/input-with-wrapper", (Wrapper<Todo> value) => value);
        app.MapGet("/async", async () =>
        {
            await Task.CompletedTask;
            return new Todo(1, "Walk the dog");
        });
        return app;
    }
}

record Wrapper<T>(T Value);
record Todo(int Id, string Task);
[JsonSerializable(typeof(Wrapper<Todo>[]))]
internal partial class AppJsonSerializerContext : JsonSerializerContext
{

}
// </snippet_1f>
#endif
