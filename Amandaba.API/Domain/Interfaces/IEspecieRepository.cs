using Amandaba.Domain.Entities;

namespace Amandaba.API.Domain.Interfaces
{
    public interface IEspecieRepository
    {
        IEnumerable<EspecieEntity> ObterTodas();
    }
}