using Amandaba.Domain.Entities;
using Amandaba.Domain.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Amandaba.Infrastructure.Data.Repositories
{
    public class ExameRepository : IExameRepository
    {
        private readonly ApplicationContext _context;

        public ExameRepository(ApplicationContext context)
        {
            _context = context;
        }

        public bool ExistePet(decimal petId)
        {
            return _context.Pets
                .Any(p => p.IdPet == petId);
        }

        public IEnumerable<ExameEntity> ObterPorPet(
            decimal petId,
            string? status)
        {
            var query = _context.Exames
                .AsNoTracking()
                .Where(e => e.IdPet == petId);

            if (!string.IsNullOrWhiteSpace(status))
            {
                query = query.Where(e => e.Status == status);
            }

            return query
                .OrderByDescending(e => e.DataSolicitacao)
                .ThenByDescending(e => e.IdExame)
                .ToList();
        }

        public ExameEntity? ObterPorId(
            decimal petId,
            decimal exameId)
        {
            return _context.Exames
                .FirstOrDefault(e =>
                    e.IdPet == petId &&
                    e.IdExame == exameId);
        }

        public ExameEntity Cadastrar(ExameEntity exame)
        {
            _context.Exames.Add(exame);
            _context.SaveChanges();

            return exame;
        }

        public void Atualizar(ExameEntity exame)
        {
            _context.Exames.Update(exame);
            _context.SaveChanges();
        }

        public void Excluir(ExameEntity exame)
        {
            _context.Exames.Remove(exame);
            _context.SaveChanges();
        }
    }
}