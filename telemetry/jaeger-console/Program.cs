// NÃO FUNCIONA

using Jaeger.Reporters;
using Jaeger.Samplers;
using Jaeger;
using Microsoft.Extensions.Logging;
using Jaeger.Senders;
using Jaeger.Senders.Thrift;

Console.WriteLine(".:: Jaeger ::.");

var loggerFactory = LoggerFactory.Create(builder => builder
    .SetMinimumLevel(LogLevel.Information)
    .AddConsole());
var logger = loggerFactory.CreateLogger<Program>();
var serviceName = "jaeger-console-app-playground";


//Configuration.SenderConfiguration.DefaultSenderResolver = new SenderResolver(loggerFactory)
//    .RegisterSenderFactory<ThriftSenderFactory>();

//Configuration config = new Configuration(serviceName, loggerFactory);
//.WithSampler(...)   // optional, defaults to RemoteControlledSampler with HttpSamplingManager on localhost:5778
//.WithReporter(...); // optional, defaults to RemoteReporter with UdpSender on localhost:6831 when ThriftSenderFactory is registered

//var reporter = new LoggingReporter(loggerFactory);
var reporter = new SenderResolver(loggerFactory);
var sampler = new ConstSampler(true);
var tracer = new Tracer.Builder(serviceName)
    .WithLoggerFactory(loggerFactory)
    //.WithReporter(reporter)
    .WithSampler(sampler)
    .Build();

ExecuteAction();
Thread.Sleep(250);
ExecuteAction();

Thread.Sleep(5000);

void ExecuteAction()
{
    var operationName = "Job::teste";
    var builder = tracer
        .BuildSpan(operationName)
        .WithTag("id", Guid.NewGuid().ToString());
    //var builder = config.GetTracer().BuildSpan(operationName).WithTag("id", Guid.NewGuid().ToString());
    var span = builder.Start();
    logger.LogInformation("Iniciando...");
    Thread.Sleep(500);
    logger.LogInformation("Concluído");
    span.Finish();
}