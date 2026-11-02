using Amandaba.API.Application.Dtos;
using Amandaba.Application.Dtos.Pets;

namespace Amandaba.API.Application.Dtos.Pets;

public class PetItemDto
{
    public PetResponseDto Pet { get; set; } = new();
    public List<LinkDto> Links { get; set; } = new();
}
