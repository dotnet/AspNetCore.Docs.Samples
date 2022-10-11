public class SampleProblemDetailsWriter : IProblemDetailsWriter
{
    public bool CanWrite(ProblemDetailsContext context)
        => context.HttpContext.Response.StatusCode == 400;

    public ValueTask WriteAsync(ProblemDetailsContext context)
    {
        // Additional customizations.

        // Write to the response.
        return new ValueTask(context.HttpContext.Response.WriteAsJsonAsync(context.ProblemDetails));
    }
}
