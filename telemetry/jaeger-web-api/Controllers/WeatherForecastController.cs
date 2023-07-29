using jaeger_playground;
using Microsoft.AspNetCore.Mvc;
using OpenTracing;

namespace JaegerWebApiPlayground.Controllers;

[ApiController]
[Route("[controller]")]
public class WeatherForecastController : ControllerBase
{
    private static readonly string[] Summaries = new[]
    {
        "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
    };

    private readonly ILogger<WeatherForecastController> _logger;
    private readonly ITracer _tracer;

    public WeatherForecastController(ILogger<WeatherForecastController> logger, ITracer tracer)
    {
        _logger = logger;
        _tracer = tracer;
    }

    [HttpGet(Name = "GetWeatherForecast")]
    public IEnumerable<WeatherForecast> Get()
    {
        var span = _tracer.BuildSpan("Teste").Start();
        //using var activity = DiagnosticsConfig.ActivitySource.StartActivity("SayHello");
        //activity?.SetTag("foo", 1);
        //activity?.SetTag("bar", "Hello, World!");
        //activity?.SetTag("baz", new int[] { 1, 2, 3 });

        _logger.LogInformation("Teste");
        Thread.Sleep(100);
        span.Finish();
        return Enumerable.Range(1, 5).Select(index => new WeatherForecast
        {
            Date = DateOnly.FromDateTime(DateTime.Now.AddDays(index)),
            TemperatureC = Random.Shared.Next(-20, 55),
            Summary = Summaries[Random.Shared.Next(Summaries.Length)]
        })
        .ToArray();
    }
}
