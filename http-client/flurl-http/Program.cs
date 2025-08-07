using System;
using flurl_http_playground;
using Flurl;
using Flurl.Http;
using static System.Console;
using Maestria.Extensions;
using Flurl.Http.Newtonsoft;
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
FlurlHttp.ConfigureClientForUrl(url).WithSettings(settings =>
    {
        settings.JsonSerializer = new NewtonsoftJsonSerializer(Converter.Settings);
    })
    .BeforeCall(call => WriteLine($"My custom before call event intercept => {call.Request.Url}"))
    .OnError(call => WriteLine($"My custom error event intercept => {call.Exception}"));

var cepResponse = await "https://viacep.com.br/ws/01001000/json/".GetJsonAsync<ViaCepResponse>();
WriteLine($"ViaCep: {cepResponse.Cep} - {cepResponse.Logradouro}, {cepResponse.Bairro}, {cepResponse.Localidade}/{cepResponse.Uf}");
WriteLine();

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


record ViaCepResponse(
    string Cep,
    string Logradouro,
    string Complemento,
    string Bairro,
    string Localidade,
    string Uf,
    string Ibge,
    string Gia,
    string Ddd,
    string Siafi);
