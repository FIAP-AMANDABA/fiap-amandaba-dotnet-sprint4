using Microsoft.AspNetCore.Authorization;
using Amandaba.Application.Dtos.Pets;
using Amandaba.Application.Interfaces;
using Amandaba.Application.UseCases;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Amandaba.API.Application.UseCases;
using Amandaba.API.Application.Interfaces;
using Amandaba.API.Presentation;
using System.Linq;

namespace Amandaba.Presentation.Controllers
{
    [ApiController]
    [Authorize]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    [Route("api")]
    public class PetsController : ControllerBase
    {
        private readonly IPetUseCase _petUseCase;
        private readonly ILogger<PetsController> _logger;
        private readonly IPlanoCuidadosUseCase _planoCuidados;

        public PetsController(
            IPetUseCase petUseCase,
            ILogger<PetsController> logger,
            IPlanoCuidadosUseCase planoCuidados)
        {
            _petUseCase = petUseCase;
            _logger = logger;
            _planoCuidados = planoCuidados;
        }

        [HttpGet("tutores/{idTutor}/pets")]
        [SwaggerOperation(
            Summary = "Listar pets de um tutor",
            Description = "Retorna todos os pets, ativos e inativos, associados ao tutor."
        )]
        [SwaggerResponse(
            StatusCodes.Status200OK,
            "Pets retornados com sucesso."
        )]
        [SwaggerResponse(
            StatusCodes.Status404NotFound,
            "Tutor não encontrado."
        )]
        public IActionResult ObterPorTutor(decimal idTutor)
        {
            try
            {
                _logger.LogInformation(
                    "Buscando pets do tutor {IdTutor}",
                    idTutor
                );

                var pets = _petUseCase.ObterPorTutor(idTutor);

                _logger.LogInformation(
                    "Pets do tutor {IdTutor} retornados com sucesso",
                    idTutor
                );

                // GET de colecao mantem o array original, acrescentando links por pet.
                var lista = pets.ToList();
                foreach (var item in lista)
                    item.Links = HateoasLinks.ParaPet(item.IdPet);
                return Ok(lista);
            }
            catch (KeyNotFoundException ex)
            {
                _logger.LogWarning(
                    "Tutor {IdTutor} nao encontrado ao listar pets",
                    idTutor
                );

                return NotFound(new { mensagem = ex.Message });
            }
            
        }

        [HttpPost("tutores/{idTutor}/pets")]
        [SwaggerOperation(
            Summary = "Cadastrar pet",
            Description = "Cadastra um novo pet associado ao tutor informado."
        )]
        [SwaggerResponse(
            StatusCodes.Status201Created,
            "Pet cadastrado com sucesso."
        )]
        [SwaggerResponse(
            StatusCodes.Status400BadRequest,
            "Dados inválidos."
        )]
        [SwaggerResponse(
            StatusCodes.Status404NotFound,
            "Tutor não encontrado."
        )]
        public IActionResult Cadastrar(
            decimal idTutor,
            [FromBody] PetRequestDto dto)
        {
            try
            {
                _logger.LogInformation(
                    "Iniciando cadastro de pet para o tutor {IdTutor}",
                    idTutor
                );

                var pet = _petUseCase.Cadastrar(idTutor, dto);

                _logger.LogInformation(
                    "Pet {PetId} cadastrado com sucesso para o tutor {IdTutor}",
                    pet.IdPet,
                    idTutor
                );

                return CreatedAtAction(
                    nameof(ObterPorId),
                    new { petId = pet.IdPet },
                    pet
                );
            }
            catch (KeyNotFoundException ex)
            {
                _logger.LogWarning(
                    "Tutor {IdTutor} nao encontrado durante cadastro de pet",
                    idTutor
                );

                return NotFound(new { mensagem = ex.Message });
            }
            catch (ArgumentException ex)
            {
                _logger.LogWarning(
                    "Dados invalidos ao cadastrar pet para o tutor {IdTutor}: {Mensagem}",
                    idTutor,
                    ex.Message
                );

                return BadRequest(new { mensagem = ex.Message });
            }
            
        }

