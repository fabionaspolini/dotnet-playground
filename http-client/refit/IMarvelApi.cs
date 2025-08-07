using System.Threading.Tasks;
using Refit;

namespace refit_playground;

[Headers("User-Agent: My-dotnet-Client")]
public interface IMarvelApi
{
    [Get("/v1/public/characters")]
    Task<CharactersResponse> GetCharactersAsync([Query] CharactersRequest request);
}