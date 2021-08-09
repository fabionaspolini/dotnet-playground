# .NET Samples

Repositório com pequenos exemplos de configurações e uso de bibliotecas úteis para C# / .NET.

Maiores detalhes descritos no meu [post](https://fabionaspolini.medium.com/bibliotecas-para-incrementar-a-produtividade-em-c-net-52749e9329d3).

## Biliotecas

- Syntax sugar
  - [FluentDateTime](FluentDateTime): Melhorar a experiência de trabalho com datas
  - [Humanizer](Humanizer): Humanizar leitura de unidades de medias
  - [Maestria.Extensions](MaestriaExtensions): Métodos de extensões úteis
  - [Maestria.FluentCast](MaestriaFluentCast): Converter valores de tipos primitidos de forma fluente
- Data
  - [CacheManager](CacheManager): Pipeline para gerenciamento de cache em memória e Redis
  - Dapper: Simples ORM pequeno para leitura de dados
  - DapperContrib: Adicionar suporte a escrita no Dapper
  - [FluentValidation](FluentValidation): Validação de dados
  - ClosedXML: Leitura e escrita de excel independente de softwares instalados na estação de trabalho.
- HTTP Client
  - [FlurlHttp](FlurlHttp): Acesso a HTTP por sintax fluente
  - [NSwagStudio](NSwagStudio): Geração de código a partir de especificação swagger
  - [Refit](Refit): Acesso a HTTP por mapeamento de objetos
- Template Engine
  - [Liquid](Liquid): Template com suporte a execução de expressões lógicas
  - [Mustache](Mustache): Template sem suporte a execuções de expressões lógicas
- Background tasks
  - HangFire: Agendamento de tarefas em backgroud
  - Quartz.NET: Agendamento de tarefas em backgroud
  - Worker Service: Execução de tarefas em background sem suporte a agendamento de horários
- Source Generators
  - [Maestria.TypeProviders](MaestriaTypeProviders): Gerador de código a partir de template de arquivo (Excel)
  - [PrimaryConstructor](PrimaryConstructor): Adicionar construtor padrão na classe para variáveis e propriedades read only
  - [DataBuilderGenerator](DataBuilderGenerator): Adicionar métodos para contrução de classes de dados fluente
- Logs
  - [NLog](NLog): Gerenciador de logs
- Others
  - [LangFeatures](LangFeatures): Recursos da linguagem C# / .NET
  - [Polly](Polly): Resiliência de operações


## Avaliações

Nas avaliações de frameworks concorrentes, estará indicado com :heart: a preferência do autor :smile:

- [Logs](Docs/Logs.md)