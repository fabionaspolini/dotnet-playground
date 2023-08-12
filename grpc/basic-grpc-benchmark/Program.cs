// See https://aka.ms/new-console-template for more information
using BasicGrpcBenchmarkPlayground;
using Grpc.Net.Client;
using System.Diagnostics;
using System.Net.Http.Json;

Console.WriteLine(".:: gRPC Playground -  Basic Benchmark ::.");

const string Protocol = "HTTPS";

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
    var channelOptions = new GrpcChannelOptions { HttpHandler = httpHandler, UnsafeUseInsecureChannelCallCredentials=true };
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
        },
    };
    channel = GrpcChannel.ForAddress("http://localhost:5155", channelOptions);

    // rest
    httpClient = new HttpClient();
    postUrl = httpPostUrl;
}


// Tests
Greeter.GreeterClient client = new Greeter.GreeterClient(channel);

var reply = await client.SayHelloAsync(new HelloRequest { Name = "Teste" });
Console.WriteLine("gRPC test response: OK");

var httpResponse = await httpClient.PostAsJsonAsync(postUrl, new HelloRequest { Name = "Teste" });
Console.WriteLine("REST test response: " + httpResponse.StatusCode);
Console.WriteLine();

if (!httpResponse.IsSuccessStatusCode)
    throw new Exception("Erro teste REST!");

// ------------- Parâmetros -------------
var TotalBenchmarkTime = TimeSpan.FromSeconds(5);
const int Threads = 5;
// --------------------------------------

Console.WriteLine($"Protocolo: {Protocol}");
Console.WriteLine();

// Tasks
var grpcTasks = new List<Task<int>>();
var restTasks = new List<Task<int>>();

var watch = Stopwatch.StartNew();
for (var i = 0; i < Threads; i++)
    grpcTasks.Add(StartGrpcTask());
    //grpcTasks.Add(StartGrpcTaskAsync());
Task.WaitAll(grpcTasks.ToArray());
watch.Stop();
var grpcCount = grpcTasks.Sum(x => x.Result);
Console.WriteLine($"gRPC -> {grpcCount} requisições em {watch.Elapsed}");

watch = Stopwatch.StartNew();
for (var i = 0; i < Threads; i++)
    restTasks.Add(StartRestTask());
Task.WaitAll(restTasks.ToArray());
watch.Stop();
var restCount = restTasks.Sum(x => x.Result);

Console.WriteLine($"REST -> {restCount} requisições em {watch.Elapsed}");

// Fim
Console.WriteLine("Fim");


Task<int> StartGrpcTask() => Task.Run(() =>
{
    //Greeter.GreeterClient client2 = new Greeter.GreeterClient(channel);
    var count = 0;
    var watch = Stopwatch.StartNew();
    while (watch.Elapsed <= TotalBenchmarkTime)
    {
        client.SayHello(new HelloRequest { Name = "Teste" });
        count++;
    }
    watch.Stop();
    return count;
});

Task<int> StartGrpcTaskAsync() => Task.Run(async () =>
{
    //Greeter.GreeterClient client2 = new Greeter.GreeterClient(channel);
    var count = 0;
    var watch = Stopwatch.StartNew();
    while (watch.Elapsed <= TotalBenchmarkTime)
    {
        await client.SayHelloAsync(new HelloRequest { Name = "Teste" });
        count++;
    }
    watch.Stop();
    return count;
});

Task<int> StartRestTask() => Task.Run(async () =>
{
    var count = 0;
    var watch = Stopwatch.StartNew();
    while (watch.Elapsed <= TotalBenchmarkTime)
    {
        var response = await httpClient.PostAsJsonAsync(httpPostUrl, new HelloRequest { Name = "Teste" });
        if (!response.IsSuccessStatusCode)
            throw new Exception("Erro benchmark REST!");
        count++;
    }
    watch.Stop();
    return count;
});