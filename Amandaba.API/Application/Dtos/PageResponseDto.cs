using System.Text.Json.Serialization;

namespace Amandaba.API.Application.Dtos;

// Mesma estrategia da aula 07 (offset, limit, total), mantendo o JSON
// pagina/tamanho/total/dados ja utilizado pelo aplicativo e pelo Swagger.
public class PageResponseDto<T>
{
    [JsonIgnore]
    public int Deslocamento { get; set; }
    [JsonIgnore]
    public int RegistroRetornado { get; set; }
    [JsonIgnore]
    public int TotalRegistros { get; set; }

    [JsonPropertyName("pagina")]
    public int Pagina => RegistroRetornado > 0 ? (Deslocamento / RegistroRetornado) + 1 : 1;
    [JsonPropertyName("tamanho")]
    public int Tamanho => RegistroRetornado;
    [JsonPropertyName("total")]
    public int Total => TotalRegistros;
    [JsonPropertyName("dados")]
    public IEnumerable<T> Items { get; set; } = new List<T>();
    [JsonPropertyName("links")]
    public List<LinkDto> Links { get; set; } = new();
}
