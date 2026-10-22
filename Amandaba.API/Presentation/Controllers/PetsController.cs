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
using System.Text.Json;
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

        public PetsController(
            IPetUseCase petUseCase,
            ILogger<PetsController> logger)
        {
            _petUseCase = petUseCase;
            _logger = logger;
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

                return Ok(pets);
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
        public async Task<IActionResult> ObterPlanoDeCuidados(
            decimal petId, 
            [FromServices] GeminiUseCase geminiService,
            [FromServices] IServiceProvider serviceProvider)
        {
            
                _logger.LogInformation(
                    "Gerando plano de cuidados IA para o pet {PetId}", 
                    petId
                );

                var pet = _petUseCase.ObterPorId(petId);
                
                if (pet == null)
                {
                    return NotFound(new { mensagem = "Pet não encontrado no banco de dados." });
                }

                var tipoPet = pet.GetType();
                var nomePet = tipoPet.GetProperty("Nome")?.GetValue(pet)?.ToString() 
                           ?? tipoPet.GetProperty("NmPet")?.GetValue(pet)?.ToString() 
                           ?? "Pet";
                           
                var especie = tipoPet.GetProperty("Especie")?.GetValue(pet)?.ToString() 
                           ?? tipoPet.GetProperty("NmEspecie")?.GetValue(pet)?.ToString() 
                           ?? "Desconhecida";

                var medicamentos = ObterDadosDinamicos(serviceProvider, "MedicamentoUseCase", petId);
                var alergias = ObterDadosDinamicos(serviceProvider, "AlergiaUseCase", petId);
                var doencas = ObterDadosDinamicos(serviceProvider, "DoencaUseCase", petId);

                var opcoesJson = new JsonSerializerOptions { WriteIndented = true, PropertyNamingPolicy = JsonNamingPolicy.CamelCase };
                
                var contextoClinicoCruzado = $@"
Você é um sistema veterinário avançado. Analise os dados reais do banco de dados em formato JSON abaixo e crie um plano de cuidados seguro e amigável.
Cruze ativamente as informações (Ex: Se o pet tem alergia a X e está tomando Y, cite cuidados pertinentes).

DADOS TÉCNICOS:
- Pet: {JsonSerializer.Serialize(pet, opcoesJson)}
- Medicamentos Atuais/Histórico: {(medicamentos != null ? JsonSerializer.Serialize(medicamentos, opcoesJson) : "[]")}
- Quadro de Alergias: {(alergias != null ? JsonSerializer.Serialize(alergias, opcoesJson) : "[]")}
- Quadro de Doenças: {(doencas != null ? JsonSerializer.Serialize(doencas, opcoesJson) : "[]")}
";

                var recomendacaoIA = await geminiService.GerarPlanoDeCuidadosAsync(nomePet, especie, contextoClinicoCruzado);

                _logger.LogInformation(
                    "Plano de cuidados IA gerado com sucesso para o pet {PetId}", 
                    petId
                );

                return Ok(new { PetId = petId, Nome = nomePet, Especie = especie, PlanoGerado = recomendacaoIA });
            
            
        }

        private object? ObterDadosDinamicos(IServiceProvider provider, string useCaseSuffix, decimal petId)
        {
            
                var tipoInterface = AppDomain.CurrentDomain.GetAssemblies()
                    .SelectMany(a => a.GetTypes())
                    .FirstOrDefault(t => t.IsInterface && t.Name.EndsWith(useCaseSuffix));
                    
                if (tipoInterface != null)
                {
                    var servico = provider.GetService(tipoInterface);
                    if (servico != null)
                    {
                        var metodo = tipoInterface.GetMethod("ObterPorPet") 
                                  ?? tipoInterface.GetMethod("ObterTodos") 
                                  ?? tipoInterface.GetMethod("ObterPorIdPet");
                                  
                        if (metodo != null)
                        {
                            return metodo.Invoke(servico, new object[] { petId });
                        }
                    }
                }
            
            
            
            return null;
        }
    }
}