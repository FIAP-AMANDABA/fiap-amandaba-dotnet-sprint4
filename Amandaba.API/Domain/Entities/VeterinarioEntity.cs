namespace Amandaba.Domain.Entities;

public class VeterinarioEntity
{
    public decimal IdVeterinario { get; set; }
    public decimal IdUsuario { get; set; }
    public string Crmv { get; set; } = string.Empty;
    public string Status { get; set; } = "ATIVO";
}
