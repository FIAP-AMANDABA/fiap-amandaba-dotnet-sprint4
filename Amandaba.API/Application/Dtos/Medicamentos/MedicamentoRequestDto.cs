using System.ComponentModel.DataAnnotations;

namespace Amandaba.Application.Dtos.Medicamentos
{
    public class MedicamentoRequestDto
    {
        [Required(ErrorMessage = "O nome do medicamento é obrigatório.")]
        [StringLength(250)]
        public string Nome { get; set; } = string.Empty;

        [StringLength(500)]
        public string? Motivo { get; set; }

        [Range(0.01, 99999999.99, ErrorMessage = "A dosagem deve ser maior que zero.")]
        public decimal? Dosagem { get; set; }

        [StringLength(30)]
        public string? Unidade { get; set; }

        [StringLength(50)]
        public string? Quantidade { get; set; }

        [StringLength(100)]
        public string? Frequencia { get; set; }

        [StringLength(100)]
        public string? Administracao { get; set; }

        [StringLength(500)]
        public string? Horario { get; set; }

        [Required(ErrorMessage = "A data de início é obrigatória.")]
        public DateTime DataInicio { get; set; }

        public DateTime? DataTermino { get; set; }

        [Required(ErrorMessage = "O status é obrigatório.")]
        [StringLength(20)]
        public string Status { get; set; } = string.Empty;

        [StringLength(500)]
        public string? Prescricao { get; set; }

        [StringLength(1000)]
        public string? Observacao { get; set; }
    }
}