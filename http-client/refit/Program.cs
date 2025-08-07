using System;
using static System.Console;
using Maestria.Extensions;
using Microsoft.Extensions.Configuration;
using Refit;
using System.Net.Http;
using System.Threading.Tasks;
using refit_playground;

// 1 - Exemplo utilizando a API pública da Marvel https://developer.marvel.com/docs
// 2 - Cadastre-se gratuitamente e obtenha a chave publica e privada para executar este demo https://developer.marvel.com/account
// 3 - Configure os secrets com as instruções no README.md
// 4 - A classe modelo de resposta foi gerada automaticamente com o plugin do VS Code https://marketplace.visualstudio.com/items?itemName=quicktype.quicktype

WriteLine(".:: Refit Samples ::.");

var config = new ConfigurationBuilder()
    .SetBasePath(AppDomain.CurrentDomain.BaseDirectory)
    .AddUserSecrets("1a38f28a-eaf9-461f-a976-c1050e70fcd0")
    .Build();

const string url = "http://gateway.marvel.com";
var privateKey = config.GetValue<string>("privateKey");
var publicKey = config.GetValue<string>("publicKey");

// Realizando customizações para deserealização de acordo com o código gerado automaticamente e definindo evento global para log de erro.
// Para log de requisições foi necessário criar a classe "HttpLoggingHandler" para escrever no Console através do HttpClient nativo.
var httpClient = new HttpClient(new HttpLoggingHandler()) { BaseAddress = new Uri(url) };
var settings = new RefitSettings
{
    ContentSerializer = new NewtonsoftJsonContentSerializer(Converter.Settings),
    ExceptionFactory = httpResponse =>
    {
        if (httpResponse.IsSuccessStatusCode)
            return Task.FromResult<Exception>(null);
        WriteLine("My custom error event intercept => " + httpResponse.ToString());
        return Task.FromResult<Exception>(new Exception(httpResponse.ToString()));
    }
};

var marvelApi = RestService.For<IMarvelApi>(httpClient, settings);
var personagens = await marvelApi.GetCharactersAsync(new CharactersRequest(privateKey, publicKey)
{
    Limit = 100
});

WriteLine($"{personagens.Data.Count} obtidos de {personagens.Data.Total}");
WriteLine();
WriteLine($"|{"Nome",-40}|{"Atualizado",-27}|{"Comics",10}|{"Series",10}|{"Stories",10}|{"Events",10}|");
WriteLine(new string('-', 114));
foreach (var item in personagens.Data.Results)
    WriteLine($"|{item.Name.LimitLen(40),-40}|{item.Modified,-27}|{item.Comics.Available,10}|{item.Series.Available,10}|{item.Stories.Available,10}|{item.Events.Available,10}|");
