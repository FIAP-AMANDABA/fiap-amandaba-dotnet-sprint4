namespace Amandaba.Application.Dtos.Pesos
{
    public class PesoResponseDto
    {
        public List<Amandaba.API.Application.Dtos.LinkDto> Links { get; set; } = new();

        public decimal IdHistoricoPeso { get; set; }

        public decimal IdPet { get; set; }

        public decimal Peso { get; set; }

        public DateTime DataMedicao { get; set; }

        public DateTime DataCadastro { get; set; }
    }
}