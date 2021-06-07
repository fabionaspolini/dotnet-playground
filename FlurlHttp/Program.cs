using System;
using Flurl;
using Flurl.Http;
using static System.Console;
using Maestria.Extensions;
using Flurl.Http.Configuration;
using QuickType;
using Microsoft.Extensions.Configuration;

// 1 - Exemplo utilizando a API pública da Marvel https://developer.marvel.com/docs
// 2 - Cadastre-se gratuitamente e obtenha a chave publica e privada para executar este demo https://developer.marvel.com/account
// 3 - Configure os secrets com as instruções no README.md
// 4 - A classe modelo de resposta foi gerada automaticamente com o plugin do VS Code https://marketplace.visualstudio.com/items?itemName=quicktype.quicktype

WriteLine(".:: Flurl.Http Samples ::.");

var config = new ConfigurationBuilder()
    .SetBasePath(AppDomain.CurrentDomain.BaseDirectory)
    .AddUserSecrets("1a38f28a-eaf9-461f-a976-c1050e70fcd0")
    .Build();

const string url = "http://gateway.marvel.com";
var privateKey = config.GetValue<string>("privateKey");
var publicKey = config.GetValue<string>("publicKey");

// Realizando customizações para deserealização de acordo com o código gerado automaticamente e definindo eventos globais para tratamento de erro e log de requisições
FlurlHttp.Configure(settings =>
{
    var jsonSettings = QuickType.Converter.Settings;
    settings.JsonSerializer = new NewtonsoftJsonSerializer(jsonSettings);
    settings.BeforeCall += (FlurlCall call) => WriteLine($"My custom before call event intercept => {call.Request.Url}");
    settings.OnError += (FlurlCall call) => WriteLine($"My custom error event intercept => {call.Exception.GetAllMessages()}");
});

var ts = Guid.NewGuid();
WriteLine("Consultando personagens...");
var personagens = await url
    .AppendPathSegment("v1/public/characters")
    // Parâmetros query string e headers podem ser atribudos um por vez ou vários através de objetos anônimos
    .WithHeader("x-my-custom-header", 123)
    .SetQueryParams(new
    {
        apikey = publicKey,
        ts = ts,
        hash = (ts + privateKey + publicKey).GetHashMd5(),
        limit = 100
    })
    .GetJsonAsync<CharactersResponse>();

WriteLine($"{personagens.Data.Count} obtidos de {personagens.Data.Total}");
WriteLine();
WriteLine($"|{"Nome",-40}|{"Atualizado",-27}|{"Comics",10}|{"Series",10}|{"Stories",10}|{"Events",10}|");
WriteLine(new string('-', 114));
foreach (var item in personagens.Data.Results)
    WriteLine($"|{item.Name.LimitLen(40),-40}|{item.Modified,-27}|{item.Comics.Available,10}|{item.Series.Available,10}|{item.Stories.Available,10}|{item.Events.Available,10}|");
