## NLog :heart:

- Personalização do layout
- 93 targets suportados para log, entre os principais: Console, Arquivo, Elasticsearch, AWS Cloud Watch, Azure.
- Suporte para logar automaticamente o nome do método que invocou a geração de log através do render [`${callsite}`](https://github.com/NLog/NLog/wiki/Callsite-Layout-Renderer)
- Suporte a layout com condicionais através do render [`${when}`](https://github.com/NLog/NLog/wiki/When-Layout-Renderer)
- Suporte ao pattern de Correlation Id através do render [`${activityid}`](https://github.com/NLog/NLog/wiki/Trace-Activity-Id-Layout-Renderer). Útil principalmente em aplicações web.
  - ```Trace.CorrelationManager.ActivityId = Guid.NewGuid();```
- Layout diferente por target
- [Lista completa de configurações](https://nlog-project.org/config/?tab=layout-renderers)
 