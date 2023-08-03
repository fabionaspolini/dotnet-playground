using OpenTelemetry.Resources;
using OpenTelemetry;
using OpenTelemetry.Trace;
using System.Diagnostics;
using Microsoft.Extensions.Logging;
using System.Net.Http.Json;
using Npgsql;
using System.Security.Cryptography;
using OpenTelemetryConsolePlayground;
using Microsoft.EntityFrameworkCore;

Console.WriteLine(".:: Open Telemetry ::.");

const string ServiceName = "open-telemetry-console-playground";
const string ServiceVersion = "1.0.0";

const string ConnectionString = "Server=127.0.0.1;Port=5432;Database=teste;User Id=postgres;Password=123456;";

var loggerFactory = LoggerFactory.Create(builder => builder
    .SetMinimumLevel(LogLevel.Information)
    /*.Configure(x => x.ActivityTrackingOptions = ActivityTrackingOptions.SpanId |
                        ActivityTrackingOptions.TraceId |
                        ActivityTrackingOptions.ParentId |
                        ActivityTrackingOptions.Tags |
                        ActivityTrackingOptions.Baggage)
    .AddJsonConsole(opts =>
    {
        opts.IncludeScopes = true;
    }));*/
    .AddConsole());

ILogger<Program> _logger = loggerFactory.CreateLogger<Program>();
HttpClient _httpClient = new HttpClient();

using var tracerProvider = Sdk.CreateTracerProviderBuilder()
    .AddSource(ServiceName)
    .SetResourceBuilder(ResourceBuilder.CreateDefault()
        .AddService(serviceName: ServiceName, serviceVersion: ServiceVersion))
    .AddHttpClientInstrumentation(opts => opts.RecordException = true)
    .AddNpgsql()
    .AddEntityFrameworkCoreInstrumentation(x =>
    {
        // Instrumentação do EF não oferece muita informação, apenas a query executada.
        // A instrumentação do DbConnection com Npgsql oferece isso e um pouco mais. Não precisa usar os dois.
        x.SetDbStatementForText = true;
        x.EnrichWithIDbCommand = (act, cmd) => act.DisplayName = "EF " + act.DisplayName;
    })
    .SetSampler(new AlwaysOnSampler())
    // 4317 -> Collector: accept OpenTelemetry Protocol (OTLP) over gRPC, if enabled
    .AddOtlpExporter(opts => opts.Endpoint = new Uri("http://localhost:4317"))
    //.AddZipkinExporter(opts => opts.Endpoint = new Uri("http://localhost:9511"))
    .AddConsoleExporter()
    .Build()!;

// OpenTelemetry.Exporter.Jaeger: Exportação para agent Jaeger é depreciada. Dê preferência ao padrão OTLP (Open Telemetry Protocol).
//.AddJaegerExporter(exporter =>
// {
//     exporter.AgentHost = "localhost";
//     exporter.AgentPort = 6831;
//     //exporter.Endpoint = new Uri("http://localhost14268:/api/traces");
// })


// Tracing com objetos da Library OpenTelemetry.
// Internamente iniciam a Activity nativa do .net core, apenas possuem outros nomes de propriedades para os mesmo objetivos.
/*var tracer = tracerProvider.GetTracer(ServiceName, ServiceVersion);
using (var tempSpan = tracer.StartActiveSpan("temp-span"))
{
    var teste = Activity.Current;
    using (var subspan1 = tracer.StartActiveSpan("temp-sub-span-1")) ;
    _logger.LogInformation("Temp span");
    using (var subspan2 = tracer.StartActiveSpan("temp-sub-span-2")) ;
}*/

ActivitySource _myActivitySource = new ActivitySource(ServiceName, ServiceVersion);
using (var rootSpan = _myActivitySource.StartActivity("execute")) // Somente para agrupar todas as childs da execução do app num span raiz
{
    try
    {
        const bool simultaneo = false;
        if (simultaneo)
        {
            // --- Execução simultânea ---
            var tasks = new List<Task>
            {
                CalcularEmLote(simulateException: false),
                ConsultarCep("01153000"), // ok
                ConsultarCep("01153988"), // status: 200, erro: true
                ConsultarCep("011539xx"), // status: 400
                ConsultarWeatherForecast(),
                ConsultarDatabaseNative(simulateException: false),
                ConsultarDatabaseNative(simulateException: true),
                ConsultarDatabaseComEntityFramework(simulateException: false),
                ConsultarDatabaseComEntityFramework(simulateException: true)
            };
            Task.WaitAll(tasks.ToArray());
        }
        else
        {
            // --- Execução sequencial ---
            await CalcularEmLote(simulateException: false);
            await ConsultarCep("01153000"); // ok
            await ConsultarCep("01153988"); // status: 200, erro: true
            await ConsultarCep("011539xx"); // status: 400
            await ConsultarWeatherForecast();
            await ConsultarDatabaseNative(simulateException: false);
            await ConsultarDatabaseNative(simulateException: true);
            await ConsultarDatabaseComEntityFramework(simulateException: false);
            await ConsultarDatabaseComEntityFramework(simulateException: true);
        }

        // --- Outros testes ---
        //Calcular(10, 2, "somar");
        //Calcular(15, 3, "subtrair");
        //Calcular(15, 3, "teste");
    }
    catch (Exception) { }
}
tracerProvider.ForceFlush();
tracerProvider.Shutdown();

