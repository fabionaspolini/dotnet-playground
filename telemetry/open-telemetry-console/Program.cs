using OpenTelemetry.Resources;
using OpenTelemetry;
using OpenTelemetry.Trace;
using System.Diagnostics;
using System.Net;
using System.Net.Http;

Console.WriteLine(".:: Open Telemetry ::.");

const string ServiceName = "open-telemetry-console-playground";

using var tracerProvider = Sdk.CreateTracerProviderBuilder()
    .AddSource(ServiceName)
    .SetResourceBuilder(ResourceBuilder.CreateDefault()
        .AddService(serviceName: ServiceName, serviceVersion: "1.0.0"))
    .AddHttpClientInstrumentation()
    .SetSampler(new AlwaysOnSampler())
    /*.AddJaegerExporter(exporter =>
    {
        exporter.AgentHost = "localhost";
        exporter.AgentPort = 14268;
        //exporter.Endpoint = new Uri("http://localhost:14268/api/traces");
    })*/
    .AddOtlpExporter(opts => opts.Endpoint = new Uri("http://localhost:4317")) // 4317 -> accept OpenTelemetry Protocol (OTLP) over gRPC, if enabled
    .AddConsoleExporter()
    .Build()!;

//var tracer = TracerProvider.Default.GetTracer(ServiceName);
var tracer = tracerProvider.GetTracer(ServiceName);

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

ActivitySource MyActivitySource = new ActivitySource(ServiceName);
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

}

/*using (var span = tracerProvider.GetTracer(ServiceName).StartActiveSpan("teste"))
{
    span.SetAttribute("aaa", 45);
    var html = await client.GetStringAsync("https://example.com/");
}*/



tracerProvider.ForceFlush();
tracerProvider.Shutdown();
Thread.Sleep(1000);

/*
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
}*/