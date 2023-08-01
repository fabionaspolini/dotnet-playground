using OpenTelemetry.Resources;
using OpenTelemetry;
using OpenTelemetry.Trace;
using System.Diagnostics;
using Microsoft.Extensions.Logging;
using System.Net.Http.Json;

Console.WriteLine(".:: Open Telemetry ::.");

const string ServiceName = "open-telemetry-console-playground";
const string ServiceVersion = "1.0.0";

var loggerFactory = LoggerFactory.Create(builder => builder
    .SetMinimumLevel(LogLevel.Information)
    .AddConsole());

var _logger = loggerFactory.CreateLogger<Program>();
var _httpClient = new HttpClient()!;

using var tracerProvider = Sdk.CreateTracerProviderBuilder()
    .AddSource(ServiceName)
    .SetResourceBuilder(ResourceBuilder.CreateDefault()
        .AddService(serviceName: ServiceName, serviceVersion: ServiceVersion))
    .AddHttpClientInstrumentation()
    .SetSampler(new AlwaysOnSampler())
    // 4317 -> Collector: accept OpenTelemetry Protocol (OTLP) over gRPC, if enabled
    .AddOtlpExporter(opts => opts.Endpoint = new Uri("http://localhost:4317"))
    .AddConsoleExporter()
    .Build()!;

// OpenTelemetry.Exporter.Jaeger: Exportação para agent Jaeger é depreciada. Dê preferência ao padrão OTLP (Open Telemetry Protocol).
//.AddJaegerExporter(exporter =>
// {
//     exporter.AgentHost = "localhost";
//     exporter.AgentPort = 6831;
//     //exporter.Endpoint = new Uri("http://localhost14268:/api/traces");
// })

ActivitySource _myActivitySource = new ActivitySource(ServiceName, ServiceVersion);

try
{
    //Calcular(10, 2, "somar");
    //Calcular(15, 3, "subtrair");
    //Calcular(15, 3, "teste");
    await CalcularEmLote(simulateError: false);
    await ConsultarCep("01153000"); // ok
    await ConsultarCep("01153988"); // status: 200, erro: true
    await ConsultarCep("011539xx"); // status: 400
}
catch (Exception) { }

tracerProvider.ForceFlush();
tracerProvider.Shutdown();


// ----- Http Client -----
async Task ConsultarCep(string cep)
{
    using var span = _myActivitySource.StartActivity("consultar-cep");
    try
    {
        //var response = await httpClient.GetAsync($"https://viacep.com.br/ws/{cep}/json/");
        var response = await _httpClient.GetAsync($"https://viacep.com.br/ws/{cep}/json/");
        if (response.IsSuccessStatusCode)
        {
            var body = (await response.Content.ReadFromJsonAsync<CepResponse>())!;

            if (body.erro)
                span?.SetStatus(Status.Error);
            else
            {
                _logger.LogInformation($"Cidade: {body.localidade}");
                span?.SetStatus(Status.Ok);
            }
        }
        else
            span?.SetStatus(Status.Error);
    }
    catch (Exception ex)
    {
        span?.SetStatus(Status.Error.WithDescription(ex.Message));
    }
}

// ----- Cálculos -----

Task CalcularEmLote(bool simulateError) => Task.Run(() =>
{
    using var span = _myActivitySource.StartActivity("lote-operacoes");
    try
    {
        var task = Task.Run(() => Calcular(5, 6, "somar"));
        Calcular(2, 3, "somar");
        Calcular(5, 3, "subtrair");
        if (simulateError)
            Calcular(15, 3, "teste");
        task.Wait();
        span.SetStatus(Status.Ok);
    }
    catch (Exception ex)
    {
        span.SetStatus(Status.Error.WithDescription(ex.Message));
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
            _ => throw new ArgumentOutOfRangeException($"Operação não suportada: {op}")
        };
        span.SetTag("resultado", result);
        span.SetStatus(Status.Ok);
        _logger.LogInformation("Concluído");
    }
    catch (Exception ex)
    {
        _logger.LogCritical(ex, "Erro ao calcular");
        //span.SetStatus(Status.Error);
        span.SetStatus(Status.Error.WithDescription(ex.Message));
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