// ----- Consulta com Entity Framework -----
async Task ConsultarDatabaseComEntityFramework(bool simulateException)
{
    // Se não criar o span, a requisição http ainda é trackeada, porém ficam como dois processo root sem vinculo neste exemplo
    using var span = _myActivitySource.StartActivity("consultar-database-com-entity-framework")!;
    try
    {
        var options = new DbContextOptionsBuilder().UseNpgsql(ConnectionString).Options;
        using var conn = new TesteContext(options);
        if (simulateException)
            await conn.PessoasSimulateException.ToArrayAsync();
        else
            await conn.Pessoas.ToArrayAsync();
        span.SetStatus(Status.Ok);
    }
    catch (Exception ex)
    {
        span.SetStatus(ActivityStatusCode.Error, ex.Message);
        //span.RecordException(ex); // Gravado automaticamente pela instrumentação do PG
    }
}

// ----- Consulta SQL Nativa -----
async Task ConsultarDatabaseNative(bool simulateException)
{
    // Se não criar o span, a requisição http ainda é trackeada, porém ficam como dois processo root sem vinculo neste exemplo
    using var span = _myActivitySource.StartActivity("consultar-database-native")!;
    try
    {
        using var conn = new NpgsqlConnection(ConnectionString);
        await conn.OpenAsync();
        using var cmd = conn.CreateCommand();
        cmd.CommandText = simulateException ? @"select * from pessoa_aaaaaaaa" : @"select * from pessoa";
        using var reader = await cmd.ExecuteReaderAsync();
        while (reader.Read()) ;
        span.SetStatus(Status.Ok);
    }
    catch (Exception ex)
    {
        span.SetStatus(ActivityStatusCode.Error, ex.Message);
        //span.RecordException(ex); // Gravado automaticamente pela instrumentação do PG
    }
}

// ----- Consumir open-telemetry-web-api-playground ----
async Task ConsultarWeatherForecast()
{
    // Se não criar o span, a requisição http ainda é trackeada, porém ficam como dois processo root sem vinculo neste exemplo
    using var span = _myActivitySource.StartActivity("consultar-weather-forecast")!;
    try
    {
        var html = await _httpClient.GetStringAsync("https://example.com/");
        var response = await _httpClient.GetAsync("http://localhost:5237/WeatherForecast");
        span.SetStatus(response.IsSuccessStatusCode ? Status.Ok : Status.Error);
    }
    catch (Exception ex)
    {
        span.SetStatus(ActivityStatusCode.Error, ex.Message);
        //span.RecordException(ex); // Gravado automaticamente pela configuração da instrumentação
    }
}


// ----- Http Client -----
async Task ConsultarCep(string cep)
{
    using var span = _myActivitySource.StartActivity("consultar-cep")!;
    try
    {
        //var response = await httpClient.GetAsync($"https://viacep.com.br/ws/{cep}/json/");
        var response = await _httpClient.GetAsync($"https://viacep.com.br/ws/{cep}/json/");
        if (response.IsSuccessStatusCode)
        {
            var body = (await response.Content.ReadFromJsonAsync<CepResponse>())!;

            if (body.erro)
                span.SetStatus(ActivityStatusCode.Error, "Cep não encontrado");
            else
            {
                _logger.LogInformation($"Cidade: {body.localidade}");
                span.SetStatus(Status.Ok);
            }
        }
        else
            span.SetStatus(ActivityStatusCode.Error, "Cep em formato inválido");
    }
    catch (Exception ex)
    {
        span.SetStatus(ActivityStatusCode.Error, ex.Message);
        span.RecordException(ex);
    }
}

// ----- Cálculos -----

Task CalcularEmLote(bool simulateException) => Task.Run(() =>
{
    using var span = _myActivitySource.StartActivity("lote-operacoes")!;
    try
    {
        var task = Task.Run(() => Calcular(5, 6, "somar"));
        Calcular(2, 3, "somar");
        Calcular(5, 3, "subtrair");
        if (simulateException)
            Calcular(15, 3, "teste");
        task.Wait();
        span.SetStatus(Status.Ok);
    }
    catch (Exception ex)
    {
        span.SetStatus(ActivityStatusCode.Error, ex.Message);
        span.RecordException(ex); // Salvar na seção "logs" do Jaeger (events OTLP)
        //throw;
    }
});

void Calcular(int a, int b, string op)
{
    using var span = _myActivitySource.StartActivity("calcular")!;
    try
    {
        span.SetTag(nameof(a), a);
        span.SetTag(nameof(b), b);
        span.SetTag(nameof(b), b);
        span.AddBaggage("root-baggage", "xxxxxxxxx");

        _logger.LogInformation("Iniciando...");

        var result = op switch
        {
            "somar" => Somar(a, b),
            "subtrair" => Subtrair(a, b),
            _ => throw new ArgumentOutOfRangeException(nameof(op), $"Operação não suportada: {op}")
        };
        span.SetTag("resultado", result);
        span.SetStatus(Status.Ok);
        _logger.LogInformation("Concluído");
    }
    catch (Exception ex)
    {
        _logger.LogCritical(ex, "Erro ao calcular");
        span.SetStatus(ActivityStatusCode.Error, ex.Message);
        span.RecordException(ex);
        throw;
    }
}

int Somar(int a, int b)
{
    using var span = _myActivitySource.StartActivity("somar")!;

    // Baggage não é armazenado pelo collector e não será exibido na interface do Jaeger (https://opentelemetry.io/docs/specs/status/#baggage)
    // Baggages de spans pais são acessíveis aqui, mas o que é adicionado aqui não volta para o pai.
    span.AddBaggage("teste", "aaa");
    span.AddBaggage("teste-2", "bbb");
    Thread.Sleep(Random.Shared.Next(50, 500));
    span.AddEvent(new ActivityEvent("mensagem de log"));
    span.SetStatus(Status.Ok);
    return a + b;
}

int Subtrair(int a, int b)
{
    using var span = _myActivitySource.StartActivity("subtrair")!;
    Thread.Sleep(Random.Shared.Next(50, 500));
    span.SetStatus(Status.Ok);
    return a - b;
}