using Microsoft.Extensions.Logging;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Encodings.Web;
using System.Text.Json;

namespace MicrosoftLogging_Sample.MyLogger;

// https://learn.microsoft.com/en-us/dotnet/core/extensions/custom-logging-provider
// https://www.treinaweb.com.br/blog/criando-um-provider-customizado-para-o-microsoft-extensions-logging

public class MyLogger : ILogger
{
    private static readonly JsonSerializerOptions JsonOptions = new JsonSerializerOptions
    {
        Encoder = JavaScriptEncoder.UnsafeRelaxedJsonEscaping // Imprimir caracteres especiais
    };

    private readonly string _categoryName;
    private readonly IExternalScopeProvider _scopeProvider = new LoggerExternalScopeProvider();

    public MyLogger(string categoryName)
    {
        _categoryName = categoryName;
    }

    public IDisposable BeginScope<TState>(TState state) where TState : notnull => _scopeProvider.Push(state);

    public bool IsEnabled(LogLevel logLevel) => true;

    public void Log<TState>(LogLevel logLevel, EventId eventId, TState state, Exception? exception, Func<TState, Exception?, string> formatter)
    {
        var message = new StringBuilder(formatter(state, exception));
        if (exception != null)
            message
                .Append(Environment.NewLine)
                .Append(Environment.NewLine)
                .Append(exception.ToString());

        var (properties, scopes) = GetPropertiesAndScopes(state);

        var model = new Dictionary<string, object?>
        {
            { "Timestamp", DateTime.Now },
            { "Level", logLevel.ToString() },
            { "Message", message.ToString() },
            { "Logger", _categoryName },
            { "Properties", properties },
            { "Scopes", scopes },
        };

        var logText = JsonSerializer.Serialize(model, JsonOptions);
        Console.WriteLine(logText);
    }

    /// <summary>
    /// Gerar dicionário com propriedades adicionais injetadas na mensagem e nos scopes + mensagens formatadas dos scopes
    /// </summary>
    /// <returns></returns>
    private (Dictionary<string, object?>? Properties, IEnumerable<string>? Scopes) GetPropertiesAndScopes<TState>(TState state)
    {
        Dictionary<string, object?> properties;
        var messages = new List<string>();

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
                var scopeProperties = stateItems
                    .Where(x => x.Key != "{OriginalFormat}")
                    .ToDictionary(x => x.Key, x => x.Value);
                foreach (var (key, value) in scopeProperties)
                    properties.TryAdd(key, value);

                //Microsoft.Extensions.Logging.FormattedLogValues
                // Se for dicionário é porque o usuário iniciou um scope sem texto
                if (scope != null && scope is not IDictionary)
                {
                    try
                    {
                        var scopedMessage = scope.ToString()!;
                        messages.Add(scopedMessage);
                    }
                    catch (Exception ex)
                    {
                        InternalLogError(ex, "Erro ao gerar mensagem de scope.");
                    }
                }
            }
        }, default(TState));

        return (
            Properties: properties.Any() ? properties : null,
            Scopes: messages.Any() ? messages : null);
    }

    private void InternalLogError(Exception ex, string message) =>
        Console.WriteLine($"[{typeof(MyLogger).FullName}] [Error] {message}{Environment.NewLine}{Environment.NewLine}{ex}");
}