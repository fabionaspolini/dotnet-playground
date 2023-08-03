using jaeger_playground;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using System.Net.Http;

namespace OpenTelemetryWebApiPlayground.Controllers;

[ApiController]
[Route("[controller]")]
public class WeatherForecastController : ControllerBase
{
    private static readonly string[] Summaries = new[]
    {
        "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
    };

    private readonly ILogger<WeatherForecastController> _logger;
    private readonly HttpClient _httpClient;

    public WeatherForecastController(ILogger<WeatherForecastController> logger, HttpClient httpClient)
    {
        _logger = logger;
        _httpClient = httpClient;
    }

    [HttpGet(Name = "GetWeatherForecast")]
    public async Task<IEnumerable<WeatherForecast>> GetAsync()
    {
        //using var activity = DiagnosticsConfig.ActivitySource.StartActivity("SayHello")!;
        //activity.SetTag("foo", 1);
        //activity.SetTag("bar", "Hello, World!");
        //activity.SetTag("baz", new int[] { 1, 2, 3 });

        _logger.LogInformation("Teste");

        //throw new Exception("Fake exception");

        var html = await _httpClient.GetStringAsync("https://example.com/");
        var html2 = await _httpClient.GetStringAsync("https://example.com/");


        return Enumerable.Range(1, 5).Select(index => new WeatherForecast
        {
            Date = DateOnly.FromDateTime(DateTime.Now.AddDays(index)),
            TemperatureC = Random.Shared.Next(-20, 55),
            Summary = Summaries[Random.Shared.Next(Summaries.Length)]
        })
        .ToArray();
    }
}
