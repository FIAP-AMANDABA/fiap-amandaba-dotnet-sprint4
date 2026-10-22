using System.ComponentModel.DataAnnotations;

namespace Amandaba.Application.Dtos.Pets
{
    public class PetRequestDto
    {
        [Required(ErrorMessage = "A espécie é obrigatória.")]
        [Range(1, 999999999999999999, ErrorMessage = "O identificador deve ser maior que zero.")]
        public decimal IdEspecie { get; set; }

        [Required(ErrorMessage = "O nome do pet é obrigatório.")]
        [StringLength(100, ErrorMessage = "O nome deve possuir no máximo 100 caracteres.")]
        public string Nome { get; set; } = string.Empty;

        [StringLength(500, ErrorMessage = "A referência da foto deve possuir no máximo 500 caracteres.")]
        public string? FotoUrl { get; set; }

        [StringLength(100, ErrorMessage = "A raça deve possuir no máximo 100 caracteres.")]
        public string? Raca { get; set; }

        [StringLength(10, ErrorMessage = "O sexo deve possuir no máximo 10 caracteres.")]
        public string? Sexo { get; set; }

        public DateTime? DataNascimento { get; set; }

        [StringLength(50, ErrorMessage = "A cor deve possuir no máximo 50 caracteres.")]
        public string? Cor { get; set; }

        public bool Castrado { get; set; }

        [StringLength(50, ErrorMessage = "O microchip deve possuir no máximo 50 caracteres.")]
        public string? Microchip { get; set; }
    }
}