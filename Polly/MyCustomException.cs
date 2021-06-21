using System;

namespace Polly_Sample
{
    public class MyCustomException : Exception
{

    public MyCustomException(string message) : base(message)
    {
    }
}
}
