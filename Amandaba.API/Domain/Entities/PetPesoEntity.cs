namespace Amandaba.Domain.Entities
{
    public class PetPesoEntity
    {
        public decimal IdHistoricoPeso { get; set; }

        public decimal IdPet { get; set; }

        public decimal Peso { get; set; }

        public DateTime DataMedicao { get; set; }

        public DateTime DataCadastro { get; set; }

        public PetEntity Pet { get; set; } = null!;
    }
}