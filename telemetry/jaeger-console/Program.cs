using Jaeger.Reporters;
using Jaeger.Samplers;
using Jaeger;
using Microsoft.Extensions.Logging;
using Jaeger.Senders.Thrift;
using System.Diagnostics;

// #######################################################
// #####   NÃO USAR ESTE CÓDIGO PARA NOVOS PROJETO   #####
// #####   UTILIZE SDK DO OPEN TELEMETRY             #####
// #######################################################

Console.WriteLine(".:: Jaeger ::.");
const string ServiceName = "jaeger-console-app-playground";

var loggerFactory = LoggerFactory.Create(builder => builder
    .SetMinimumLevel(LogLevel.Information)
    .AddConsole());
var logger = loggerFactory.CreateLogger<Program>();

var reporter = new RemoteReporter.Builder()
    .WithLoggerFactory(loggerFactory)
    .WithSender(new HttpSender("http://localhost:14268/api/traces"))
    .Build();

using var tracer = new Tracer.Builder(ServiceName)
    .WithLoggerFactory(loggerFactory)
    .WithSampler(new ConstSampler(true))
    .WithReporter(reporter)
    .Build();

//Calcular(10, 2, "somar");
//Calcular(15, 3, "subtrair");

var lote1 = Task.Run(() =>
{
    Trace.CorrelationManager.ActivityId = Guid.NewGuid();
    using (tracer.BuildSpan("lote-operacoes").StartActive())
    {
        tracer.ActiveSpan.SetBaggageItem("lote-id", "1");
        var task = Task.Run(() => Calcular(5, 6, "somar"));
        Calcular(2, 3, "somar");
        Calcular(5, 3, "subtrair");
        task.Wait();
    }
});

var lote2 = Task.Run(() =>
{
    Trace.CorrelationManager.ActivityId = Guid.NewGuid();
    using (tracer.BuildSpan("lote-operacoes").StartActive())
    {
        tracer.ActiveSpan.SetBaggageItem("lote-id", "2");
        var task = Task.Run(() => Calcular(10, 12, "somar"));
        Calcular(4, 6, "somar");
        Calcular(10, 6, "subtrair");
        task.Wait();
    }
});

lote1.Wait();
lote2.Wait();

void Calcular(int a, int b, string op)
{
    const string operationName = "calcular";
    using var activity = tracer.BuildSpan(operationName)
        .WithTag(nameof(a), a)
        .WithTag(nameof(b), b)
        .WithTag(nameof(op), op)
        .StartActive();
    logger.LogInformation("Iniciando...");


    var result = op switch
    {
        "somar" => Somar(a, b),
        "subtrair" => Subtrair(a, b),
        _ => throw new ArgumentOutOfRangeException($"Operação não suportada: {op}")
    };
    activity.Span.SetTag("resultado", result);

    logger.LogInformation("Concluído");
}

int Somar(int a, int b)
{
    var span = tracer.BuildSpan("somar").Start();
    try
    {
        span.SetBaggageItem("teste", "aaa");
        span.SetBaggageItem("teste-2", "bbb");
        Thread.Sleep(Random.Shared.Next(50, 500));
        span.Log("mensagem de log");
        return a + b;
    }
    finally
    {
        span.Finish();
    }
}

int Subtrair(int a, int b)
{
    var span = tracer.BuildSpan("subtrair").Start();
    try
    {
        Thread.Sleep(Random.Shared.Next(50, 500));
        return a - b;
    }
    finally
    {
        span.Finish();
    }
}