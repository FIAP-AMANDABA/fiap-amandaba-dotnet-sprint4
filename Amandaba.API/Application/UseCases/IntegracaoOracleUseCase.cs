using Amandaba.API.Application.Interfaces;
using Amandaba.API.Domain.Interfaces;

namespace Amandaba.API.Application.UseCases;

public class IntegracaoOracleUseCase : IIntegracaoOracleUseCase
{
    private readonly IIntegracaoOracleRepository _repositorio;
    public IntegracaoOracleUseCase(IIntegracaoOracleRepository repositorio)
    { _repositorio = repositorio; }

    public Task ExecutarPetJsonAsync(decimal idTutor) =>
        _repositorio.ExecutarPetJsonAsync(idTutor);

    public Task<int?> CalcularIdadePetAsync(decimal petId) =>
        _repositorio.CalcularIdadePetAsync(petId);
}
