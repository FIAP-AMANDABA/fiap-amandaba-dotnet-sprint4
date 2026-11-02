using System.Text.Json;
using Amandaba.API.Application.Interfaces;
using Amandaba.Application.Interfaces;

namespace Amandaba.API.Application.UseCases;

// Orquestracao fica na Application (aula 03/07); o controller conhece apenas a interface.
public class PlanoCuidadosUseCase : IPlanoCuidadosUseCase
{
    private readonly IPetUseCase _pets;
    private readonly IMedicamentoUseCase _medicamentos;
    private readonly IAlergiaUseCase _alergias;
    private readonly IDoencaUseCase _doencas;
    private readonly IGeminiService _gemini;

    public PlanoCuidadosUseCase(IPetUseCase pets, IMedicamentoUseCase medicamentos,
        IAlergiaUseCase alergias, IDoencaUseCase doencas, IGeminiService gemini)
    { _pets = pets; _medicamentos = medicamentos; _alergias = alergias;
      _doencas = doencas; _gemini = gemini; }

    public async Task<PlanoCuidadosDto?> GerarAsync(decimal petId)
    {
        var pet = _pets.ObterPorId(petId);
        if (pet is null) return null;

        var opcoes = new JsonSerializerOptions
        { WriteIndented = true, PropertyNamingPolicy = JsonNamingPolicy.CamelCase };
        var contexto = $"""
            Voce e um sistema de apoio veterinario. Analise dados do historico clinico
            e prepare orientacoes preventivas para o tutor. Nao substitua o veterinario.
            Pet: {JsonSerializer.Serialize(pet, opcoes)}
            Medicamentos: {JsonSerializer.Serialize(_medicamentos.ObterPorPet(petId, null), opcoes)}
            Alergias: {JsonSerializer.Serialize(_alergias.ObterPorPet(petId), opcoes)}
            Doencas: {JsonSerializer.Serialize(_doencas.ObterPorPet(petId), opcoes)}
            """;

        var resposta = await _gemini.GerarPlanoDeCuidadosAsync(pet.Nome, pet.Especie, contexto);
        return new PlanoCuidadosDto
        { PetId = petId, Nome = pet.Nome, Especie = pet.Especie, PlanoGerado = resposta };
    }
}
