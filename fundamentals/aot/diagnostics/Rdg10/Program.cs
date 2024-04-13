#define RDG10F //   RDG10 RDG10F
#if NEVER
#elif RDG10
// <snippet_1>
using System.Text.Json.Serialization;
using Microsoft.AspNetCore.Mvc;

var builder = WebApplication.CreateSlimBuilder(args);
builder.Services.ConfigureHttpJsonOptions(options =>
{
    options.SerializerOptions.TypeInfoResolverChain.Insert(0,
                                 AppJsonSerializerContext.Default);
});

var app = builder.Build();

app.MapGet("/todos/{id}", ([AsParameters] TodoRequest? request)
    => Results.Ok(new Todo(request!.Id)));

app.Run();

public record TodoRequest(HttpContext HttpContext, [FromRoute] int Id);
public record Todo(int Id);
[JsonSerializable(typeof(Todo[]))]
internal partial class AppJsonSerializerContext : JsonSerializerContext
{

}
// </snippet_1>
#elif RDG10F
// <snippet_1f>
using System.Text.Json.Serialization;
using Microsoft.AspNetCore.Mvc;

var builder = WebApplication.CreateSlimBuilder(args);
builder.Services.ConfigureHttpJsonOptions(options =>
{
    options.SerializerOptions.TypeInfoResolverChain.Insert(0,
                                 AppJsonSerializerContext.Default);
});

var app = builder.Build();

app.MapGet("/todos/{id}", ([AsParameters] TodoRequest request)
    => Results.Ok(new Todo(request.Id)));

app.Run();

public record TodoRequest(HttpContext HttpContext, [FromRoute] int Id);
public record Todo(int Id);
[JsonSerializable(typeof(Todo[]))]
internal partial class AppJsonSerializerContext : JsonSerializerContext
{

}
// </snippet_1f>
#endif

