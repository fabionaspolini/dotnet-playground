﻿using Microsoft.Extensions.Logging.Abstractions;
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

namespace MicrosoftLogging_Sample.MyLogger
{

    // Exemplo baseado no código do formatter sealed Microsoft.Extensions.Logging.Console.JsonConsoleFormatter
    // https://github.com/dotnet/runtime/blob/main/src/libraries/Microsoft.Extensions.Logging.Console/src/JsonConsoleFormatter.cs

    public class MyConsoleLoggerFormatter : ConsoleFormatter, IDisposable
    {
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
                    writer.WriteNumber(nameof(logEntry.EventId), eventId);
                    writer.WriteString(nameof(logEntry.LogLevel), GetLogLevelString(logLevel));
                    writer.WriteString(nameof(logEntry.Category), category);
                    writer.WriteString("Message", message);

                    if (exception != null)
                    {
                        writer.WriteString(nameof(Exception), exception.ToString());
                    }

                    if (logEntry.State != null)
                    {
                        writer.WriteStartObject(nameof(logEntry.State));
                        writer.WriteString("Message", logEntry.State.ToString());
                        if (logEntry.State is IReadOnlyCollection<KeyValuePair<string, object>> stateProperties)
                        {
                            foreach (KeyValuePair<string, object> item in stateProperties)
                            {
                                WriteItem(writer, item);
                            }
                        }
                        writer.WriteEndObject();
                    }
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

        private void WriteScopeInformation(Utf8JsonWriter writer, IExternalScopeProvider? scopeProvider)
        {
            if (FormatterOptions.IncludeScopes && scopeProvider != null)
            {
                writer.WriteStartArray("Scopes");
                scopeProvider.ForEachScope((scope, state) =>
                {
                    if (scope is IEnumerable<KeyValuePair<string, object>> scopeItems)
                    {
                        state.WriteStartObject();
                        state.WriteString("Message", scope.ToString());
                        foreach (KeyValuePair<string, object> item in scopeItems)
                        {
                            WriteItem(state, item);
                        }
                        state.WriteEndObject();
                    }
                    else
                    {
                        state.WriteStringValue(ToInvariantString(scope));
                    }
                }, writer);
                writer.WriteEndArray();
            }
        }

        private static void WriteItem(Utf8JsonWriter writer, KeyValuePair<string, object> item)
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