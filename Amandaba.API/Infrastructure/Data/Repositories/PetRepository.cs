using Amandaba.API.Domain.Interfaces;
using Amandaba.Domain.Entities;
using Amandaba.Domain.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Amandaba.Infrastructure.Data.Repositories
{
    public class PetRepository : IPetRepository
    {
        private readonly ApplicationContext _context;

        public PetRepository(ApplicationContext context)
        {
            _context = context;
        }

        public IEnumerable<PetEntity> ObterPorTutor(decimal idTutor)
        {
            return _context.Pets
                .AsNoTracking()
                .Include(p => p.Especie)
                .Where(p => p.IdTutor == idTutor)
                .OrderBy(p => p.Nome)
                .ToList();
        }

        public PetEntity? ObterPorId(decimal idPet)
        {
            return _context.Pets
                .Include(p => p.Especie)
                .FirstOrDefault(p => p.IdPet == idPet);
        }

        public PetEntity Cadastrar(PetEntity pet)
        {
            _context.Pets.Add(pet);
            _context.SaveChanges();

            return pet;
        }

        public void Atualizar(PetEntity pet)
        {
            _context.SaveChanges();
        }

        public bool ExisteTutor(decimal idTutor)
        {
            return _context.Tutores
                .Any(t => t.IdTutor == idTutor);
        }

        public bool ExisteEspecie(decimal idEspecie)
        {
            return _context.Especies
                .Any(e => e.IdEspecie == idEspecie);
        }
    }
}