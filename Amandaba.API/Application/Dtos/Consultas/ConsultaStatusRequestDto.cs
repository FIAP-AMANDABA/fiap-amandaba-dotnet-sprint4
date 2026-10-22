using System.ComponentModel.DataAnnotations;

namespace Amandaba.Application.Dtos.Consultas
{
    public class ConsultaStatusRequestDto
    {
        [Required(ErrorMessage = "O status é obrigatório.")]
        public string Status { get; set; } = string.Empty;
    }
}