using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace MicrosoftLoggingWebApiPlayground.Controllers;

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

        var currentAct = Activity.Current;

        _logger.LogInformation("Teste");
        _logger.LogInformation("Teste 2");

        //_payloadLogger.
        var act = new Activity("Sub atividade");
        act.Start();
        _logger.LogInformation("Teste 3");

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
