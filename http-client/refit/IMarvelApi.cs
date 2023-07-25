using System.Threading.Tasks;
using QuickType;
using Refit;

[Headers("User-Agent: My-dotnet-Client")]
public interface IMarvelApi
{
    [Get("/v1/public/characters")]
    Task<CharactersResponse> GetCharactersAsync([Query] CharactersRequest request);
}