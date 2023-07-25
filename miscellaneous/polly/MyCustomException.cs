using System;

namespace PollyPlayground
{
    public class MyCustomException : Exception
{

    public MyCustomException(string message) : base(message)
    {
    }
}
}
