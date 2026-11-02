using Amandaba.API.Application.Dtos;

namespace Amandaba.API.Presentation;

// Centraliza a criacao dos links; os controllers apenas escolhem os recursos.
// Aula 08: LinkDto com Href, Rel e Method.
public static class HateoasLinks
{
    public static List<LinkDto> ParaPet(decimal petId, bool somenteLeitura = false)
    {
        var caminho = $"/api/pets/{petId}";
        var links = new List<LinkDto>
        {
            new(caminho, "self"),
            new($"{caminho}/pesos", "pesos"),
            new($"{caminho}/vacinas", "vacinas"),
            new($"{caminho}/consultas", "consultas"),
            new($"{caminho}/exames", "exames"),
            new($"{caminho}/doencas", "doencas"),
            new($"{caminho}/alergias", "alergias"),
            new($"{caminho}/medicamentos", "medicamentos"),
            new($"{caminho}/plano-cuidados", "plano-cuidados")
        };
        if (!somenteLeitura)
        {
            links.Add(new LinkDto(caminho, "update", "PUT"));
            links.Add(new LinkDto($"{caminho}/status", "alterar-status", "PATCH"));
            links.Add(new LinkDto($"{caminho}/veterinarios", "compartilhar", "POST"));
        }
        return links;
    }

    public static List<LinkDto> ParaRegistro(string caminho, bool permitirEdicao = true)
    {
        var links = new List<LinkDto> { new(caminho, "self") };
        if (permitirEdicao)
        {
            links.Add(new LinkDto(caminho, "update", "PUT"));
            links.Add(new LinkDto(caminho, "delete", "DELETE"));
        }
        return links;
    }

    public static List<LinkDto> ParaPagina(string caminho, int pagina, int tamanho, int total)
    {
        var links = new List<LinkDto> { new($"{caminho}?pagina={pagina}&tamanho={tamanho}", "self") };
        if (pagina > 1)
            links.Add(new LinkDto($"{caminho}?pagina={pagina - 1}&tamanho={tamanho}", "previous"));
        if ((long)pagina * tamanho < total)
            links.Add(new LinkDto($"{caminho}?pagina={pagina + 1}&tamanho={tamanho}", "next"));
        return links;
    }
}
