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
        return Task.FromResult(new HelloReply
        {
            Message = "Hello " + request.Name
        });
    }*/

    public override async Task SayHello(HelloRequest request, IServerStreamWriter<HelloReply> responseStream, ServerCallContext context)
    {
        for (var i = 0; i < request.Count; i++)
        {
            await responseStream.WriteAsync(new HelloReply { Index = i, Message = $"{i:N0} -> Hello {request.Name}" });
            if (request.UpperResult)
                await responseStream.WriteAsync(new HelloReply { Index = i, Message = $"{i:N0} -> Hello {request.Name.ToUpper()}" });
            if (request.LowerResult)
                await responseStream.WriteAsync(new HelloReply { Index = i, Message = $"{i:N0} -> Hello {request.Name.ToLower()}" });
        }
    }
}
