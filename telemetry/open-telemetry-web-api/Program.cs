using OpenTelemetry.Resources;
using OpenTelemetry.Trace;
using System.Diagnostics;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddHttpClient();

builder.Services.AddOpenTelemetry()
    .WithTracing(builder => builder
        .AddSource(DiagnosticsConfig.ActivitySource.Name)
        .ConfigureResource(resource => resource.AddService(DiagnosticsConfig.ServiceName, serviceVersion: DiagnosticsConfig.ServiceVersion))
        .AddHttpClientInstrumentation(opts => opts.RecordException = true)
        .AddAspNetCoreInstrumentation(opts => opts.RecordException = true)
        .AddOtlpExporter(opts => opts.Endpoint = new Uri("http://localhost:4317"))
        .AddConsoleExporter());

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();

public static class DiagnosticsConfig
{
    public const string ServiceName = "open-telemetry-web-api-playground";
    public const string ServiceVersion = "1.0.0";
    public static readonly ActivitySource ActivitySource = new(ServiceName, ServiceVersion);
}