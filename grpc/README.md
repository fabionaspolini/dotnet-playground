# gRPC

- [Tutorial para criar server e client](https://learn.microsoft.com/en-us/aspnet/core/tutorials/grpc/grpc-start?view=aspnetcore-7.0&tabs=visual-studio)
- [Performance gRPC](https://learn.microsoft.com/en-us/aspnet/core/grpc/performance?view=aspnetcore-7.0)
- [Status code](https://grpc.github.io/grpc/core/md_doc_statuscodes.html) / [From Google](https://developers.google.com/maps-booking/reference/grpc-api/status_codes?hl=pt-br)
- [protobuf data types](https://protobuf.dev/programming-guides/proto3/)
- [.net decimal support](https://learn.microsoft.com/en-us/dotnet/architecture/grpc-for-wcf-developers/protobuf-data-types)

## Stream Sample

Benchmarks simples com debug information.

Response stream -> 900.000 itens em 00:00:02.4495950


<https://www.stevejgordon.co.uk/server-streaming-with-grpc-in-asp-dotnet-core>

```proto
syntax = "proto3";

import "google/protobuf/empty.proto";
import "google/protobuf/timestamp.proto";

option csharp_namespace = "WeatherForecast";
package WeatherForecast;

service WeatherForecasts {
  rpc GetWeatherStream (google.protobuf.Empty) returns (stream WeatherData);
}

message WeatherData {
  google.protobuf.Timestamp dateTimeStamp = 1;
  int32 temperatureC = 2;
  int32 temperatureF = 3;
  string summary = 4;
}
```

## Benchmark

```
Protocolo: HTTP
5608 requisições gRPC em 00:00:05.0005822
15467 requisições HTTP em 00:00:05.0000037

Protocolo: HTTPS
4263 requisições gRPC em 00:00:05.0005243
15443 requisições HTTP em 00:00:05.0002855
```
