using Jaeger.Samplers;
using Jaeger.Senders.Thrift;
using Jaeger.Senders;
using Jaeger;
using OpenTracing.Util;
using OpenTracing;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Use "OpenTracing.Contrib.NetCore" to automatically generate spans for ASP.NET Core, Entity Framework Core, ...
// See https://github.com/opentracing-contrib/csharp-netcore for details.
builder.Services.AddOpenTracing();

// Adds the Jaeger Tracer.
builder.Services.AddSingleton<ITracer>(serviceProvider =>
{
    var serviceName = serviceProvider.GetRequiredService<IWebHostEnvironment>().ApplicationName;
    var loggerFactory = serviceProvider.GetRequiredService<ILoggerFactory>();

    // This is necessary to pick the correct sender, otherwise a NoopSender is used!
    Jaeger.Configuration.SenderConfiguration.DefaultSenderResolver = new SenderResolver(loggerFactory)
        .RegisterSenderFactory<ThriftSenderFactory>();

    // This will log to a default localhost installation of Jaeger.
    var tracer = new Tracer.Builder(serviceName)
        .WithLoggerFactory(loggerFactory)
        .WithSampler(new ConstSampler(true))
        .Build();

    // Allows code that can't use DI to also access the tracer.
    GlobalTracer.Register(tracer);

    return tracer;
});

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
