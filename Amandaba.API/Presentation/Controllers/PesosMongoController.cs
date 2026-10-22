using Amandaba.API.Application.Interfaces;
using Amandaba.API.Infrastructure.Mongo;
using Amandaba.Application.Interfaces;
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
    private readonly IPesoUseCase _pesos;
    private readonly IPesoMongoRepository _mongo;
    public PesosMongoController(IPesoUseCase pesos, IPesoMongoRepository mongo)
    { _pesos = pesos; _mongo = mongo; }

    // Exporta um JSON real do Oracle. Pode ser usado como dataset da disciplina de Banco.
    [HttpGet("exportar-json")]
    public IActionResult Exportar(decimal petId)
    {
        var pesos = _pesos.ObterPorPet(petId);
        return Ok(new { petId, historico = pesos });
    }

    // Sincronizacao manual para manter o Oracle como fonte oficial.
    // Apenas o tutor proprietario pode substituir o documento no Mongo.
    [HttpPost("sincronizar")]
    public IActionResult Sincronizar(decimal petId)
    {
        var lista = _pesos.ObterPorPet(petId).ToList();
        var doc = new PesoHistoricoDocumento
        {
            Id = $"pet-{petId}", PetId = petId, AtualizadoEm = DateTime.UtcNow,
            Historico = lista.Select(p => new MedicaoPesoDocumento
            { PesoId = p.IdHistoricoPeso, Peso = p.Peso, DataMedicao = p.DataMedicao }).ToList()
        };
        _mongo.Salvar(doc);
        return Ok(new { mensagem = "Historico copiado do Oracle para MongoDB.", petId, medicoes = doc.Historico.Count });
    }

    [HttpGet("mongo")]
    public IActionResult ConsultarMongo(decimal petId)
    {
        var doc = _mongo.ObterPorPet(petId);
        return doc is null ? NotFound(new { mensagem = "Nao foi sincronizado no MongoDB." }) : Ok(doc);
    }
}
