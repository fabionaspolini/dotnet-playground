# .NET Samples

Repositório com pequenos exemplos de configurações e uso de bibliotecas úteis para C# / .NET.

Maiores detalhes descritos no meu [post](https://fabionaspolini.medium.com/bibliotecas-para-incrementar-a-produtividade-em-c-net-52749e9329d3).

## Biliotecas

- Syntax Sugar
  - [FluentDateTime](Syntax-Sugar/FluentDateTime): Melhorar a experiência de trabalho com datas
  - [Humanizer](Syntax-Sugar/Humanizer): Humanizar leitura de unidades de medias
  - [Maestria.Extensions](Syntax-Sugar/MaestriaExtensions): Métodos de extensões úteis
  - [Maestria.FluentCast](Syntax-Sugar/MaestriaFluentCast): Converter valores de tipos primitidos de forma fluente
- Data
  - [CacheManager](Data/CacheManager): Pipeline para gerenciamento de cache em memória e Redis
  - Dapper: Simples ORM pequeno para leitura de dados
  - DapperContrib: Adicionar suporte a escrita no Dapper
  - [FluentValidation](Data/FluentValidation): Validação de dados
  - ClosedXML: Leitura e escrita de excel independente de softwares instalados na estação de trabalho.
- Http Client
  - [FlurlHttp](Http-Client/FlurlHttp): Acesso a HTTP por sintax fluente
  - [NSwagStudio](Http-Client/NSwagStudio): Geração de código a partir de especificação swagger
  - [Refit](Http Client/Refit): Acesso a HTTP por mapeamento de objetos
- Template Engine
  - [Liquid](Template-Engine/Liquid): Template com suporte a execução de expressões lógicas
  - [Mustache](Template-Engine/Mustache): Template sem suporte a execuções de expressões lógicas
- Source Generator
  - [Maestria.TypeProviders](Source-Generator/MaestriaTypeProviders): Gerador de código a partir de template de arquivo (Excel)
  - [PrimaryConstructor](Source-Generator/PrimaryConstructor): Adicionar construtor padrão na classe para variáveis e propriedades read only
  - [DataBuilderGenerator](Source-Generator/DataBuilderGenerator): Adicionar métodos para contrução de classes de dados fluente
- Background tasks
  - HangFire: Agendamento de tarefas em backgroud
  - Quartz.NET: Agendamento de tarefas em backgroud
  - Worker Service: Execução de tarefas em background sem suporte a agendamento de horários
- [Logs](Logs/README.md)
  - [NLog](Logs/NLog)
  - [NLogAsMicrosoftProxy](Logs/NLogAsMicrosoftProxy)
  - [Log4Net](Logs/Log4Net)
  - [MicrosoftLogging](Logs/MicrosoftLogging)
  - [Serilog](Logs/Serilog)
- Others
  - [LangFeatures](Others/LangFeatures): Recursos da linguagem C# / .NET
  - [Polly](Others/Polly): Resiliência de operações


## Avaliações

Nas avaliações de frameworks concorrentes, estará indicado com :heart: a preferência do autor :smile:

**Legenda marcadores:**

- :heavy_check_mark: Requisito atendido com excelência
- :+1: Requisito atendido oficialmente, mas com deficiências
- :-1: Requisito não atendido oficialmente, mas com alternativas manuais
- :x: Requisito não atendido
