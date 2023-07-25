using NSwagStudioPlayground;
using System.Net.Http;
using static System.Console;

WriteLine(".:: Swagger Samples ::.");

var httpClient = new HttpClient();
var client = new GhibliApiClient(httpClient);
client.BaseUrl = "https://ghibliapi.herokuapp.com";

var response = await client.FilmsGetAsync(fields: "title,release_date", limit: 10);
WriteLine($"|{"Title",-40}|{"Date",-5}|");
WriteLine(new string('-', 48));
foreach (var item in response)
    WriteLine($"|{item.Title,-40}|{item.Release_date,5}|");
