# Tracing

- [Open Tracing Specification](https://github.com/opentracing/specification/blob/master/specification.md)

## Jaeger

Modelo de representação do Jaeger é inspirado no Open Tracing, sendo muito similar, apenas com nomenclaturas diferentes ([Detalhes](https://www.jaegertracing.io/docs/1.47/architecture/#terminology)).

### Terminologia

- Span: Unidade de trabalho. Possui nome da operação, hora de início e duração. Podem estar aninhados e ordenados para modelar relacionamentos.
- Trace: Representa os dados ou o fluxo de execução pelo sistema. É composto de um ou vários spans.
- Baggage: Metadados adicionais anexados ao span e propagados pelo SDK de tracing ([W3C Baggage]([https://www.w3.org/TR/baggage)).

### Componentes do serviço

- **jaeger-agent**: Recebe spans dos clientes Jaeger e encaminha para o coletor. Projetado para ser executado como sidecar ou host agent.
- **jaeger-collect**: Recebe spans de agentes ou diretamente de clientes e os salva em armazenamento persistente.
- **jaeger-query**: Serve Jaeger UI e uma API que recupera rastreamentos do armazenamento.
- **jaeger-ingester**: Uma alternativa ao coletor; lê spans do tópico Kafka e os salva no armazenamento.
- **jaeger-remote-storage**: Um serviço que implementa a API de armazenamento remoto em cima de outro back-end compatível.
  Pode ser usado para compartilhar um back-end de armazenamento de nó único, como memória, em vários processos Jaeger.
- **jaeger-operator**: Um Kubernetes Operator para empacotar, implantar e gerenciar a instalação do Jaeger.

Lista com todos componentes: <https://www.jaegertracing.io/download>

### Local server all-in-one

```bash
docker run -d --name jaeger \
  -e COLLECTOR_ZIPKIN_HOST_PORT=:9411 \
  -e COLLECTOR_OTLP_ENABLED=true \
  -p 6831:6831/udp \
  -p 6832:6832/udp \
  -p 5778:5778 \
  -p 16686:16686 \
  -p 4317:4317 \
  -p 4318:4318 \
  -p 14250:14250 \
  -p 14268:14268 \
  -p 14269:14269 \
  -p 9411:9411 \
  jaegertracing/all-in-one:1.47
```

Jaeger UI: <http://localhost:16686>

Lista de cada endpoint: <https://www.jaegertracing.io/docs/1.47/getting-started/#all-in-one>

```
docker run \
  --rm \
  --link jaeger \
  --env OTEL_EXPORTER_JAEGER_ENDPOINT=http://jaeger:14268/api/traces \
  -p8080-8083:8080-8083 \
  jaegertracing/example-hotrod:latest \
  all
```


### dotnet

:warning: Library [jaeger-client-cshap](https://github.com/jaegertracing/jaeger-client-csharp)(Jaeger + Jaeger.Core) foi descontinuada e a instrução é utilizar [opentracing-csharp](https://github.com/opentracing/opentracing-csharp) (OpenTracing).


- [Getting Started OpenTracing](https://opentelemetry.io/docs/instrumentation/net/getting-started/)
- [Cart sample](https://opentelemetry.io/docs/demo/services/cart/)