using OpenTelemetry.Resources;
using OpenTelemetry.Trace;
using System.Diagnostics;
using System.Diagnostics.Metrics;

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
        .ConfigureResource(resource => resource.AddService(DiagnosticsConfig.ServiceName))
        .AddHttpClientInstrumentation()
        .AddAspNetCoreInstrumentation()
        .AddJaegerExporter()
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
    public const string ServiceName = "open-telemetry-playground";
    public static ActivitySource ActivitySource = new(ServiceName);
}