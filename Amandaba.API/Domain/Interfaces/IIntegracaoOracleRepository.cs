namespace Amandaba.API.Domain.Interfaces;

public interface IIntegracaoOracleRepository
{
    Task ExecutarPetJsonAsync(decimal idTutor);
    Task<int?> CalcularIdadePetAsync(decimal petId);
}
