using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;

namespace microsoft_logging_web_api_playground.Controllers;

[ApiController]
[Route("[controller]")]
public class WeatherForecastController : ControllerBase
{
    private static readonly string[] Summaries = new[]
    {
        "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
    };

    private readonly ILogger<WeatherForecastController> _logger;
    private readonly ILogger _payloadLogger;

    public WeatherForecastController(ILogger<WeatherForecastController> logger, ILoggerFactory loggerFactory)
    {
        _logger = logger;
        _payloadLogger = loggerFactory.CreateLogger("Payloads.HttpIn");
    }

    [HttpGet(Name = "GetWeatherForecast")]
    public IEnumerable<WeatherForecast> Get()
    {
        _payloadLogger.LogInformation("Payload log 1");

        _logger.LogInformation("Teste");

        using (_logger.BeginScope("Meu scope {ScopeId}", Guid.NewGuid()))
        {
            _logger.LogInformation("Log in scope");
        }

        //_payloadLogger.
        var act = new Activity("Sub atividade");
        act.Start();
        act.AddTag("Act tag", 999);
        act.AddBaggage("Baggage info", "xxx");
        _logger.LogInformation("Log in sub activity");

        act.Stop();


        return Enumerable.Range(1, 5).Select(index => new WeatherForecast
        {
            Date = DateOnly.FromDateTime(DateTime.Now.AddDays(index)),
            TemperatureC = Random.Shared.Next(-20, 55),
            Summary = Summaries[Random.Shared.Next(Summaries.Length)]
        })
        .ToArray();
    }
}
