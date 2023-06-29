using Microsoft.Extensions.Logging;

namespace MicrosoftLogging_Sample.MyLogger;

public class MyLoggerProvider : ILoggerProvider
{
    public ILogger CreateLogger(string categoryName) => new MyLogger(categoryName);

    public void Dispose() { }
}