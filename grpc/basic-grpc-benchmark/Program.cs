// See https://aka.ms/new-console-template for more information
using BasicGrpcBenchmarkPlayground;
using Grpc.Net.Client;
using System.Diagnostics;
using System.Net.Http.Json;

Console.WriteLine(".:: gRPC Playground - Basic Client ::.");

const string Protocol = "HTTP";

const string httpsPostUrl = "https://localhost:7081/say-hello";
const string httpPostUrl = "http://localhost:5225/say-hello";

// Setup clients
GrpcChannel channel;
HttpClient http;
string postUrl;
if (Protocol == "HTTPS")
{
    // grpc
    var httpHandler = new HttpClientHandler();
    httpHandler.ServerCertificateCustomValidationCallback = HttpClientHandler.DangerousAcceptAnyServerCertificateValidator;
    var channelOptions = new GrpcChannelOptions { HttpHandler = httpHandler };
    channel = GrpcChannel.ForAddress("https://localhost:7130", channelOptions);

    // http
    var handler = new HttpClientHandler();
    handler.ClientCertificateOptions = ClientCertificateOption.Manual;
    handler.ServerCertificateCustomValidationCallback =
        (httpRequestMessage, cert, cetChain, policyErrors) =>
        {
            return true;
        };
    http = new HttpClient(handler);
    postUrl = httpsPostUrl;
}
else
{
    // grpc
    var channelOptions = new GrpcChannelOptions
    {
        HttpHandler = new SocketsHttpHandler
        {
            PooledConnectionIdleTimeout = Timeout.InfiniteTimeSpan,
            EnableMultipleHttp2Connections = true,
            UseCookies = false,
            PreAuthenticate = false,
        }
    };
    channel = GrpcChannel.ForAddress("http://localhost:5155", channelOptions);

    // http
    http = new HttpClient();
    postUrl = httpPostUrl;
}


// Tests
var client = new Greeter.GreeterClient(channel);

var reply = await client.SayHelloAsync(new HelloRequest { Name = "Teste" });
Console.WriteLine("gRPC test response: " + reply.Message);

var httpResponse = await http.PostAsJsonAsync(postUrl, new HelloRequest { Name = "Teste" });
Console.WriteLine("HTTP test response: " + httpResponse.StatusCode);
Console.WriteLine();

if (!httpResponse.IsSuccessStatusCode)
    throw new Exception("Erro teste HTTP!");

var TotalBenchmarkTime = TimeSpan.FromSeconds(2);

Console.WriteLine($"Protocolo: {Protocol}");

// gRPC benchmark
var grpcCount = 0;
var watch = Stopwatch.StartNew();
while (watch.Elapsed <= TotalBenchmarkTime)
{
    await client.SayHelloAsync(new HelloRequest { Name = "Teste" });
    grpcCount++;
}
watch.Stop();

Console.WriteLine($"{grpcCount} requisições gRPC em {watch.Elapsed}");

// HTTP benchmark
var httpCount = 0;
watch = Stopwatch.StartNew();
while (watch.Elapsed <= TotalBenchmarkTime)
{
    var response = await http.PostAsJsonAsync(httpPostUrl, new HelloRequest { Name = "Teste" });
    if (!response.IsSuccessStatusCode)
        throw new Exception("Erro benchmark HTTP!");

    httpCount++;
}
watch.Stop();

Console.WriteLine($"{httpCount} requisições HTTP em {watch.Elapsed}");

// Fim
Console.WriteLine("Press any key to exit...");
Console.ReadKey();
