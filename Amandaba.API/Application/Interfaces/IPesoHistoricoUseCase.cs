using Amandaba.API.Application.Dtos.Pesos;
using Amandaba.Application.Dtos.Pesos;

namespace Amandaba.API.Application.Interfaces;

public interface IPesoHistoricoUseCase
{
    IEnumerable<PesoResponseDto> Exportar(decimal petId);
    int Sincronizar(decimal petId);
    PesoHistoricoDto? ConsultarMongo(decimal petId);
}
