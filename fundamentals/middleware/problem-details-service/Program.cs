#define MIDDLEWARE // FIRST MIDDLEWARE
#if NEVER
#elif FIRST
// <snippet_1>
using Microsoft.AspNetCore.Http.Features;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddProblemDetails(options =>
    options.CustomizeProblemDetails = (context) =>
    {
        var mathErrorFeature = context.HttpContext.Features
                                                   .GetRequiredFeature<MathErrorFeature>();
      
        (string Detail, string Type) details = mathErrorFeature.MathError switch
        {
            MathErrorType.DivisionByZeroError => 
            ("Divison by zero is not defined.",
                                       "https://wikipedia.org/wiki/Division_by_zero"),
            _ => ("Negative or complex numbers are not valid input.",
                                         "https://wikipedia.org/wiki/Square_root")
        };

        context.ProblemDetails.Type = details.Type;
        context.ProblemDetails.Title = "Wrong Input";
        context.ProblemDetails.Detail = details.Detail;
    });

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

// Middleware to handle writing problem details to the response
app.Use(async (context, next) =>
{
    var mathErrorFeature = new MathErrorFeature();
    context.Features.Set(new MathErrorFeature());
    await next(context);
});

app.UseStatusCodePages();

// /divide?numerator=2&denominator=4
app.MapGet("/divide", (HttpContext context, double numerator, double denominator) =>
{
    if (denominator == 0)
    {
        context.Features.GetRequiredFeature<MathErrorFeature>().MathError = 
                                                     MathErrorType.DivisionByZeroError;
        return Results.BadRequest();
    }

    var calculation = numerator / denominator;
    return Results.Ok(calculation);
});

// /squareroot?radicand=16
app.MapGet("/squareroot", (HttpContext context, int radicand) =>
{
    if (radicand < 0)
    {
        context.Features.GetRequiredFeature<MathErrorFeature>().MathError =
                                                       MathErrorType.NegativeRadicandError;
        return Results.BadRequest();
    }

    var calculation = Math.Sqrt(radicand);
    return Results.Ok(calculation);
});

app.Run();
// </snippet_1>
#elif MIDDLEWARE
using Microsoft.AspNetCore.Http.Features;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddProblemDetails();
var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

// Middleware to handle writing problem details to the response
app.Use(async (context, next) =>
{
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

// /divide?numerator=2&denominator=4
app.MapGet("/divide", (HttpContext context, double numerator, double denominator) =>
{
    if (denominator == 0)
    {
        context.Features.GetRequiredFeature<MathErrorFeature>().MathError =
                                                     MathErrorType.DivisionByZeroError;
        return Results.BadRequest();
    }

    var calculation = numerator / denominator;
    return Results.Ok(calculation);
});

// /squareroot?radicand=16
app.MapGet("/squareroot", (HttpContext context, int radicand) =>
{
    if (radicand < 0)
    {
        context.Features.GetRequiredFeature<MathErrorFeature>().MathError =
                                                       MathErrorType.NegativeRadicandError;
        return Results.BadRequest();
    }

    var calculation = Math.Sqrt(radicand);
    return Results.Ok(calculation);
});

app.Run();
#endif
