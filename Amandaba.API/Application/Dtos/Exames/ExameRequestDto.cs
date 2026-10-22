using System.ComponentModel.DataAnnotations;

namespace Amandaba.Application.Dtos.Exames
{
    public class ExameRequestDto
    {
        [Required(ErrorMessage = "O nome do exame é obrigatório.")]
        [StringLength(150)]
        public string Nome { get; set; } = string.Empty;

        public DateTime? DataSolicitacao { get; set; }

        public DateTime? DataRealizacao { get; set; }

        [StringLength(150)]
        public string? Veterinario { get; set; }

        [StringLength(150)]
        public string? Clinica { get; set; }

        [StringLength(500)]
        public string? Motivo { get; set; }

        [StringLength(4000)]
        public string? Resultado { get; set; }

        [StringLength(2000)]
        public string? Observacao { get; set; }

        [StringLength(500)]
        public string? Arquivo { get; set; }

        [Required(ErrorMessage = "O status é obrigatório.")]
        [StringLength(30)]
        public string Status { get; set; } = string.Empty;
    }
}