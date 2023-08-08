using StreamGrpcClientPlayground;
using Grpc.Net.Client;

Console.WriteLine(".:: gRPC Playground - Basic Client ::.");

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
    /*var channelOptions = new GrpcChannelOptions
    {
        HttpHandler = new SocketsHttpHandler
        {
            PooledConnectionIdleTimeout = Timeout.InfiniteTimeSpan,
            EnableMultipleHttp2Connections = true,
            UseCookies = false,
            PreAuthenticate = false,
        }
    };
    channel = GrpcChannel.ForAddress("http://localhost:5155", channelOptions);*/
    channel = GrpcChannel.ForAddress("http://localhost:5155");
}


// Tests
var client = new Greeter.GreeterClient(channel);

var reply = await client.SayHelloAsync(new HelloRequest { Name = "Teste" });
Console.WriteLine("gRPC test response: " + reply.Message);

Console.WriteLine($"Protocolo: {Protocol}");

// gRPC benchmark
var response = await client.SayHelloAsync(new HelloRequest { Name = "Teste" });
Console.WriteLine($"Response: {response.Message}");

Console.WriteLine("Fim");