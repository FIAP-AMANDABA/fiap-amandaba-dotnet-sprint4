using System.ComponentModel.DataAnnotations;

namespace Amandaba.Application.Dtos.Pesos
{
    public class PesoRequestDto
    {
        [Range(0.01, 9999.99, ErrorMessage = "O peso deve ser maior que zero.")]
        public decimal Peso { get; set; }

        [Required(ErrorMessage = "A data da medição é obrigatória.")]
        public DateTime DataMedicao { get; set; }
    }
}