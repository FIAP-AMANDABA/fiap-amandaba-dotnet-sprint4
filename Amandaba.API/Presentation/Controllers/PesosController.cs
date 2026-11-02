using Amandaba.API.Presentation;
using Microsoft.AspNetCore.Authorization;
using Amandaba.Application.Dtos.Pesos;
using Amandaba.Application.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace Amandaba.Presentation.Controllers
{
    [ApiController]
    [Authorize]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    [Route("api/pets/{petId}/pesos")]
    public class PesosController : ControllerBase
    {
        private readonly IPesoUseCase _pesoUseCase;

        public PesosController(IPesoUseCase pesoUseCase)
        {
            _pesoUseCase = pesoUseCase;
        }

        [HttpGet]
        [SwaggerOperation(
            Summary = "Listar histórico de peso",
            Description = "Retorna todas as medições de peso do pet."
        )]
        [SwaggerResponse(StatusCodes.Status200OK, "Pesos retornados com sucesso.")]
        [SwaggerResponse(StatusCodes.Status404NotFound, "Pet não encontrado.")]
        public IActionResult ObterPorPet(decimal petId)
        {
            try
            {
                var registros = _pesoUseCase.ObterPorPet(petId).ToList();
                foreach (var registro in registros)
                    registro.Links = new() { new($"/api/pets/{petId}/pesos", "historico"),
                        new($"/api/pets/{petId}/pesos/atual", "atual") };
                return Ok(registros);
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(new { mensagem = ex.Message });
            }
            
        }

        [HttpGet("atual")]
        [SwaggerOperation(
            Summary = "Consultar peso atual",
            Description = "Retorna a medição mais recente do pet."
        )]
        [SwaggerResponse(StatusCodes.Status200OK, "Peso atual retornado com sucesso.")]
        [SwaggerResponse(StatusCodes.Status404NotFound, "Pet ou medição não encontrada.")]
        public IActionResult ObterAtual(decimal petId)
        {
            try
            {
                var peso = _pesoUseCase.ObterAtual(petId);

                if (peso is null)
                    return NotFound(new { mensagem = "Nenhuma medição encontrada para o pet." });

                peso.Links = new()
                {
                    new($"/api/pets/{petId}/pesos/atual", "self"),
                    new($"/api/pets/{petId}/pesos", "historico")
                };
                return Ok(peso);
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(new { mensagem = ex.Message });
            }
            
        }

        [HttpPost]
        [SwaggerOperation(
            Summary = "Registrar peso",
            Description = "Registra uma nova medição de peso para o pet."
        )]
        [SwaggerResponse(StatusCodes.Status201Created, "Peso registrado com sucesso.")]
        [SwaggerResponse(StatusCodes.Status400BadRequest, "Dados inválidos.")]
        [SwaggerResponse(StatusCodes.Status404NotFound, "Pet não encontrado.")]
        public IActionResult Cadastrar(
            decimal petId,
            [FromBody] PesoRequestDto dto)
        {
            try
            {
                var peso = _pesoUseCase.Cadastrar(petId, dto);

                return CreatedAtAction(
                    nameof(ObterPorPet),
                    new { petId },
                    peso
                );
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(new { mensagem = ex.Message });
            }
            catch (ArgumentException ex)
            {
                return BadRequest(new { mensagem = ex.Message });
            }
            
        }

        [HttpPut("{pesoId}")]
        [SwaggerOperation(
            Summary = "Editar medição de peso",
            Description = "Atualiza uma medição existente do pet."
        )]
        [SwaggerResponse(StatusCodes.Status204NoContent, "Peso atualizado com sucesso.")]
        [SwaggerResponse(StatusCodes.Status400BadRequest, "Dados inválidos.")]
        [SwaggerResponse(StatusCodes.Status404NotFound, "Pet ou medição não encontrada.")]
        public IActionResult Atualizar(
            decimal petId,
            decimal pesoId,
            [FromBody] PesoRequestDto dto)
        {
            try
            {
                var atualizado = _pesoUseCase.Atualizar(
                    petId,
                    pesoId,
                    dto
                );

                if (!atualizado)
                    return NotFound(new { mensagem = "Medição de peso não encontrada." });

                return NoContent();
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(new { mensagem = ex.Message });
            }
            catch (ArgumentException ex)
            {
                return BadRequest(new { mensagem = ex.Message });
            }
            
        }

        [HttpDelete("{pesoId}")]
        [SwaggerOperation(
            Summary = "Excluir medição de peso",
            Description = "Remove uma medição do histórico de peso do pet."
        )]
        [SwaggerResponse(StatusCodes.Status204NoContent, "Peso excluído com sucesso.")]
        [SwaggerResponse(StatusCodes.Status404NotFound, "Pet ou medição não encontrada.")]
        public IActionResult Excluir(decimal petId, decimal pesoId)
        {
            try
            {
                var excluido = _pesoUseCase.Excluir(petId, pesoId);

                if (!excluido)
                    return NotFound(new { mensagem = "Medição de peso não encontrada." });

                return NoContent();
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(new { mensagem = ex.Message });
            }
            
        }
    }
}