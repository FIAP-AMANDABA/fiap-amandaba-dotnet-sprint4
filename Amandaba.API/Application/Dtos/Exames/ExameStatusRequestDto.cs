using System.ComponentModel.DataAnnotations;

namespace Amandaba.Application.Dtos.Exames
{
    public class ExameStatusRequestDto
    {
        [Required(ErrorMessage = "O status é obrigatório.")]
        public string Status { get; set; } = string.Empty;
    }
}