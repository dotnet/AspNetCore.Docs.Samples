#define RDG12F //   RDG12 RDG12F
#if NEVER
#elif RDG12
// <snippet_1>
using System.Text.Json.Serialization;

var builder = WebApplication.CreateSlimBuilder(args);
builder.Services.ConfigureHttpJsonOptions(options =>
{
    options.SerializerOptions.TypeInfoResolverChain.Insert(0,
                                 AppJsonSerializerContext.Default);
});

var app = builder.Build();
app.MapEndpoints();
app.Run();

public static class TodoRouteBuilderExtensions
{
    public static IEndpointRouteBuilder MapEndpoints(this IEndpointRouteBuilder app)
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

    private record Todo(int Id, string Task);
}

record Wrapper<T>(T Value);
[JsonSerializable(typeof(Wrapper<TodoRouteBuilderExtensions.Todo>[]))]
internal partial class AppJsonSerializerContext : JsonSerializerContext
{

}
// </snippet_1>
#elif RDG12F
// <snippet_1f>
using System.Text.Json.Serialization;

var builder = WebApplication.CreateSlimBuilder(args);
builder.Services.ConfigureHttpJsonOptions(options =>
{
    options.SerializerOptions.TypeInfoResolverChain.Insert(0,
                                 AppJsonSerializerContext.Default);
});


var app = builder.Build();
app.MapEndpoints();
app.Run();

public static class TodoRouteBuilderExtensions
{
    public static IEndpointRouteBuilder MapEndpoints(this IEndpointRouteBuilder app)
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

    public record Todo(int Id, string Task);
}

record Wrapper<T>(T Value);
[JsonSerializable(typeof(Wrapper<TodoRouteBuilderExtensions.Todo>[]))]
internal partial class AppJsonSerializerContext : JsonSerializerContext
{

}
// </snippet_1f>
#endif
