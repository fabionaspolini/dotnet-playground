// See https://aka.ms/new-console-template for more information
using BasicGrpcClientPlayground;
using Grpc.Net.Client;
using System.Diagnostics;
using System.Net;
using System.Net.Http.Json;

Console.WriteLine(".:: gRPC Playground - Basic Client ::.");

var httpHandler = new HttpClientHandler();
httpHandler.ServerCertificateCustomValidationCallback = HttpClientHandler.DangerousAcceptAnyServerCertificateValidator;
var channelOptions = new GrpcChannelOptions { HttpHandler = httpHandler };

using var channel = GrpcChannel.ForAddress("https://localhost:7130", channelOptions);
var client = new Greeter.GreeterClient(channel);

var reply = await client.SayHelloAsync(new HelloRequest { Name = "Teste" });
Console.WriteLine("gRPC test response: " + reply.Message);
Console.WriteLine();


var timing = TimeSpan.FromSeconds(5);

// gRPC benchmark
var grpcCount = 0;
var watch = Stopwatch.StartNew();
while (watch.Elapsed <= timing)
{
    await client.SayHelloAsync(new HelloRequest { Name = "Teste" });
    grpcCount++;
}
watch.Stop();

Console.WriteLine($"{grpcCount} requisições gRPC em {watch.Elapsed}");

// HTTP benchmark
var handler = new HttpClientHandler();
handler.ClientCertificateOptions = ClientCertificateOption.Manual;
handler.ServerCertificateCustomValidationCallback =
    (httpRequestMessage, cert, cetChain, policyErrors) =>
    {
        return true;
    };
var http = new HttpClient(handler);

var httpResponse = await http.PostAsJsonAsync("https://localhost:7081/say-hello", new HelloRequest { Name = "Teste" });
if (!httpResponse.IsSuccessStatusCode)
    throw new Exception("Erro teste HTTP!");

var httpCount = 1;
watch = Stopwatch.StartNew();
while (watch.Elapsed <= timing)
{
    await http.PostAsJsonAsync("https://localhost:7081/say-hello", new HelloRequest { Name = "Teste" });
    httpCount++;
}
watch.Stop();

Console.WriteLine($"{httpCount} requisições HTTP em {watch.Elapsed}");


Console.WriteLine("Press any key to exit...");
Console.ReadKey();
