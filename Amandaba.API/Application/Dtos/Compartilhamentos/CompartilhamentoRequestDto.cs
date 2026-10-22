using System.ComponentModel.DataAnnotations;
namespace Amandaba.API.Application.Dtos.Compartilhamentos;
public class CompartilhamentoRequestDto
{
    [Range(1, double.MaxValue)]
    public decimal VeterinarioId { get; set; }
}