        [HttpGet("pets/{petId}")]
        [SwaggerOperation(
            Summary = "Consultar pet",
            Description = "Retorna os dados completos de um pet."
        )]
        [SwaggerResponse(
            StatusCodes.Status200OK,
            "Pet retornado com sucesso."
        )]
        [SwaggerResponse(
            StatusCodes.Status404NotFound,
            "Pet não encontrado."
        )]
        public IActionResult ObterPorId(decimal petId)
        {
            
                _logger.LogInformation(
                    "Buscando pet com ID {PetId}",
                    petId
                );

                var pet = _petUseCase.ObterPorId(petId);

                if (pet is null)
                {
                    _logger.LogWarning(
                        "Pet com ID {PetId} nao encontrado",
                        petId
                    );

                    return NotFound(
                        new { mensagem = "Pet não encontrado." }
                    );
                }

                _logger.LogInformation(
                    "Pet com ID {PetId} retornado com sucesso",
                    petId
                );

                // HATEOAS aula 08, preservando o formato do DTO para clientes atuais.
                pet.Links = HateoasLinks.ParaPet(petId,
                    User.FindFirst("perfil")?.Value != "TUTOR");
                return Ok(pet);
            
            
        }

        [HttpPut("pets/{petId}")]
        [SwaggerOperation(
            Summary = "Editar pet",
            Description = "Atualiza os dados cadastrais de um pet."
        )]
        [SwaggerResponse(
            StatusCodes.Status204NoContent,
            "Pet atualizado com sucesso."
        )]
        [SwaggerResponse(
            StatusCodes.Status400BadRequest,
            "Dados inválidos."
        )]
        [SwaggerResponse(
            StatusCodes.Status404NotFound,
            "Pet não encontrado."
        )]
        public IActionResult Atualizar(
            decimal petId,
            [FromBody] PetRequestDto dto)
        {
            try
            {
                _logger.LogInformation(
                    "Iniciando atualizacao do pet {PetId}",
                    petId
                );

                var atualizado = _petUseCase.Atualizar(petId, dto);

                if (!atualizado)
                {
                    _logger.LogWarning(
                        "Pet {PetId} nao encontrado durante atualizacao",
                        petId
                    );

                    return NotFound(
                        new { mensagem = "Pet não encontrado." }
                    );
                }

                _logger.LogInformation(
                    "Pet {PetId} atualizado com sucesso",
                    petId
                );

                return NoContent();
            }
            catch (ArgumentException ex)
            {
                _logger.LogWarning(
                    "Dados invalidos ao atualizar pet {PetId}: {Mensagem}",
                    petId,
                    ex.Message
                );

                return BadRequest(new { mensagem = ex.Message });
            }
            
        }

        [HttpPatch("pets/{petId}/status")]
        [SwaggerOperation(
            Summary = "Alterar status do pet",
            Description = "Ativa ou desativa um pet sem removê-lo fisicamente."
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
            "Pet não encontrado."
        )]
        public IActionResult AtualizarStatus(
            decimal petId,
            [FromBody] PetStatusRequestDto dto)
        {
            try
            {
                _logger.LogInformation(
                    "Alterando status do pet {PetId} para {Status}",
                    petId,
                    dto.Status
                );

                var atualizado = _petUseCase.AtualizarStatus(
                    petId,
                    dto
                );

                if (!atualizado)
                {
                    _logger.LogWarning(
                        "Pet {PetId} nao encontrado durante alteracao de status",
                        petId
                    );

                    return NotFound(
                        new { mensagem = "Pet não encontrado." }
                    );
                }

                _logger.LogInformation(
                    "Status do pet {PetId} alterado para {Status} com sucesso",
                    petId,
                    dto.Status
                );

                return NoContent();
            }
            catch (ArgumentException ex)
            {
                _logger.LogWarning(
                    "Status invalido informado para o pet {PetId}: {Mensagem}",
                    petId,
                    ex.Message
                );

                return BadRequest(new { mensagem = ex.Message });
            }
            
        }

        [HttpGet("pets/{petId}/plano-cuidados")]
        [SwaggerOperation(
            Summary = "Gerar plano de cuidados",
            Description = "Gera um plano de cuidados personalizado para o pet cruzando histórico médico e utilizando IA Generativa (Gemini)."
        )]
        [SwaggerResponse(
            StatusCodes.Status200OK,
            "Plano gerado com sucesso."
        )]
        [SwaggerResponse(
            StatusCodes.Status400BadRequest,
            "Erro na geração do plano."
        )]
        public async Task<IActionResult> ObterPlanoDeCuidados(decimal petId)
        {
            _logger.LogInformation("Gerando plano de cuidados IA do pet {PetId}", petId);
            var plano = await _planoCuidados.GerarAsync(petId);
            return plano is null
                ? NotFound(new { mensagem = "Pet nao encontrado no banco de dados." })
                : Ok(plano);
        }
    }
}
