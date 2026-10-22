using Amandaba.Domain.Entities;
using Amandaba.Domain.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Amandaba.Infrastructure.Data.Repositories
{
    public class DoencaRepository : IDoencaRepository
    {
        private readonly ApplicationContext _context;

        public DoencaRepository(ApplicationContext context)
        {
            _context = context;
        }

        public bool ExistePet(decimal petId)
        {
            return _context.Pets
                .Any(p => p.IdPet == petId);
        }

        public IEnumerable<PetDoencaEntity> ObterPorPet(decimal petId)
        {
            return _context.Doencas
                .AsNoTracking()
                .Where(d => d.IdPet == petId)
                .OrderByDescending(d => d.DataDiagnostico)
                .ThenByDescending(d => d.IdRegistroDoenca)
                .ToList();
        }

        public PetDoencaEntity? ObterPorId(
            decimal petId,
            decimal doencaId)
        {
            return _context.Doencas
                .FirstOrDefault(d =>
                    d.IdPet == petId &&
                    d.IdRegistroDoenca == doencaId);
        }

        public PetDoencaEntity Cadastrar(PetDoencaEntity doenca)
        {
            _context.Doencas.Add(doenca);
            _context.SaveChanges();

            return doenca;
        }

        public void Atualizar(PetDoencaEntity doenca)
        {
            _context.Doencas.Update(doenca);
            _context.SaveChanges();
        }

        public void Excluir(PetDoencaEntity doenca)
        {
            _context.Doencas.Remove(doenca);
            _context.SaveChanges();
        }
    }
}