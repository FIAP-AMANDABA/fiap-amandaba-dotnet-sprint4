namespace Amandaba.API.Application.Interfaces;

public interface IPlanoCuidadosUseCase
{
    Task<PlanoCuidadosDto?> GerarAsync(decimal petId);
}

public class PlanoCuidadosDto
{
    public decimal PetId { get; set; }
    public string Nome { get; set; } = string.Empty;
    public string Especie { get; set; } = string.Empty;
    public string PlanoGerado { get; set; } = string.Empty;
}
