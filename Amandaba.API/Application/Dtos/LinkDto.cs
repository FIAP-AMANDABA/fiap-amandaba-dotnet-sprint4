namespace Amandaba.API.Application.Dtos;

// Aula 08: cada link HATEOAS informa o recurso, sua relacao e o verbo HTTP.
public class LinkDto
{
    public string Href { get; set; } = string.Empty;
    public string Rel { get; set; } = string.Empty;
    public string Method { get; set; } = "GET";

    public LinkDto(string href, string rel, string method = "GET")
    { Href = href; Rel = rel; Method = method; }
}
