using System.ComponentModel.DataAnnotations;

namespace Amandaba.Application.Dtos.Pets
{
    public class PetStatusRequestDto
    {
        [Required(ErrorMessage = "O status é obrigatório.")]
        public string Status { get; set; } = string.Empty;
    }
}