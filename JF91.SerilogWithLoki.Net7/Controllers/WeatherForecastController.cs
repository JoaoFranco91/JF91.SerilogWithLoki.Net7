using Microsoft.AspNetCore.Mvc;

namespace JF91.SerilogWithLoki.Net7.Controllers;

[ApiController]
[Route("[controller]")]
public class WeatherForecastController : ControllerBase
{
    private static readonly string[] Summaries = new[]
    {
        "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
    };

    private readonly ILogger<WeatherForecastController> _logger;

    public WeatherForecastController
    (
        ILogger<WeatherForecastController> logger
    )
    {
        _logger = logger;
    }

    [HttpGet(Name = "GetWeatherForecast")]
    public IEnumerable<WeatherForecast> Get()
    {
        _logger.LogDebug("Debug Log");
        _logger.LogTrace("Trace Log");
        _logger.LogCritical("Critical Log");
        _logger.LogError("Error Log");
        _logger.LogWarning("Warning Log");
        _logger.LogInformation("Information Log");
        
        return Enumerable.Range(1, 5).Select
            (
                index => new WeatherForecast
                {
                    Date = DateOnly.FromDateTime(DateTime.Now.AddDays(index)),
                    TemperatureC = Random.Shared.Next(-20, 55),
                    Summary = Summaries[Random.Shared.Next(Summaries.Length)]
                }
            )
            .ToArray();
    }
}