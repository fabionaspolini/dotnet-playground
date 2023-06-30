using Microsoft.Extensions.Logging.Abstractions;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Console;
using Microsoft.Extensions.Options;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using System.IO;
using System.Runtime.InteropServices;
using System.Text.Json;
using System.Text;
using System;
using MicrosoftLogging_Sample.MyLogger;
using System.Linq;
using System.Diagnostics;
using System.Xml;

namespace MicrosoftLogging_Sample.MyLogger
{

    // Exemplo baseado no código do formatter sealed Microsoft.Extensions.Logging.Console.JsonConsoleFormatter
    // https://github.com/dotnet/runtime/blob/main/src/libraries/Microsoft.Extensions.Logging.Console/src/JsonConsoleFormatter.cs

    public class MyConsoleLoggerFormatter : ConsoleFormatter, IDisposable
    {
        private const string OriginalFormatKeyName = "{OriginalFormat}";
        private readonly IDisposable? _optionsReloadToken;

        public MyConsoleLoggerFormatter(IOptionsMonitor<JsonConsoleFormatterOptions> options)
            : base("MyConsoleLoggerFormatter")
        {
            ReloadLoggerOptions(options.CurrentValue);
            _optionsReloadToken = options.OnChange(ReloadLoggerOptions);
        }

        public override void Write<TState>(in LogEntry<TState> logEntry, IExternalScopeProvider? scopeProvider, TextWriter textWriter)
        {
            string message = logEntry.Formatter(logEntry.State, logEntry.Exception);
            if (logEntry.Exception == null && message == null)
            {
                return;
            }
            if (logEntry.Exception != null)
                message += Environment.NewLine + Environment.NewLine + logEntry.Exception.ToString();

            LogLevel logLevel = logEntry.LogLevel;
            string category = logEntry.Category;
            int eventId = logEntry.EventId.Id;
            Exception? exception = logEntry.Exception;
            const int DefaultBufferSize = 1024;
            using (var output = new PooledByteBufferWriter(DefaultBufferSize))
            {
                using (var writer = new Utf8JsonWriter(output, FormatterOptions.JsonWriterOptions))
                {
                    writer.WriteStartObject();
                    var timestampFormat = FormatterOptions.TimestampFormat;
                    if (timestampFormat != null)
                    {
                        DateTimeOffset dateTimeOffset = FormatterOptions.UseUtcTimestamp ? DateTimeOffset.UtcNow : DateTimeOffset.Now;
                        writer.WriteString("Timestamp", dateTimeOffset.ToString(timestampFormat));
                    }

                    if (eventId != 0)
                        writer.WriteNumber("EventId", eventId);
                    writer.WriteString("Level", GetLogLevelString(logLevel));
                    writer.WriteString("Message", message);
                    writer.WriteString("Logger", category);

                    WriteEnvironmentInformation(writer, scopeProvider, logEntry);
                    WriteScopeInformation(writer, scopeProvider);

                    writer.WriteEndObject();
                    writer.Flush();
                }
#if NETCOREAPP
                textWriter.Write(Encoding.UTF8.GetString(output.WrittenMemory.Span));
#else
                textWriter.Write(Encoding.UTF8.GetString(output.WrittenMemory.Span.ToArray()));
#endif
            }
            textWriter.Write(Environment.NewLine);
        }

        private static bool IsOriginalFormatKey(string key) => key == OriginalFormatKeyName;
        private static bool HasOriginalFormatKey(IEnumerable<KeyValuePair<string, object?>> items) => items.Any(x => x.Key == OriginalFormatKeyName);

        private static string GetLogLevelString(LogLevel logLevel)
        {
            return logLevel switch
            {
                LogLevel.Trace => "Trace",
                LogLevel.Debug => "Debug",
                LogLevel.Information => "Information",
                LogLevel.Warning => "Warning",
                LogLevel.Error => "Error",
                LogLevel.Critical => "Critical",
                _ => throw new ArgumentOutOfRangeException(nameof(logLevel))
            };
        }

