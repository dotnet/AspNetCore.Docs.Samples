#define RDG10F //   RDG10 RDG10F
#if NEVER
#elif RDG10
// <snippet_1>
using Microsoft.AspNetCore.Mvc;

var builder = WebApplication.CreateBuilder(args);

var app = builder.Build();

app.MapGet("/todos/{id}", ([AsParameters] TodoRequest? request)
    => Results.Ok(new Todo(request!.Id)));

app.Run();

public record TodoRequest(HttpContext HttpContext, [FromRoute] int Id);
public record Todo(int Id);
// </snippet_1>
#elif RDG10F
// <snippet_1f>
using Microsoft.AspNetCore.Mvc;

var builder = WebApplication.CreateBuilder(args);

var app = builder.Build();

app.MapGet("/todos/{id}", ([AsParameters] TodoRequest request)
    => Results.Ok(new Todo(request.Id)));

app.Run();

public record TodoRequest(HttpContext HttpContext, [FromRoute] int Id);
public record Todo(int Id);
// </snippet_1f>
#endif

