using Amandaba.API.Application.Dtos;
using Amandaba.API.Application.Dtos.Pets;
using Amandaba.API.Application.Interfaces;
using Amandaba.API.Presentation;
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

        var resultado = _pets.ObterPorTutorPaginado(tutor.Value, pagina, tamanho, nome, ordem);
        var caminho = "/api/pets/me";
        var query = $"&nome={Uri.EscapeDataString(nome ?? "")}&ordem={Uri.EscapeDataString(ordem ?? "nome")}";
        var itens = resultado.Items.Select(p => new PetItemDto
        {
            Pet = p,
            Links = HateoasLinks.ParaPet(p.IdPet)
        }).ToList();
        resultado.Links = HateoasLinks.ParaPagina(caminho, resultado.Pagina, resultado.Tamanho, resultado.Total)
            .Select(link => new LinkDto(link.Href + query, link.Rel, link.Method)).ToList();
        return Ok(new PageResponseDto<PetItemDto>
        {
            Deslocamento = resultado.Deslocamento,
            RegistroRetornado = resultado.RegistroRetornado,
            TotalRegistros = resultado.TotalRegistros,
            Items = itens,
            Links = resultado.Links
        });
    }
}
