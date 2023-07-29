using OpenTelemetry.Resources;
using OpenTelemetry;
using OpenTelemetry.Trace;
using System.Diagnostics;
using System.Net;
using System.Net.Http;

Console.WriteLine(".:: Open Telemetry ::.");

const string ServiceName = "open-telemetry-console-playground";

//using var tracerProvider = Sdk.CreateTracerProviderBuilder()
//    .AddSource(ServiceName)
//    .SetResourceBuilder(
//        ResourceBuilder.CreateDefault()
//            .AddService(serviceName: ServiceName, serviceVersion: "1.0.0"))
//    .AddHttpClientInstrumentation()
//    .AddJaegerExporter(exporter =>
//    {
//        exporter.AgentHost = "localhost";
//        exporter.AgentPort = 6831;
//    })
//    .Build()!;
///*
//using var activitySource = new ActivitySource(ServiceName, "1.0.0");

//using (var activity = activitySource.StartActivity("SendRequests"))
//{
//    activity?.SetTag("startPoint", "Program.cs");
//    Console.WriteLine("Fim");
//}*/

//var tracer = tracerProvider.GetTracer(ServiceName, "1.0.0");
//var span = tracer.StartRootSpan("SpanName");
//Thread.Sleep(100);
//tracerProvider.ForceFlush();

//Console.WriteLine("Fim");
//Thread.Sleep(5000);


using var tracerProvider = Sdk.CreateTracerProviderBuilder()
    .AddSource(ServiceName)
    .SetResourceBuilder(
        ResourceBuilder.CreateDefault()
            .AddService(serviceName: ServiceName, serviceVersion: "1.0.0"))
    .AddHttpClientInstrumentation()
    .AddJaegerExporter(exporter =>
    {
        exporter.AgentHost = "localhost";
        exporter.AgentPort = 6831;
    })
    .Build();

using var client = new HttpClient();
using var activitySource = new ActivitySource(ServiceName, "1.0.0");

while (true)
{
    using var activity = activitySource.StartActivity("Teste", ActivityKind.Server);
    activity?.SetTag("meu-id", Guid.NewGuid().ToString());

    var html = await client.GetStringAsync("https://example.com/");

    activity?.Dispose();

    Console.WriteLine("Pressione qualquer tecla para novo teste...");
    Console.ReadLine();
}