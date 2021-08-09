| Feature                                               | NLog                  | Microsoft         | Log4Net               |
|-------------------------------------------------------|-----------------------|-------------------|-----------------------|
| Arquivo com nome diário                               | :heavy_check_mark:    | :-1:              | :heavy_check_mark:    |
| Exclusão de arquivos antigos                          | :heavy_check_mark:    | :-1:              | :heavy_check_mark:    |
| Obter nome do método que chamou o log automaticamente | :heavy_check_mark:    | :x:               | :heavy_check_mark:    |
| Proxy para Microsoft Logging                          | :heavy_check_mark:    | :ok_hand:         |                       |
| Facilidade de instância Log Manager                   | :heavy_check_mark:    | :x:               | :-1                   |
| Documentação                                          | :heavy_check_mark:    | :-1:              | :-1                   |
| Flush assincrono                                      | :heavy_check_mark:    | heavy_check_mark: | :x:                   |

## NLog :heart:

- Personalização do layout
- 93 targets suportados para log, entre os principais: Console, Arquivo, RabbitMQ, Redis, Elasticsearch, AWS Cloud Watch, Azure.
- Suporte para logar automaticamente o nome do método que invocou a geração de log através do render [`${callsite}`](https://github.com/NLog/NLog/wiki/Callsite-Layout-Renderer)
- Suporte a layout com condicionais através do render [`${when}`](https://github.com/NLog/NLog/wiki/When-Layout-Renderer)
- Suporte ao pattern de Correlation Id através do render [`${activityid}`](https://github.com/NLog/NLog/wiki/Trace-Activity-Id-Layout-Renderer). Útil principalmente em aplicações web.
  - ```Trace.CorrelationManager.ActivityId = Guid.NewGuid();```
- Layout diferente por target
- Ferramenta de visualização de logs [NlogViewer](https://github.com/dojo90/NLogViewer)
- Suporte a inclusão de propriedades estruturadas adicionais
- [Lista completa de configurações](https://nlog-project.org/config/?tab=layout-renderers)
 

## Microsoft Logging

- :x: Para obter o nome do método que chamou o log é necessário iniciar um escopo manualmente ```using (_logger.BeginScope(nameof(NomeMethodo)))```
- :x: Para armazenar em arquivo texto é necessário componentes de terceiros
- :x: Dificuldade de acesso ao Log Manager
  - Pela convensão é recomendado informar a própria classe na declaração da variável de log para o trace capturar o nome da mesma ```private readonly ILogger<Teste> _logger```. 
  - Geralmente instância por injeção de dependência do construtor
  - Influencia no design das classes dependentes por ser necessário adicionar o argumento no construtor também
 - :x: Não há suporte a inclusão de propriedades estruturadas adicionais

## Log4Net

- :-1: Documentação fraca
- :-1: Não possui log no level Trace por padrão, sendo necessário adicionar Helpers para para simular operação
- :+1: É possível adicionar informações estruturadas adicionais ao log, mas é necessário limpar manualmente após o descarregamento da mesma e o arquivo de format
