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

Greeter.GreeterClient client = new Greeter.GreeterClient(channel); // 

// Tests
//await ServerStreamingCallTest();
await ClientStreamingCallTest();



async Task ServerStreamingCallTest()
{
    // Inicia com requisição do cliente e servidor response vários itens
    try
    {
        var count = 0;
        var request = new HelloRequest
        {
            Name = "Teste",
            Count = 1_000_000,
            UpperResult = false,
            LowerResult = false
        };
        var watch = Stopwatch.StartNew();
        using var call = client.SayHello(request);
        await foreach (var response in call.ResponseStream.ReadAllAsync())
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
        Console.WriteLine("ServerStreamingCallTest response: " + ex.StatusCode);
        Console.WriteLine(ex.ToString());
    }
}

async Task ClientStreamingCallTest()
{
    // Client envia vários itens e obtém resultado após conlusão
    try
    {
        const int count = 10_000;
        var request = new ItemRequest
        {
            Nome = "Teste",
            Quantidade = 2,
            Valor = 13
        };
        var watch = Stopwatch.StartNew();
        using var call = client.AddItem();
        for (int i = 0; i < count; i++)
            await call.RequestStream.WriteAsync(request);
        await call.RequestStream.CompleteAsync();
        var response = await call;
        watch.Stop();

        Console.WriteLine($"Concluído - {count:N0} itens em {watch.Elapsed}");
        Console.WriteLine($"Response -> Itens: {response.Itens:N0}, QuantidadeTotal: {response.QuantidadeTotal:N0}, ValorTotal: {response.ValorTotal:N2}");
    }
    catch (RpcException ex)
    {
        Console.WriteLine("ClientStreamingCallTest response: " + ex.StatusCode);
        Console.WriteLine(ex.ToString());
    }
}