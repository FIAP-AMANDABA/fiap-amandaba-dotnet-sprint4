using System.ComponentModel.DataAnnotations;

namespace Amandaba.API.Application.Dtos.Alergias
{
    public class AlergiaRequestDto
    {
        [Required(ErrorMessage = "O nome da alergia é obrigatório.")]
        [StringLength(250)]
        public string Nome { get; set; } = string.Empty;

        [StringLength(30)]
        public string? Tipo { get; set; }

        public DateTime? DataIdentificacao { get; set; }

        [StringLength(500)]
        public string? Reacao { get; set; }

        [StringLength(20)]
        public string? Gravidade { get; set; }

        [StringLength(1000)]
        public string? Observacao { get; set; }
    }
}