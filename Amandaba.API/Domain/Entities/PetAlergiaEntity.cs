namespace Amandaba.Domain.Entities
{
    public class PetAlergiaEntity
    {
        public decimal IdRegistroAlergia { get; set; }

        public decimal IdPet { get; set; }

        public string Nome { get; set; } = string.Empty;

        public string? Tipo { get; set; }

        public DateTime? DataIdentificacao { get; set; }

        public string? Reacao { get; set; }

        public string? Gravidade { get; set; }

        public string? Observacao { get; set; }

        public DateTime DataCadastro { get; set; }

        public PetEntity Pet { get; set; } = null!;
    }
}