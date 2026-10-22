using Amandaba.API.Domain.Interfaces;
using Amandaba.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Amandaba.Infrastructure.Data.Repositories
{
    public class EspecieRepository : IEspecieRepository
    {
        private readonly ApplicationContext _context;

        public EspecieRepository(ApplicationContext context)
        {
            _context = context;
        }

        public IEnumerable<EspecieEntity> ObterTodas()
        {
            return _context.Especies
                .AsNoTracking()
                .OrderBy(e => e.Nome)
                .ToList();
        }
    }
}