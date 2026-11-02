namespace Amandaba.API.Application.Dtos.Pesos;

public class PesoHistoricoDto
{
    public string Id { get; set; } = string.Empty;
    public decimal PetId { get; set; }
    public DateTime AtualizadoEm { get; set; }
    public List<MedicaoPesoDto> Historico { get; set; } = new();
}

public class MedicaoPesoDto
{
    public decimal PesoId { get; set; }
    public decimal Peso { get; set; }
    public DateTime DataMedicao { get; set; }
}
