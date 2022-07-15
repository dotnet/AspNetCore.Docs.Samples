var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
var app = builder.Build();
var uri = "/WeatherForecast/GetByRange?range=" +
                      $"{DateTime.Now.Date.ToShortDateString()}-" +
                      $"{DateTime.Now.AddDays(5).Date.ToShortDateString()}";

app.MapGet("/", () => Results.Redirect(uri));

app.UseAuthorization();

app.MapControllers();

app.Run();
