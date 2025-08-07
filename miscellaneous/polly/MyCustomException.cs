using System;

namespace polly_playground;

public class MyCustomException : Exception
{

    public MyCustomException(string message) : base(message)
    {
    }
}