using Amandaba.API.Application.Dtos.Compartilhamentos;
using Amandaba.API.Application.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Amandaba.API.Presentation.Controllers;
[ApiController]
[Authorize]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
public class CompartilhamentosController : ControllerBase
{
    private readonly ICompartilhamentoUseCase _servico;
    private readonly IPetAccessService _acesso;
    public CompartilhamentosController(ICompartilhamentoUseCase servico, IPetAccessService acesso)
    { _servico = servico; _acesso = acesso; }

    [HttpPost("api/pets/{petId}/veterinarios")]
    public IActionResult Compartilhar(decimal petId, [FromBody] CompartilhamentoRequestDto dto)
    {
        try
        {
            var item = _servico.Compartilhar(petId, dto.VeterinarioId);
            return Ok(new { item.IdPet, item.IdVeterinario, item.Status, item.DataCompartilhamento });
        }
        catch (ArgumentException ex) { return BadRequest(new { mensagem = ex.Message }); }
    }

    [HttpGet("api/pets/{petId}/veterinarios")]
    public IActionResult Listar(decimal petId) => Ok(_servico.Listar(petId).Select(item => new
    { item.IdPet, item.IdVeterinario, item.Status, item.DataCompartilhamento }));

    [HttpPatch("api/pets/{petId}/veterinarios/{veterinarioId}/revogar")]
    public IActionResult Revogar(decimal petId, decimal veterinarioId) => _servico.Revogar(petId, veterinarioId)
        ? NoContent() : NotFound(new { mensagem = "Compartilhamento nao encontrado." });

    [HttpGet("api/veterinarios/me/pets")]
    public IActionResult MeusPets([FromQuery] int pagina = 1, [FromQuery] int tamanho = 10)
    {
        var id = _acesso.ObterIdUsuario(User);
        if (!id.HasValue || User.FindFirst("perfil")?.Value != "VETERINARIO") return Forbid();
        var vetId = _acesso.ObterIdVeterinario(id.Value);
        if (!vetId.HasValue) return Forbid();
        var pets = _servico.ListarPetsVeterinario(vetId.Value)
            .Select(p => new { p.IdPet, p.Nome, Especie = p.Especie.Nome, p.Status }).ToList();
        return Ok(Paginacao(pets, pagina, tamanho, "/api/veterinarios/me/pets"));
    }

    private object Paginacao<T>(IEnumerable<T> lista, int pagina, int tamanho, string caminho)
    {
        pagina = Math.Max(1, pagina); tamanho = Math.Clamp(tamanho, 1, 100);
        var itens = lista.ToList();
        return new { pagina, tamanho, total = itens.Count,
            dados = itens.Skip((pagina - 1) * tamanho).Take(tamanho),
            links = new[] {
                new { rel = "self", href = $"{caminho}?pagina={pagina}&tamanho={tamanho}" },
                new { rel = "next", href = $"{caminho}?pagina={pagina+1}&tamanho={tamanho}" }
            }.Where(x => x.rel != "next" || pagina * tamanho < itens.Count) };
    }
}
