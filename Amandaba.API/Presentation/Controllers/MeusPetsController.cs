using Amandaba.API.Application.Interfaces;
using Amandaba.Application.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Amandaba.API.Presentation.Controllers;
[ApiController]
[Authorize]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
[Route("api/pets/me")]
public class MeusPetsController : ControllerBase
{
    private readonly IPetAccessService _acesso;
    private readonly IPetUseCase _pets;
    public MeusPetsController(IPetAccessService acesso, IPetUseCase pets)
    { _acesso = acesso; _pets = pets; }

    [HttpGet]
    public IActionResult Listar([FromQuery] int pagina = 1, [FromQuery] int tamanho = 10,
        [FromQuery] string? nome = null, [FromQuery] string? ordem = "nome")
    {
        var id = _acesso.ObterIdUsuario(User);
        if (!id.HasValue || User.FindFirst("perfil")?.Value != "TUTOR") return Forbid();
        var tutor = _acesso.ObterIdTutor(id.Value);
        if (!tutor.HasValue) return Forbid();
        pagina = Math.Max(1, pagina); tamanho = Math.Clamp(tamanho, 1, 100);
        var dados = _pets.ObterPorTutor(tutor.Value);
        if (!string.IsNullOrWhiteSpace(nome)) dados = dados.Where(p => p.Nome.Contains(nome, StringComparison.OrdinalIgnoreCase));
        dados = ordem?.ToLowerInvariant() == "nome_desc"
            ? dados.OrderByDescending(p => p.Nome) : dados.OrderBy(p => p.Nome);
        var lista = dados.ToList();
        string href(int n) => $"/api/pets/me?pagina={n}&tamanho={tamanho}&nome={Uri.EscapeDataString(nome ?? "")}&ordem={ordem}";
        var links = new List<object> { new { rel = "self", href = href(pagina) } };
        if (pagina > 1) links.Add(new { rel = "previous", href = href(pagina-1) });
        if (pagina * tamanho < lista.Count) links.Add(new { rel = "next", href = href(pagina+1) });
        return Ok(new { pagina, tamanho, total = lista.Count,
            dados = lista.Skip((pagina-1)*tamanho).Take(tamanho).Select(p => new {
                pet = p, links = new[] { new { rel = "self", href = $"/api/pets/{p.IdPet}" },
                                             new { rel = "pesos", href = $"/api/pets/{p.IdPet}/pesos" } }
            }), links });
    }
}
