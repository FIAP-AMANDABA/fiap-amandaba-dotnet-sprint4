namespace Amandaba.Domain.Entities
{
    public class PetDoencaEntity
    {
        public decimal IdRegistroDoenca { get; set; }

        public decimal IdPet { get; set; }

        public string Nome { get; set; } = string.Empty;

        public DateTime? DataDiagnostico { get; set; }

        public string Status { get; set; } = string.Empty;

        public string? Tratamento { get; set; }

        public string? Observacao { get; set; }

        public DateTime DataCadastro { get; set; }

        public PetEntity Pet { get; set; } = null!;
    }
}