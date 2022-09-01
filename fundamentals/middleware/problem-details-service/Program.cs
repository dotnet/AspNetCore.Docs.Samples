using Microsoft.AspNetCore.OpenApi;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddProblemDetails();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

// middleware to handle writing problem details to the response
app.Use(async (context, next) =>
{
    // added the MathErrorFeature to the request pipeline
    var mathErrorFeature = new MathErrorFeature();
    context.Features.Set(mathErrorFeature);

    await next(context);

    if (context.Response.StatusCode > 399)
    {
        if (context.RequestServices.GetService<IProblemDetailsService>() is { } problemDetailsService)
        {
            (string Detail, string Type) details = mathErrorFeature.MathError switch
            {
                MathErrorType.DivisionByZeroError => ("The number you inputed is zero", "https://en.wikipedia.org/wiki/Division_by_zero"),
                _ => ("Negative or complex numbers are not handled", "https://en.wikipedia.org/wiki/Square_root")
            };

            await problemDetailsService.WriteAsync(new ProblemDetailsContext
            {
                HttpContext = context,
                ProblemDetails =
                {
                    Title = "Wrong Input",
                    Detail = details.Detail,
                    Type = details.Type
                }
            });
        }
    }
});

// endpoint for dividing numbers
app.MapGet("/divide", async (HttpContext context, double numerator, double denominator) =>
{
    if (denominator == 0)
    {
        context.Features.Get<MathErrorFeature>()!.MathError = MathErrorType.DivisionByZeroError;
        return Results.BadRequest();
    }

    var calculation = await Task.FromResult(numerator / denominator);
    return Results.Ok(calculation);
});

// endpoint for obtaining the squareroot of a number
app.MapGet("/squareroot", async (HttpContext context, int radicand) =>
{
    if (radicand < 0)
    {
        context.Features.Get<MathErrorFeature>()!.MathError = MathErrorType.NegativeRadicandError;
        return Results.BadRequest();
    }

    var calculation = await Task.FromResult(Math.Sqrt(radicand));
    return Results.Ok(calculation);
});

app.Run();

// Custom math errors
enum MathErrorType
{
    DivisionByZeroError,
    NegativeRadicandError
}

// Custom Http Request Feature
class MathErrorFeature
{
    public MathErrorType MathError { get; set; }
}
