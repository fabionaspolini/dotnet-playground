[Voltar](../README.md)

**Legenda marcadores:**

- :heart: Preferência do autor
- :heavy_check_mark: Requisito atendido com excelência
- :+1: Requisito atendido oficialmente, mas com deficiências
- :-1: Requisito não atendido oficialmente, mas com alternativas manuais
- :x: Requisito não atendido

| Feature                                       | NLog :heart:          | Microsoft             | Log4Net               | Serilog             |
|-----------------------------------------------|-----------------------|-----------------------|-----------------------|---------------------|
| Arquivo com nome diário                       | :heavy_check_mark:    | :-1:                  | :heavy_check_mark:    | :heavy_check_mark:  |
| Exclusão de arquivos antigos                  | :heavy_check_mark:    | :-1:                  | :heavy_check_mark:    | :heavy_check_mark:  |
| Obter nome do método logando automaticamente  | :heavy_check_mark:    | :x:                   | :heavy_check_mark:    | :-1:                |
| Proxy para Microsoft Logging                  | :heavy_check_mark:    | :heavy_check_mark:    |                       | :heavy_check_mark:  |
| Facilidade para obter Log Manager             | :heavy_check_mark:    | :+1:                  | :+1:                  | :heavy_check_mark:  |
| Documentação                                  | :heavy_check_mark:    | :heavy_check_mark:    | :+1:                  | :heavy_check_mark:  |
| Flush assincrono                              | :heavy_check_mark:    | :heavy_check_mark:    | :x:                   | :heavy_check_mark:  |
| Personalização layout das mensagens           | :heavy_check_mark:    | :x:                   | :heavy_check_mark:    | :heavy_check_mark:  |
| Configuração por arquivos                     | :heavy_check_mark:    | :heavy_check_mark:    | :heavy_check_mark:    | :heavy_check_mark:  |
| Configuração por código                       | :heavy_check_mark:    | :heavy_check_mark:    | :heavy_check_mark:    | :heavy_check_mark:  |
| Logs estruturados                             | :heavy_check_mark:    | :x:                   | :-1:                  | :heavy_check_mark:  |

## NLog :heart:

- :heavy_check_mark: Personalização do layout
- :heavy_check_mark: 93 targets suportados para log, entre os principais: Console, Arquivo, RabbitMQ, Redis, Elasticsearch, AWS Cloud Watch, Azure ([Fonte](https://nlog-project.org/config/)).
- :heavy_check_mark: Suporte para logar automaticamente o nome do método que invocou a geração de log através do render [`${callsite}`](https://github.com/NLog/NLog/wiki/Callsite-Layout-Renderer)
- :heavy_check_mark: Suporte a layout com condicionais através do render [`${when}`](https://github.com/NLog/NLog/wiki/When-Layout-Renderer)
- :heavy_check_mark: Suporte ao pattern de Correlation Id através do render [`${activityid}`](https://github.com/NLog/NLog/wiki/Trace-Activity-Id-Layout-Renderer). Útil principalmente em aplicações web.
  - ```Trace.CorrelationManager.ActivityId = Guid.NewGuid();```
- :heavy_check_mark: Layout diferente por target
- :heavy_check_mark: Ferramenta de visualização de logs [NlogViewer](https://github.com/dojo90/NLogViewer)
- :heavy_check_mark: Suporte a inclusão de propriedades estruturadas adicionais
- :heavy_check_mark: Suporte a logs estruturados com a syntax ao adicionar o identificado entre "{}" na mensagem e indicando o valor como argumentos adicionais da mensagem.
- [Lista completa de configurações](https://nlog-project.org/config/?tab=layout-renderers)

## Microsoft Logging

- :x: Para obter o nome do método que chamou o log é necessário iniciar um escopo manualmente ```using (_logger.BeginScope(nameof(NomeMethodo)))```
- :x: Para armazenar em arquivo texto é necessário componentes de terceiros
- :x: Dificuldade de acesso ao Log Manager
  - Pela convensão é recomendado informar a própria classe na declaração da variável de log para o trace capturar o nome da mesma ```private readonly ILogger<Teste> _logger```.
  - Geralmente instância por injeção de dependência do construtor
  - Influencia no design das classes dependentes por ser necessário adicionar o argumento no construtor também
- :x: Não há suporte a inclusão de propriedades estruturadas adicionais
- :x: A melhor maneira de adicionar suporte a gravação de arquivos é adicionar o proxy provider para o NLog ou Serilog

## Log4Net

- :heavy_check_mark: Personalização do layout
- :-1: Documentação fraca
- :-1: Não possui log no level Trace por padrão, sendo necessário adicionar Helpers para para simular operação
- :+1: É possível adicionar informações estruturadas adicionais ao log, mas é necessário limpar manualmente após o descarregamento da mesma e o arquivo de format

## Serilog

- :heavy_check_mark: 95 targets suportados, entre os principais: Console, Arquivo, RabbitMQ, Elasticsearch, AWS Cloud Watch, Azure ([Fonte](https://github.com/serilog/serilog/wiki/Provided-Sinks))
- :heavy_check_mark: Ótimos recursos para geração de logs estruturados para envio a ferramentas de análise ([Structured Data](https://github.com/serilog/serilog/wiki/Structured-Data))
- :-1: Para logar automaticamente o nome do método e classes é necessário alternativa manuais em cada escrita ([Exemplo](https://hovermind.com/serilog/class-name-method-name-and-line-number.html)) ou um configuração de LogManager por classe ([Exemplo](https://benfoster.io/blog/serilog-best-practices/))
- :heavy_check_mark: Fácil configuração