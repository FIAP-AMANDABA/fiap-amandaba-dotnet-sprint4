using Amandaba.Application.Dtos.Especies;

namespace Amandaba.Application.Interfaces
{
    public interface IEspecieUseCase
    {
        IEnumerable<EspecieResponseDto> ObterTodas();
    }
}