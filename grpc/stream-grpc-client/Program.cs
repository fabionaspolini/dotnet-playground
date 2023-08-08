using StreamGrpcClientPlayground;
using Grpc.Core;
using Grpc.Net.Client;
using System.Diagnostics;

Console.WriteLine(".:: gRPC Playground - Stream Client ::.");

const string Protocol = "HTTP";

// Setup clients
GrpcChannel channel;
if (Protocol == "HTTPS")
{
    var httpHandler = new HttpClientHandler();
    httpHandler.ServerCertificateCustomValidationCallback = HttpClientHandler.DangerousAcceptAnyServerCertificateValidator;
    var channelOptions = new GrpcChannelOptions { HttpHandler = httpHandler };
    channel = GrpcChannel.ForAddress("https://localhost:7130", channelOptions);
}
else
{
    channel = GrpcChannel.ForAddress("http://localhost:5155");
}

Console.WriteLine($"Protocolo: {Protocol}");

Greeter.GreeterClient client = new Greeter.GreeterClient(channel);

// Tests
await ReponseStreamTest();



async Task ReponseStreamTest()
{
    try
    {
        var count = 0;
        var request = new HelloRequest { Name = "Teste", Count = 1_000_000 };
        var watch = Stopwatch.StartNew();
        using var streamingCall = client.SayHello(request);
        await foreach (var response in streamingCall.ResponseStream.ReadAllAsync())
        {
            count++;
            if (response.Index % 100_000 == 0)
                Console.WriteLine($"Response: {response.Message}");
        }

        watch.Stop();
        Console.WriteLine($"Concluído - {count:N0} itens em {watch.Elapsed}");
    }
    catch (RpcException ex)
    {
        Console.WriteLine("gRPC test response: " + ex.StatusCode);
        Console.WriteLine(ex.ToString());
    }
}