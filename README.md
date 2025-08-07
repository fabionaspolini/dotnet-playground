# .NET Samples

- [Visão geral](#visão-geral)
- [Biliotecas](#biliotecas)
  - [background-jobs](#background-jobs)
  - [data](#data)
  - [data-mapper](#data-mapper)
  - [http-client](#http-client)
  - [logs](#logs)
  - [miscellaneous](#miscellaneous)
  - [syntax-sugar](#syntax-sugar)
  - [source-generator](#source-generator)
  - [template-engine](#template-engine)
- [Avaliações](#avaliações)
- [Contribuindo](#contribuindo)

## Visão geral

Repositório com pequenos exemplos de configurações e uso de bibliotecas úteis para C# / .NET.

Maiores detalhes descritos no meu [post](https://fabionaspolini.medium.com/bibliotecas-para-incrementar-a-produtividade-em-c-net-52749e9329d3).

## Biliotecas

### background-jobs

- HangFire: Agendamento de tarefas em backgroud
- quartz.NET: Agendamento de tarefas em backgroud
- Worker Service: Execução de tarefas em background sem suporte a agendamento de horários

### data

- [cache-manager](data/cache-manager): Pipeline para gerenciamento de cache em memória e Redis
- Dapper: Simples ORM pequeno para leitura de dados
- DapperContrib: Adicionar suporte a escrita no Dapper
- [fluent-validation](data/atadataaafluent-validation): Validação de dados
- ClosedXML: Leitura e escrita de excel independente de softwares instalados na estação de trabalho.

### data-mapper

- [auto-mapper](data-mapper/auto-mapper)
- [Mapster](data-mapper/Mapster)

### http-client

- [flurl-http](Http-Client/flurl-http): Acesso a HTTP por sintax fluente
- [nswag-studio](Http-Client/nswag-studio): Geração de código a partir de especificação swagger
- [refit](http-client/refit): Acesso a HTTP por mapeamento de objetos

### [logs](logs)
- [NLog](logs/NLog)
- [NLogAsMicrosoftProxy](logs/NLogAsMicrosoftProxy)
- [log4net](logs/log4net)
- [microsoft-logging](logs/microsoft-logging)
- [Serilog](logs/Serilog)

### miscellaneous
- [LanguageFeatures](miscellaneous/LanguageFeatures): Recursos da linguagem C# / .NET
- [polly](miscellaneous/polly): Resiliência de operações

### syntax-sugar

- [fluent-date-time](Syntax-Sugar/fluent-date-time): Melhorar a experiência de trabalho com datas
- [humanizer](Syntax-Sugar/humanizer): Humanizar leitura de unidades de medias
- [Maestria.Extensions](Syntax-Sugar/maestria-extensions): Métodos de extensões úteis
- [Maestria.FluentCast](Syntax-Sugar/maestria-fluent-cast): Converter valores de tipos primitidos de forma fluente

### source-generator

- [Maestria.TypeProviders](Source-Generator/maestria-type-providers): Gerador de código a partir de template de arquivo (Excel)
- [primary-constructor](Source-Generator/primary-constructor): Adicionar construtor padrão na classe para variáveis e propriedades read only
- [data-builder-generator](Source-Generator/data-builder-generator): Adicionar métodos para contrução de classes de dados fluente

### template-engine

- [Liquid](Template-Engine/Liquid): Template com suporte a execução de expressões lógicas
- [Mustache](Template-Engine/Mustache): Template sem suporte a execuções de expressões lógicas


## Avaliações

Nas avaliações de frameworks concorrentes, estará indicado com :heart: a preferência do autor :smile:

**Legenda marcadores:**

- :heavy_check_mark: Requisito atendido com excelência
- :+1: Requisito atendido oficialmente, mas com deficiências
- :-1: Requisito não atendido oficialmente, mas com alternativas manuais
- :x: Requisito não atendido

## Contribuindo

1. Criar projeto dentro da pasta adequada ao assunto no padrão kebab-case.

```bash
dotnet new console -n <nome-exemplo>-playground -o <nome-exemplo>
```

Argumentos:

```txt
-n: Nome do arquivo csproj. O name space será o mesmo nome no padrão "snake_case", matenha-o dessa forma.
-o: Nome da pasta onde o projeto será criado.
```