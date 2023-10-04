#define RDG12F //   RDG12 RDG12F
#if NEVER
#elif RDG12
// <snippet_1>
var builder = WebApplication.CreateSlimBuilder(args);

var app = builder.Build();
app.MapEndpoints();
app.Run();

public static class TodoRouteBuilderExtensions
{
    public static IEndpointRouteBuilder MapEndpoints(this IEndpointRouteBuilder app)
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

    private record Todo { };
}

record Wrapper<T> { }
// </snippet_1>
#elif RDG12F
// <snippet_1f>
var builder = WebApplication.CreateSlimBuilder(args);

var app = builder.Build();
app.MapEndpoints();
app.Run();

public static class TodoRouteBuilderExtensions
{
    public static IEndpointRouteBuilder MapEndpoints(this IEndpointRouteBuilder app)
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

    public record Todo { };
}

record Wrapper<T> { }
// </snippet_1f>
#endif
