// See https://aka.ms/new-console-template for more information
using BasicGrpcClientPlayground;
using Grpc.Net.Client;
using System.Diagnostics;

Console.WriteLine(".:: gRPC Playground - Basic Client ::.");

var httpHandler = new HttpClientHandler();
httpHandler.ServerCertificateCustomValidationCallback = HttpClientHandler.DangerousAcceptAnyServerCertificateValidator;
var channelOptions = new GrpcChannelOptions { HttpHandler = httpHandler };

using var channel = GrpcChannel.ForAddress("https://localhost:7130", channelOptions);
var client = new Greeter.GreeterClient(channel);

var reply = await client.SayHelloAsync(new HelloRequest { Name = "Teste" });
Console.WriteLine("Greeting: " + reply.Message);

var watch = Stopwatch.StartNew();
var timing = TimeSpan.FromSeconds(3);
var count = 0;
while (watch.Elapsed <= timing)
{
    await client.SayHelloAsync(new HelloRequest { Name = "Teste" });
    count++;
}
watch.Stop();

Console.WriteLine($"{count} requisições em {watch.Elapsed}");
Console.WriteLine("Press any key to exit...");
Console.ReadKey();