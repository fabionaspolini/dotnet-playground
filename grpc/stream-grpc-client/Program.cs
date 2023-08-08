using StreamGrpcClientPlayground;
using Grpc.Core;
using Grpc.Net.Client;
using System.Xml.Linq;

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

Console.WriteLine($"Protocolo: {Protocol}");

// Tests
var client = new Greeter.GreeterClient(channel);
try
{
    var response = await client.SayHelloAsync(new HelloRequest { Name = "Teste" });
    Console.WriteLine($"Response: {response.Message}");
}
catch (RpcException ex)
{
    Console.WriteLine("gRPC test response: " + ex.StatusCode);
    Console.WriteLine(ex.ToString());
}

Console.WriteLine("Fim");