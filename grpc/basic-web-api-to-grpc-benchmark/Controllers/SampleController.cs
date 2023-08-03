using Microsoft.AspNetCore.Mvc;

namespace basic_web_api_to_grpc_benchmark_playground.Controllers;

[ApiController]
[Route("")]
public class SampleController : ControllerBase
{
    [HttpPost("say-hello")]
    public HelloReply GetSayHello(HelloRequest request) => new HelloReply
    {
        Message = "Hello " + request.Name
    };
}

public class HelloRequest
{
    public string Name { get; set; }
}

public class HelloReply
{
    public string Message { get; set; }
}