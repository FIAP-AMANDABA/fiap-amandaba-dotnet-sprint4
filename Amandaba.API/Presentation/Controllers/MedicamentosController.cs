using Amandaba.API.Presentation;
using Microsoft.AspNetCore.Authorization;
using Amandaba.Application.Dtos.Medicamentos;
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
    [Route("api/pets/{petId}/medicamentos")]
    public class MedicamentosController : ControllerBase
    {
        private readonly IMedicamentoUseCase _medicamentoUseCase;

        public MedicamentosController(
            IMedicamentoUseCase medicamentoUseCase)
        {
            _medicamentoUseCase = medicamentoUseCase;
        }

        [HttpGet]
        [SwaggerOperation(
            Summary = "Listar medicamentos do pet",
            Description = "Retorna os medicamentos do pet, com filtro opcional por status."
        )]
        [SwaggerResponse(
            StatusCodes.Status200OK,
            "Medicamentos retornados com sucesso."
        )]
        [SwaggerResponse(
            StatusCodes.Status400BadRequest,
            "Status inválido."
        )]
        [SwaggerResponse(
            StatusCodes.Status404NotFound,
            "Pet não encontrado."
        )]
        public IActionResult ObterPorPet(
            decimal petId,
            [FromQuery] string? status)
        {
            try
            {
                var registros = _medicamentoUseCase.ObterPorPet(petId, status).ToList();
                foreach (var registro in registros)
                    registro.Links = HateoasLinks.ParaRegistro(
                        $"/api/pets/{petId}/medicamentos/{registro.IdRegistroMedicamento}");
                return Ok(registros);
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

        [HttpGet("{medicamentoId}")]
        [SwaggerOperation(
            Summary = "Consultar medicamento",
            Description = "Retorna um medicamento específico cadastrado para o pet."
        )]
        [SwaggerResponse(
            StatusCodes.Status200OK,
            "Medicamento retornado com sucesso."
        )]
        [SwaggerResponse(
            StatusCodes.Status404NotFound,
            "Pet ou medicamento não encontrado."
        )]
        public IActionResult ObterPorId(
            decimal petId,
            decimal medicamentoId)
        {
            try
            {
                var medicamento = _medicamentoUseCase.ObterPorId(
                    petId,
                    medicamentoId
                );

                if (medicamento is null)
                {
                    return NotFound(
                        new { mensagem = "Medicamento não encontrado." }
                    );
                }

                medicamento.Links = HateoasLinks.ParaRegistro($"/api/pets/{petId}/medicamentos/{medicamentoId}");
                return Ok(medicamento);
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(new { mensagem = ex.Message });
            }
            
        }

        [HttpPost]
        [SwaggerOperation(
            Summary = "Cadastrar medicamento",
            Description = "Cadastra um novo medicamento para o pet."
        )]
        [SwaggerResponse(
            StatusCodes.Status201Created,
            "Medicamento cadastrado com sucesso."
        )]
        [SwaggerResponse(
            StatusCodes.Status400BadRequest,
            "Dados inválidos."
        )]
        [SwaggerResponse(
            StatusCodes.Status404NotFound,
            "Pet não encontrado."
        )]
        public IActionResult Cadastrar(
            decimal petId,
            [FromBody] MedicamentoRequestDto dto)
        {
            try
            {
                var medicamento = _medicamentoUseCase.Cadastrar(
                    petId,
                    dto
                );

                return CreatedAtAction(
                    nameof(ObterPorId),
                    new
                    {
                        petId,
                        medicamentoId =
                            medicamento.IdRegistroMedicamento
                    },
                    medicamento
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

        [HttpPut("{medicamentoId}")]
        [SwaggerOperation(
            Summary = "Editar medicamento",
            Description = "Atualiza os dados de um medicamento cadastrado para o pet."
        )]
        [SwaggerResponse(
            StatusCodes.Status204NoContent,
            "Medicamento atualizado com sucesso."
        )]
        [SwaggerResponse(
            StatusCodes.Status400BadRequest,
            "Dados inválidos."
        )]
        [SwaggerResponse(
            StatusCodes.Status404NotFound,
            "Pet ou medicamento não encontrado."
        )]
        public IActionResult Atualizar(
            decimal petId,
            decimal medicamentoId,
            [FromBody] MedicamentoRequestDto dto)
        {
            try
            {
                var atualizado = _medicamentoUseCase.Atualizar(
                    petId,
                    medicamentoId,
                    dto
                );

                if (!atualizado)
                {
                    return NotFound(
                        new { mensagem = "Medicamento não encontrado." }
                    );
                }

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

        [HttpPatch("{medicamentoId}/status")]
        [SwaggerOperation(
            Summary = "Alterar status do medicamento",
            Description = "Altera o status para EM_USO, CONCLUIDO ou SUSPENSO."
        )]
        [SwaggerResponse(
            StatusCodes.Status204NoContent,
            "Status atualizado com sucesso."
        )]
        [SwaggerResponse(
            StatusCodes.Status400BadRequest,
            "Status inválido."
        )]
        [SwaggerResponse(
            StatusCodes.Status404NotFound,
            "Pet ou medicamento não encontrado."
        )]
        public IActionResult AtualizarStatus(
            decimal petId,
            decimal medicamentoId,
            [FromBody] MedicamentoStatusRequestDto dto)
        {
            try
            {
                var atualizado =
                    _medicamentoUseCase.AtualizarStatus(
                        petId,
                        medicamentoId,
                        dto
                    );

                if (!atualizado)
                {
                    return NotFound(
                        new { mensagem = "Medicamento não encontrado." }
                    );
                }

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

        [HttpDelete("{medicamentoId}")]
        [SwaggerOperation(
            Summary = "Excluir medicamento",
            Description = "Remove um medicamento cadastrado para o pet."
        )]
        [SwaggerResponse(
            StatusCodes.Status204NoContent,
            "Medicamento excluído com sucesso."
        )]
        [SwaggerResponse(
            StatusCodes.Status404NotFound,
            "Pet ou medicamento não encontrado."
        )]
        public IActionResult Excluir(
            decimal petId,
            decimal medicamentoId)
        {
            try
            {
                var excluido = _medicamentoUseCase.Excluir(
                    petId,
                    medicamentoId
                );

                if (!excluido)
                {
                    return NotFound(
                        new { mensagem = "Medicamento não encontrado." }
                    );
                }

                return NoContent();
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(new { mensagem = ex.Message });
            }
            
        }
    }
}