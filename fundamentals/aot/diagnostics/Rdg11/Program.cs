#define RDG11F //   RDG11 RDG11F
#if NEVER
#elif RDG11
// <snippet_1>
var builder = WebApplication.CreateSlimBuilder(args);

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

record Todo();
record Wrapper<T> { }
// </snippet_1>
#elif RDG11F
// <snippet_1f>
var builder = WebApplication.CreateSlimBuilder(args);

var app = builder.Build();
app.MapTodoEndpoints();
app.Run();

public static class TodoRouteBuilderExtensions
{
    public static IEndpointRouteBuilder MapTodoEndpoints(this IEndpointRouteBuilder app)
    {
        app.MapPost("/input", (Todo value) => value);
        app.MapGet("/result", () => new Todo());
        app.MapPost("/input-with-wrapper", (Wrapper<Todo> value) => value);
        app.MapGet("/async", async () =>
        {
            await Task.CompletedTask;
            return new Todo();
        });
        return app;
    }
}

record Todo();
record Wrapper<T> { }
// </snippet_1f>
#endif
