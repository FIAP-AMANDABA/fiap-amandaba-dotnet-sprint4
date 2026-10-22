namespace Amandaba.Domain.Entities;

public class PetVeterinarioEntity
{
    public decimal IdPetVeterinario { get; set; }
    public decimal IdPet { get; set; }
    public decimal IdVeterinario { get; set; }
    public DateTime DataCompartilhamento { get; set; }
    public string Status { get; set; } = "ATIVO";
}
