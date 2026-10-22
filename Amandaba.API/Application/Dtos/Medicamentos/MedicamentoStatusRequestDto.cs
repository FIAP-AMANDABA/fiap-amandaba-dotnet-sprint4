using System.ComponentModel.DataAnnotations;

namespace Amandaba.Application.Dtos.Medicamentos
{
    public class MedicamentoStatusRequestDto
    {
        [Required(ErrorMessage = "O status é obrigatório.")]
        public string Status { get; set; } = string.Empty;
    }
}