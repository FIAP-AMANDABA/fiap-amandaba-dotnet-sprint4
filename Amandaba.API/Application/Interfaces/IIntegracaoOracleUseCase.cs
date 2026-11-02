namespace Amandaba.API.Application.Interfaces;

public interface IIntegracaoOracleUseCase
{
    Task ExecutarPetJsonAsync(decimal idTutor);
    Task<int?> CalcularIdadePetAsync(decimal petId);
}
