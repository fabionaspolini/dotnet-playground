using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Configs;
using BenchmarkDotNet.Environments;
using BenchmarkDotNet.Jobs;
using BenchmarkDotNet.Running;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Security.Cryptography;
using System.Text.Encodings.Web;
using System.Threading;
using static System.Console;

namespace MicrosoftLoggingPlayground
{
    [SimpleJob(RuntimeMoniker.Net70)]
    [RPlotExporter]
    public class Md5VsSha256
    {
        private SHA256 sha256 = SHA256.Create();
        private MD5 md5 = MD5.Create();
        private byte[]? data;

        [Params(1000, 10000)]
        public int N;

        [GlobalSetup]
        public void Setup()
        {
            data = new byte[N];
            new Random(42).NextBytes(data);
        }

        [Benchmark]
        public byte[] Sha256() => sha256.ComputeHash(data!);

        [Benchmark]
        public byte[] Md5() => md5.ComputeHash(data!);
    }

    class Program
    {
        static void Main(string[] args)
        {
            WriteLine(".:: Microsoft Logging - Benchmark ::.");

            Trace.CorrelationManager.ActivityId = Guid.NewGuid();

            var config = BuildConfig();
            var serviceProvider = BuildServices(config);

            using var scope = serviceProvider.CreateScope();

            var logger = scope.ServiceProvider.GetRequiredService<ILogger<Program>>();

            logger.LogInformation("Iniciando aplicação");


            var summary = BenchmarkRunner.Run(typeof(Program).Assembly);

            logger.LogInformation("Fim");
        }

        private static IConfigurationRoot BuildConfig() => new ConfigurationBuilder()
            .SetBasePath(System.IO.Directory.GetCurrentDirectory())
            .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
            .Build();

        private static IServiceProvider BuildServices(IConfigurationRoot config) => new ServiceCollection()
                .AddSingleton(config)
                .AddLogging(builder =>
                {
                    builder.ClearProviders();
                    builder.AddConfiguration(config.GetSection("Logging"));

                    // Adicionar scope no log com o traceId gerado com new Activity("...")
                    builder.Configure(x => x.ActivityTrackingOptions = ActivityTrackingOptions.SpanId |
                        ActivityTrackingOptions.TraceId |
                        ActivityTrackingOptions.ParentId |
                        ActivityTrackingOptions.Tags |
                        ActivityTrackingOptions.Baggage);

                    /*builder.AddConsole(options =>
                    {
                        options.IncludeScopes = true;
                        options.TimestampFormat = "dd/MM/yyyy HH:mm:ss.fff ";
                    });*/
                    /*builder.AddSimpleConsole(options =>
                    {
                        options.IncludeScopes = true;
                        options.SingleLine = true;
                        options.TimestampFormat = "dd/MM/yyyy HH:mm:ss.fff ";
                    });*/
                    /*builder.AddJsonConsole(x =>
                    {
                        x.IncludeScopes = true;
                        x.JsonWriterOptions = new() { Indented = true }; // Indentação causa muita queda de performance
                    });*/
                    //builder.AddMyLogger();
                    builder.AddMyJsonFormatterConsole(x =>
                    {
                        x.IncludeScopes = true;
                        x.TimestampFormat = "O";
                        x.JsonWriterOptions = new()
                        {
                            Indented = false, // Indentação causa muita queda de performance
                            Encoder = JavaScriptEncoder.UnsafeRelaxedJsonEscaping
                        };
                    });
                })
               .BuildServiceProvider();
    }
}
