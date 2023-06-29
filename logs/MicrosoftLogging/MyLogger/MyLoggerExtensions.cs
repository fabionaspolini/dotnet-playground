using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Logging;

namespace MicrosoftLogging_Sample.MyLogger;

public static class MyLoggerExtensions
{
    public static ILoggingBuilder AddMyLogger(this ILoggingBuilder builder)
    {
        builder.Services.TryAddEnumerable(ServiceDescriptor.Singleton<ILoggerProvider, MyLoggerProvider>());
        return builder;
    }
}