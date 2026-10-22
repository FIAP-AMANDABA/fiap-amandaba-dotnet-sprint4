using System.ComponentModel.DataAnnotations;

namespace Amandaba.Application.Dtos.Vacinas
{
    public class VacinaAplicacaoRequestDto
    {
        [Required(ErrorMessage = "A vacina é obrigatória.")]
        [Range(1, 999999999999999999, ErrorMessage = "O identificador deve ser maior que zero.")]
        public decimal IdVacina { get; set; }

        [Required(ErrorMessage = "A data de aplicação é obrigatória.")]
        public DateTime DataAplicacao { get; set; }

        [Range(1, 999, ErrorMessage = "O número da dose deve ser maior que zero.")]
        public decimal? NumeroDose { get; set; }

        [StringLength(50)]
        public string? NumeroLote { get; set; }

        public DateTime? ProximaDose { get; set; }

        [StringLength(150)]
        public string? Clinica { get; set; }

        [StringLength(1000)]
        public string? Observacao { get; set; }

        [StringLength(500)]
        public string? Comprovante { get; set; }
    }
}