using Amandaba.API.Application.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Amandaba.API.Presentation.Controllers;

[ApiController]
[Authorize]
[ProducesResponseType(StatusCodes.Status401Unauthorized)]
[ProducesResponseType(StatusCodes.Status403Forbidden)]
[ProducesResponseType(StatusCodes.Status500InternalServerError)]
[Route("api/pets/{petId}/pesos")]
public class PesosMongoController : ControllerBase
{
    private readonly IPesoHistoricoUseCase _historico;
    public PesosMongoController(IPesoHistoricoUseCase historico)
    { _historico = historico; }

    [HttpGet("exportar-json")]
    public IActionResult Exportar(decimal petId) =>
        Ok(new { petId, historico = _historico.Exportar(petId) });

    [HttpPost("sincronizar")]
    public IActionResult Sincronizar(decimal petId)
    {
        var quantidade = _historico.Sincronizar(petId);
        return Ok(new { mensagem = "Historico copiado do Oracle para MongoDB.", petId, medicoes = quantidade });
    }

    [HttpGet("mongo")]
    public IActionResult ConsultarMongo(decimal petId)
    {
        var documento = _historico.ConsultarMongo(petId);
        return documento is null
            ? NotFound(new { mensagem = "Nao foi sincronizado no MongoDB." })
            : Ok(documento);
    }
}