        private void WriteEnvironmentInformation<TState>(Utf8JsonWriter writer, IExternalScopeProvider? scopeProvider, LogEntry<TState> logEntry)
        {
            writer.WriteStartObject("Envs");

            // Variáveis da mensagem sendo logada
            if (logEntry.State is IEnumerable<KeyValuePair<string, object?>> stateItems && stateItems.Any())
                foreach (var item in stateItems)
                    if (!IsOriginalFormatKey(item.Key))
                        WriteItem(writer, item);

            // Variáveis nos scopes e na tags da activity current
            scopeProvider?.ForEachScope((scope, state) =>
            {
                if (scope is IEnumerable<KeyValuePair<string, object?>> scopeItems && scopeItems.Any())
                {
                    foreach (var item in scopeItems)
                        if (!IsOriginalFormatKey(item.Key))
                            WriteItem(writer, item);
                }
            }, writer);

            // Tags das activities parent
            var activity = Activity.Current?.Parent;
            while (activity != null)
            {
                if (activity.Tags.Any())
                    foreach (var tag in activity.TagObjects)
                        WriteItem(writer, tag);
                activity = activity.Parent;
            }

            writer.WriteEndObject();
        }

        private void WriteScopeInformation(Utf8JsonWriter writer, IExternalScopeProvider? scopeProvider)
        {
            // - Scopes com mensagens textuais. Dicionários são ignorados.
            // - Activity baggage (Concatenado todos os textos numa única linha).
            if (FormatterOptions.IncludeScopes && scopeProvider != null)
            {
                writer.WriteStartArray("Scopes");
                scopeProvider.ForEachScope((scope, state) =>
                {
                    if (scope is IEnumerable<KeyValuePair<string, object?>> scopeItems)
                    {
                        if (HasOriginalFormatKey(scopeItems))
                            state.WriteStringValue(scope.ToString());
                    }
                    else
                        state.WriteStringValue(ToInvariantString(scope));
                }, writer);
                writer.WriteEndArray();
            }
        }

        private static void WriteItem(Utf8JsonWriter writer, KeyValuePair<string, object?> item)
        {
            var key = item.Key;
            switch (item.Value)
            {
                case bool boolValue:
                    writer.WriteBoolean(key, boolValue);
                    break;
                case byte byteValue:
                    writer.WriteNumber(key, byteValue);
                    break;
                case sbyte sbyteValue:
                    writer.WriteNumber(key, sbyteValue);
                    break;
                case char charValue:
#if NETCOREAPP
                    writer.WriteString(key, MemoryMarshal.CreateSpan(ref charValue, 1));
#else
                    writer.WriteString(key, charValue.ToString());
#endif
                    break;
                case decimal decimalValue:
                    writer.WriteNumber(key, decimalValue);
                    break;
                case double doubleValue:
                    writer.WriteNumber(key, doubleValue);
                    break;
                case float floatValue:
                    writer.WriteNumber(key, floatValue);
                    break;
                case int intValue:
                    writer.WriteNumber(key, intValue);
                    break;
                case uint uintValue:
                    writer.WriteNumber(key, uintValue);
                    break;
                case long longValue:
                    writer.WriteNumber(key, longValue);
                    break;
                case ulong ulongValue:
                    writer.WriteNumber(key, ulongValue);
                    break;
                case short shortValue:
                    writer.WriteNumber(key, shortValue);
                    break;
                case ushort ushortValue:
                    writer.WriteNumber(key, ushortValue);
                    break;
                case null:
                    writer.WriteNull(key);
                    break;
                default:
                    writer.WriteString(key, ToInvariantString(item.Value));
                    break;
            }
        }

        private static string? ToInvariantString(object? obj) => Convert.ToString(obj, CultureInfo.InvariantCulture);

        internal JsonConsoleFormatterOptions FormatterOptions { get; set; }

        [MemberNotNull(nameof(FormatterOptions))]
        private void ReloadLoggerOptions(JsonConsoleFormatterOptions options)
        {
            FormatterOptions = options;
        }

        public void Dispose()
        {
            _optionsReloadToken?.Dispose();
        }
    }
}

namespace Microsoft.Extensions.Logging
{

    public static class MyConsoleLoggerFormatterExtensions
    {
        /// <summary>
        /// Add and configure a console log formatter named 'json' to the factory.
        /// </summary>
        /// <param name="builder">The <see cref="ILoggingBuilder"/> to use.</param>
        /// <param name="configure">A delegate to configure the <see cref="ConsoleLogger"/> options for the built-in json log formatter.</param>
        public static ILoggingBuilder AddMyJsonFormatterConsole(this ILoggingBuilder builder, Action<JsonConsoleFormatterOptions> configure)
        {
            builder.AddConsole(opts => opts.FormatterName = "MyConsoleLoggerFormatter");
            builder.AddConsoleFormatter<MyConsoleLoggerFormatter, JsonConsoleFormatterOptions>(configure);
            return builder;
            //return builder.AddConsoleWithFormatter<JsonConsoleFormatterOptions>(ConsoleFormatterNames.Json, configure);
        }
    }
}