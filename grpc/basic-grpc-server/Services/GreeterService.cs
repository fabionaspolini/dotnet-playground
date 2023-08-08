using Grpc.Core;

namespace BasicGrpcServerPlayground.Services;

public class GreeterService : Greeter.GreeterBase
{
    private readonly ILogger<GreeterService> _logger;
    public GreeterService(ILogger<GreeterService> logger)
    {
        _logger = logger;
    }

    public override Task<HelloReply> SayHello(HelloRequest request, ServerCallContext context)
    {
        //throw new Exception("Server exception test!"); -> Client status: UNKNOWN
        //throw new ArgumentOutOfRangeException("test", "Server out of range exception test!"); // -> Client status: UNKNOWN
        //throw new RpcException(new Status(StatusCode.OutOfRange, "Server rpc out of range"));
        return Task.FromResult(new HelloReply
        {
            Message = "Hello " + request.Name
        });
    }
}
