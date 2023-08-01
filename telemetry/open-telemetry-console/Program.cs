using OpenTelemetry.Resources;
using OpenTelemetry;
using OpenTelemetry.Trace;
using System.Diagnostics;
using System.Net;
using System.Net.Http;
using Microsoft.Extensions.Logging;
using System.Reflection.Metadata.Ecma335;

Console.WriteLine(".:: Open Telemetry ::.");

const string ServiceName = "open-telemetry-console-playground";
const string ServiceVersion = "1.0.0";

var loggerFactory = LoggerFactory.Create(builder => builder
    .SetMinimumLevel(LogLevel.Information)
    .AddConsole());
var logger = loggerFactory.CreateLogger<Program>();

using var tracerProvider = Sdk.CreateTracerProviderBuilder()
    .AddSource(ServiceName)
    .SetResourceBuilder(ResourceBuilder.CreateDefault()
        .AddService(serviceName: ServiceName, serviceVersion: ServiceVersion))
    .AddHttpClientInstrumentation()
    .SetSampler(new AlwaysOnSampler())
    // 4317 -> Collector: accept OpenTelemetry Protocol (OTLP) over gRPC, if enabled
    .AddOtlpExporter(opts => opts.Endpoint = new Uri("http://localhost:4317"))
    .AddConsoleExporter()
    .Build()!;

// OpenTelemetry.Exporter.Jaeger: Exportação para agent Jaeger é depreciada. Dê preferência ao padrão OTLP (Open Telemetry Protocol).
//.AddJaegerExporter(exporter =>
// {
//     exporter.AgentHost = "localhost";
//     exporter.AgentPort = 6831;
//     //exporter.Endpoint = new Uri("http://localhost14268:/api/traces");
// })

//var tracer = TracerProvider.Default.GetTracer(ServiceName);
//var tracer = tracerProvider.GetTracer(ServiceName, ServiceVersion);
ActivitySource MyActivitySource = new ActivitySource(ServiceName, ServiceVersion);

using var client = new HttpClient();
//using var activitySource = new ActivitySource(ServiceName, "1.0.0");

/*var tracer = tracerProvider.GetTracer(ServiceName, "1.0.0");
var rootSpan = tracer.StartActiveSpan("teste");
rootSpan.SetAttribute("aaa", "xxx");
rootSpan.AddEvent("teste eventoooo");
rootSpan.Dispose();*/

//while (true)
/*{
    using var activity = activitySource.StartActivity("Teste", ActivityKind.Server);
    activity?.SetTag("meu-id", Guid.NewGuid().ToString());

    var html = await client.GetStringAsync("https://example.com/");

    activity.Stop();
    activity?.Dispose();

    Console.WriteLine("Pressione qualquer tecla para novo teste...");
    Console.ReadLine();
}*/

/*
using (var activity2 = MyActivitySource.StartActivity("SayHello")!)
using (var activity = MyActivitySource.StartActivity("aaaa"))
//using (var activity = new Activity("aaaa"))
{
    activity.SetParentId(activity2.Id);
    var teste = activity.Context;
    activity.Start();
    activity.SetTag("foo", 1);
    activity.SetTag("bar", "Hello, World!");
    activity.SetTag("baz", new int[] { 1, 2, 3 });
    activity.SetStatus(ActivityStatusCode.Ok);

}*/

/*using (var span = tracerProvider.GetTracer(ServiceName).StartActiveSpan("teste"))
{
    span.SetAttribute("aaa", 45);
    var html = await client.GetStringAsync("https://example.com/");
}*/

try
{
    //Calcular(10, 2, "somar");
    //Calcular(15, 3, "subtrair");
    //Calcular(15, 3, "teste");
    CalcularEmLote(simulateError: true).Wait();
}
catch (Exception) { }

tracerProvider.ForceFlush();
tracerProvider.Shutdown();


// ----- Métodos -----

Task CalcularEmLote(bool simulateError) => Task.Run(() =>
{
    using var span = MyActivitySource.StartActivity("lote-operacoes");
    try
    {
        var task = Task.Run(() => Calcular(5, 6, "somar"));
        Calcular(2, 3, "somar");
        Calcular(5, 3, "subtrair");
        if (simulateError)
            Calcular(15, 3, "teste");
        task.Wait();
        span.SetStatus(Status.Ok);
    }
    catch (Exception e)
    {
        span.SetStatus(Status.Error.WithDescription(e.Message));
        //throw;
    }
});

void Calcular(int a, int b, string op)
{
    using var span = MyActivitySource.StartActivity("calcular")!;
    try
    {
        span.SetTag(nameof(a), a);
        span.SetTag(nameof(b), b);
        span.SetTag(nameof(b), b);
        span.AddBaggage("root-baggage", "xxxxxxxxx");

        logger.LogInformation("Iniciando...");

        var result = op switch
        {
            "somar" => Somar(a, b),
            "subtrair" => Subtrair(a, b),
            _ => throw new ArgumentOutOfRangeException($"Operação não suportada: {op}")
        };
        span.SetTag("resultado", result);
        span.SetStatus(Status.Ok);
        logger.LogInformation("Concluído");
    }
    catch (Exception ex)
    {
        logger.LogCritical(ex, "Erro ao calcular");
        //span.SetStatus(Status.Error);
        span.SetStatus(Status.Error.WithDescription(ex.Message));
        throw;
    }
}

int Somar(int a, int b)
{
    using var span = MyActivitySource.StartActivity("somar")!;

    // Baggage não é armazenado pelo collector e não será exibido na interface do Jaeger (https://opentelemetry.io/docs/specs/status/#baggage)
    // Baggages de spans pais são acessíveis aqui, mas o que é adicionado aqui não volta para o pai.
    span.AddBaggage("teste", "aaa");
    span.AddBaggage("teste-2", "bbb");
    Thread.Sleep(Random.Shared.Next(50, 500));
    span.AddEvent(new ActivityEvent("mensagem de log"));
    span.SetStatus(Status.Ok);
    return a + b;
}

int Subtrair(int a, int b)
{
    using var span = MyActivitySource.StartActivity("subtrair")!;
    Thread.Sleep(Random.Shared.Next(50, 500));
    span.SetStatus(Status.Ok);
    return a - b;
}