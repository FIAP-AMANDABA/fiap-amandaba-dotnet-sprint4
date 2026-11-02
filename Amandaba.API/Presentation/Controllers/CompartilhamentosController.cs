using Amandaba.API.Application.Dtos.Compartilhamentos;
using Amandaba.API.Application.Dtos;
using Amandaba.API.Presentation;
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
    {
        item.IdPet, item.IdVeterinario, item.Status, item.DataCompartilhamento,
        links = new List<LinkDto>
        {
            new($"/api/pets/{petId}/veterinarios", "self"),
            new($"/api/pets/{petId}/veterinarios/{item.IdVeterinario}/revogar", "revogar", "PATCH")
        }
    }));

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
        var resultado = _servico.ListarPetsVeterinarioPaginado(vetId.Value, pagina, tamanho);
        var dados = resultado.Items.Select(p => new
        {
            p.IdPet, p.Nome, Especie = p.Especie.Nome, p.Status,
            links = HateoasLinks.ParaPet(p.IdPet, somenteLeitura: true)
        }).ToList();
        var resposta = new PageResponseDto<object>
        {
            Deslocamento = resultado.Deslocamento,
            RegistroRetornado = resultado.RegistroRetornado,
            TotalRegistros = resultado.TotalRegistros,
            Items = dados.Cast<object>().ToList(),
            Links = HateoasLinks.ParaPagina("/api/veterinarios/me/pets", resultado.Pagina,
                resultado.Tamanho, resultado.Total)
        };
        return Ok(resposta);
    }
}
