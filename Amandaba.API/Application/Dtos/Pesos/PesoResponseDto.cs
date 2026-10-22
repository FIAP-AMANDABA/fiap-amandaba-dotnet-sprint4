namespace Amandaba.Application.Dtos.Pesos
{
    public class PesoResponseDto
    {
        public decimal IdHistoricoPeso { get; set; }

        public decimal IdPet { get; set; }

        public decimal Peso { get; set; }

        public DateTime DataMedicao { get; set; }

        public DateTime DataCadastro { get; set; }
    }
}