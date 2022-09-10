#define FIRST // FIRST MIDDLEWARE API_CONTROLLER
#if NEVER
#elif FIRST
// <snippet_1>

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddProblemDetails(options =>
    options.CustomizeProblemDetails = (context) =>
    {

        var mathErrorFeature = context.HttpContext.Features
                                                   .Get<MathErrorFeature>();
        if (mathErrorFeature is not null)
        {
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
        }
    }
    );


var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseStatusCodePages();

// /divide?numerator=2&denominator=4
app.MapGet("/divide", (HttpContext context, double numerator, double denominator) =>
{
    if (denominator == 0)
    {
        var errorType = new MathErrorFeature { MathError = MathErrorType.DivisionByZeroError };
        context.Features.Set(errorType);
        return Results.BadRequest();
    }

    var calculation = numerator / denominator;
    return Results.Ok(calculation);
});

// /squareroot?radicand=16
app.MapGet("/squareroot", (HttpContext context, double radicand) =>
{
    if (radicand < 0)
    {
        var errorType = new MathErrorFeature { MathError = MathErrorType.NegativeRadicandError };
        context.Features.Set(errorType);
        return Results.BadRequest();
    }

    return Results.Ok(Math.Sqrt(radicand));
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

app.UseStatusCodePages();

// Middleware to handle writing problem details to the response
app.Use(async (context, next) =>
{
    var mathErrorFeature = new MathErrorFeature();
    context.Features.Set(mathErrorFeature);
    await next(context);
    if (context.Response.StatusCode > 399 && HasPath(context))
    {
        if (context.RequestServices.GetService<IProblemDetailsService>() is
                                                           { } problemDetailsService)
        {
            (string Detail, string Type) details = mathErrorFeature.MathError switch
            {
                MathErrorType.DivisionByZeroError => ("The number you inputed is zero",
                "https://en.wikipedia.org/wiki/Division_by_zero"),
                _ => ("Negative or complex numbers are not handled", 
                "https://en.wikipedia.org/wiki/Square_root")
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

    return Results.Ok(numerator / denominator);
});

// /squareroot?radicand=16
app.MapGet("/squareroot", (HttpContext context, double radicand) =>
{
    if (radicand < 0)
    {
        context.Features.GetRequiredFeature<MathErrorFeature>().MathError =
                                                       MathErrorType.NegativeRadicandError;
        return Results.BadRequest();
    }

    return Results.Ok(Math.Sqrt(radicand));
});

app.Run();

// Check if error message is defined for selected paths.
static bool HasPath(HttpContext context)
{
    return context.Request.Path.Value switch
    {
        "/divide" => true,
        "/squareroot" => true,
        _ => false
    };
}

#elif API_CONTROLLER
using Microsoft.AspNetCore.Http.Features;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddProblemDetails(options =>
    options.CustomizeProblemDetails = (context) =>
    {

        var mathErrorFeature = context.HttpContext.Features
                                                   .GetRequiredFeature<MathErrorFeature>();
        if (HasPath(context.HttpContext))
        {
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
        }
    }
    );


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
    context.Features.Set(new MathErrorFeature());
    await next(context);
});

app.UseStatusCodePages();

app.UseAuthorization();

app.MapControllers();

app.Run();

// Check if error message is defined for selected paths.
static bool HasPath(HttpContext context)
{
    return context.Request.Path.Value.Contains("/api/values/Divide", 
                                StringComparison.OrdinalIgnoreCase) ||
        context.Request.Path.Value.Contains("/api/values/Squareroot",
                                StringComparison.OrdinalIgnoreCase);
}

#endif
