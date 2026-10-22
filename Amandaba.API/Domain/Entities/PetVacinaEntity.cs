namespace Amandaba.Domain.Entities
{
    public class PetVacinaEntity
    {
        public decimal IdAplicacaoVacina { get; set; }

        public decimal IdPet { get; set; }

        public decimal IdVacina { get; set; }

        public DateTime DataAplicacao { get; set; }

        public decimal? NumeroDose { get; set; }

        public string? NumeroLote { get; set; }

        public DateTime? ProximaDose { get; set; }

        public string? Clinica { get; set; }

        public string? Observacao { get; set; }

        public string? Comprovante { get; set; }

        public DateTime DataCadastro { get; set; }

        public PetEntity Pet { get; set; } = null!;

        public VacinaEntity Vacina { get; set; } = null!;
    }
}