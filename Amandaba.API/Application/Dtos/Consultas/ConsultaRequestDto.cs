using System.ComponentModel.DataAnnotations;

namespace Amandaba.Application.Dtos.Consultas
{
    public class ConsultaRequestDto
    {
        [Required(ErrorMessage = "A data da consulta é obrigatória.")]
        public DateTime DataConsulta { get; set; }

        [StringLength(5)]
        public string? Horario { get; set; }

        [StringLength(150)]
        public string? Veterinario { get; set; }

        [StringLength(150)]
        public string? Clinica { get; set; }

        [StringLength(500)]
        public string? Motivo { get; set; }

        [StringLength(1000)]
        public string? Sintomas { get; set; }

        [Range(0.01, 9999.99, ErrorMessage = "O peso deve ser maior que zero.")]
        public decimal? Peso { get; set; }

        [StringLength(2000)]
        public string? Diagnostico { get; set; }

        [StringLength(2000)]
        public string? Tratamento { get; set; }

        [StringLength(2000)]
        public string? Observacao { get; set; }

        public bool Retorno { get; set; }

        public DateTime? DataRetorno { get; set; }

        [Required(ErrorMessage = "O status é obrigatório.")]
        [StringLength(20)]
        public string Status { get; set; } = string.Empty;
    }
}