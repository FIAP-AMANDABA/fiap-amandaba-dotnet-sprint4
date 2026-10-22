using System.ComponentModel.DataAnnotations;

namespace Amandaba.Application.Dtos.Doencas
{
    public class DoencaRequestDto
    {
        [Required(ErrorMessage = "O nome da doença é obrigatório.")]
        [StringLength(250, ErrorMessage = "O nome da doença deve possuir no máximo 250 caracteres.")]
        public string Nome { get; set; } = string.Empty;

        public DateTime? DataDiagnostico { get; set; }

        [Required(ErrorMessage = "O status é obrigatório.")]
        [StringLength(20)]
        public string Status { get; set; } = string.Empty;

        [StringLength(1000)]
        public string? Tratamento { get; set; }

        [StringLength(1000)]
        public string? Observacao { get; set; }
    }
}