# Telemetria

- [OpenTelemetry](https://github.com/open-telemetry/opentelemetry-specification) é o sucessor do [Open Tracing](https://github.com/opentracing/specification/blob/master/specification.md).
- Padrão OTLP (OpenTelemetry Protocol) é suportado por outras ferramentas além do Jaeger, bastando apenas alterar o endpoint para sincronização dos dados.

## Ferramentas

- [Jaeger](https://www.jaegertracing.io/): Jaeger: open source, end-to-end distributed tracing ([JAEGER.md](JAEGER.md)).
- [SigNoz](https://signoz.io): Open Source Observability with Traces, Logs and Metrics in a single pane ([SIGNOZ.md](SIGNOZ.md)). 

### Terminologia

- Span: Unidade de trabalho. Possui nome da operação, hora de início e duração. Podem estar aninhados e ordenados para modelar relacionamentos.
- Trace: Representa os dados ou o fluxo de execução pelo sistema. É composto de um ou vários spans.
- [Baggage](https://opentelemetry.io/docs/specs/status/#baggage): Metadados adicionais anexados ao span e propagados pelo SDK de tracing ([W3C Baggage]([https://www.w3.org/TR/baggage)).  
  Não é um dado armazenado pelo coletor e não é exibido na interface de exploração do tracing.
- Status: Ok, Unset ou Error. Permite anexar descrição adicional.

## dotnet OpenTelemetry SDK

:warning: Library [jaeger-client-cshap](https://github.com/jaegertracing/jaeger-client-csharp)(Jaeger + Jaeger.Core) foi descontinuada e a instrução é utilizar [opentracing-csharp](https://github.com/opentracing/opentracing-csharp) (OpenTracing).

- [Getting Started OpenTracing](https://opentelemetry.io/docs/instrumentation/net/getting-started/)
- [Cart sample](https://opentelemetry.io/docs/demo/services/cart/)
- `OpenTelemetry.Trace.SemanticConventions`: Classe com convensões padrões
- Padrões para ingestão dos headers no tracing distribuido (RabbitMQ, Kafka e atividade em geral que não possui instrumentações nativas)
	- [Projeto .net](https://github.com/open-telemetry/opentelemetry-dotnet/tree/main/examples/MicroserviceExample)
	- [Convenções de headers](https://github.com/open-telemetry/opentelemetry-specification/blob/main/specification/trace/semantic_conventions/messaging.md#span-name)