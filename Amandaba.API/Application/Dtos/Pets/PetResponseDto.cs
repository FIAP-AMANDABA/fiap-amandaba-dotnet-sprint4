namespace Amandaba.Application.Dtos.Pets
{
    public class PetResponseDto
    {
        public decimal IdPet { get; set; }

        public decimal IdTutor { get; set; }

        public decimal IdEspecie { get; set; }

        public string Especie { get; set; } = string.Empty;

        public string Nome { get; set; } = string.Empty;

        public string? FotoUrl { get; set; }

        public string? Raca { get; set; }

        public string? Sexo { get; set; }

        public DateTime? DataNascimento { get; set; }

        public string? Cor { get; set; }

        public bool Castrado { get; set; }

        public string? Microchip { get; set; }

        public DateTime DataCadastro { get; set; }

        public string Status { get; set; } = string.Empty;
    }
}