using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Logging;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace MicrosoftLogging_Sample.MyLogger;

// https://www.treinaweb.com.br/blog/criando-um-provider-customizado-para-o-microsoft-extensions-logging

public record MyLogModel(
    DateTime Timestamp,
    [property: JsonConverter(typeof(JsonStringEnumConverter))] LogLevel Level,
    string Message,
    string Logger,
    Dictionary<string, object?>? Properties,
    IEnumerable<string> Scopes);

public class MyLogger : ILogger
{
    private readonly string _categoryName;

    public MyLogger(string categoryName)
    {
        _categoryName = categoryName;
    }

    private IExternalScopeProvider _scopeProvider = new LoggerExternalScopeProvider();
    public IDisposable BeginScope<TState>(TState state) where TState : notnull => _scopeProvider.Push(state);

    public bool IsEnabled(LogLevel logLevel) => true;

    public void Log<TState>(LogLevel logLevel, EventId eventId, TState state, Exception exception, Func<TState, Exception, string> formatter)
    {
        var message = new StringBuilder(formatter(state, exception));
        if (exception != null)
            message
                .Append(Environment.NewLine)
                .Append(Environment.NewLine)
                .Append(exception.ToString());

        var (properties, scopes) = GetProperties(state, formatter);

        var model = new MyLogModel(
            Timestamp: DateTime.Now,
            Level: logLevel,
            Message: message.ToString(),
            Logger: _categoryName,
            Properties: properties,
            Scopes: scopes);

        var logText = JsonSerializer.Serialize(model);
        Console.WriteLine(logText);
    }

    /// <summary>
    /// Gerar dicionário com propriedades adicionais injetadas na mensagem e nos scopes
    /// </summary>
    /// <returns></returns>
    private (Dictionary<string, object?>? Properties, IEnumerable<string> Scopes) GetProperties<TState>(TState state, Func<TState, Exception, string> formatter)
    {
        Dictionary<string, object?> properties;
        var scopedMessages = new List<string>();

        if (state is IEnumerable<KeyValuePair<string, object?>> stateItems)
        {
            properties = stateItems
                .Where(x => x.Key != "{OriginalFormat}")
                .ToDictionary(x => x.Key, x => x.Value);
        }
        else
            properties = new();

        _scopeProvider.ForEachScope((scope, acc) =>
        {
            if (scope is IEnumerable<KeyValuePair<string, object?>> stateItems)
            {
                //var originalFormat = stateItems.FirstOrDefault("{OriginalFormat}");
                //if (originalFormat != null)
                //{
                //    var scopedMessage = formatter();
                //}

                var scopeProperties = stateItems
                    .Where(x => x.Key != "{OriginalFormat}")
                    .ToDictionary(x => x.Key, x => x.Value);
                foreach (var (key, value) in scopeProperties)
                    properties.TryAdd(key, value);

                //Microsoft.Extensions.Logging.FormattedLogValues
                // Se for dicionário é porque o usuário iniciou um scope sem texto
                if (scope != null && scope is not IDictionary)
                {
                    var scopedMessage = scope.ToString()!;
                    scopedMessages.Add(scopedMessage);
                }
            }
        }, default(TState));

        return (properties, scopedMessages);
    }
}


public class MyLoggerProvider : ILoggerProvider
{
    public ILogger CreateLogger(string categoryName) => new MyLogger(categoryName);

    public void Dispose() { }
}

public static class MyLoggerExtensions
{
    public static ILoggingBuilder AddMyLogger(this ILoggingBuilder builder)
    {
        builder.Services.TryAddEnumerable(ServiceDescriptor.Singleton<ILoggerProvider, MyLoggerProvider>());
        return builder;
    }
}