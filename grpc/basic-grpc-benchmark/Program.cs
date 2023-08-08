// See https://aka.ms/new-console-template for more information
using BasicGrpcBenchmarkPlayground;
using Grpc.Net.Client;
using System.Diagnostics;
using System.Net.Http.Json;

Console.WriteLine(".:: gRPC Playground -  Basic Benchmark ::.");

const string Protocol = "HTTP";

const string httpsPostUrl = "https://localhost:7081/say-hello";
const string httpPostUrl = "http://localhost:5225/say-hello";

// Setup clients
GrpcChannel channel;
HttpClient httpClient;
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
    httpClient = new HttpClient(handler);
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

    // rest
    httpClient = new HttpClient();
    postUrl = httpPostUrl;
}


// Tests
var client = new Greeter.GreeterClient(channel);

var reply = await client.SayHelloAsync(new HelloRequest { Name = "Teste" });
Console.WriteLine("gRPC test response: OK");

var httpResponse = await httpClient.PostAsJsonAsync(postUrl, new HelloRequest { Name = "Teste" });
Console.WriteLine("REST test response: " + httpResponse.StatusCode);
Console.WriteLine();

if (!httpResponse.IsSuccessStatusCode)
    throw new Exception("Erro teste REST!");

var TotalBenchmarkTime = TimeSpan.FromSeconds(2);

Console.WriteLine($"Protocolo: {Protocol}");
Console.WriteLine();

// gRPC benchmark
var grpcCount = 0;
var watch = Stopwatch.StartNew();
while (watch.Elapsed <= TotalBenchmarkTime)
{
    await client.SayHelloAsync(new HelloRequest { Name = "Teste" });
    grpcCount++;
}
watch.Stop();

Console.WriteLine($"gRPC -> {grpcCount} requisições em {watch.Elapsed}");

// REST benchmark
var restCount = 0;
watch = Stopwatch.StartNew();
while (watch.Elapsed <= TotalBenchmarkTime)
{
    var response = await httpClient.PostAsJsonAsync(httpPostUrl, new HelloRequest { Name = "Teste" });
    if (!response.IsSuccessStatusCode)
        throw new Exception("Erro benchmark REST!");

    restCount++;
}
watch.Stop();

Console.WriteLine($"REST -> {restCount} requisições em {watch.Elapsed}");

// Fim
Console.WriteLine("Fim");
