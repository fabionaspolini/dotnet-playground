using Grpc.Core;

namespace StreamGrpcServerPlayground.Services;

public class GreeterService : Greeter.GreeterBase
{
    private readonly ILogger<GreeterService> _logger;
    public GreeterService(ILogger<GreeterService> logger)
    {
        _logger = logger;
    }

    /*public override Task<HelloReply> SayHello(HelloRequest request, ServerCallContext context)
    {
        // Unary call
        return Task.FromResult(new HelloReply
        {
            Message = "Hello " + request.Name
        });
    }*/


    public override async Task SayHello(HelloRequest request, IServerStreamWriter<HelloReply> responseStream, ServerCallContext context)
    {
        // Server streaming call
        for (var i = 0; i < request.Count; i++)
        {
            await responseStream.WriteAsync(new HelloReply { Index = i, Message = $"{i:N0} -> Hello {request.Name}" });
            if (request.UpperResult)
                await responseStream.WriteAsync(new HelloReply { Index = i, Message = $"{i:N0} -> Hello {request.Name.ToUpper()}" });
            if (request.LowerResult)
                await responseStream.WriteAsync(new HelloReply { Index = i, Message = $"{i:N0} -> Hello {request.Name.ToLower()}" });
        }
    }

    public override async Task<ItensResume> AddItem(IAsyncStreamReader<ItemRequest> requestStream, ServerCallContext context)
    {
        // Client streaming call
        var response = new ItensResume();
        await foreach (var item in requestStream.ReadAllAsync())
        {
            response.Itens++;
            response.QuantidadeTotal += item.Quantidade;
            response.ValorTotal += item.Valor;
        }
        return response;
    }

    public async override Task PingBidirectional(IAsyncStreamReader<PingRequest> requestStream, IServerStreamWriter<PongResponse> responseStream, ServerCallContext context)
    {
        await foreach (var item in requestStream.ReadAllAsync())
        {
            for (int i = 0; i < item.ResponseCount; i++)
                await responseStream.WriteAsync(new PongResponse { ResponseIndex = i });
        }
    }
}
