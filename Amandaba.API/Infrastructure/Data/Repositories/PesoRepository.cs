using Amandaba.API.Domain.Interfaces;
using Amandaba.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Amandaba.Infrastructure.Data.Repositories
{
    public class PesoRepository : IPesoRepository
    {
        private readonly ApplicationContext _context;

        public PesoRepository(ApplicationContext context)
        {
            _context = context;
        }

        public bool ExistePet(decimal idPet)
        {
            return _context.Pets
                .Any(p => p.IdPet == idPet);
        }

        public IEnumerable<PetPesoEntity> ObterPorPet(decimal idPet)
        {
            return _context.Pesos
                .AsNoTracking()
                .Where(p => p.IdPet == idPet)
                .OrderByDescending(p => p.DataMedicao)
                .ToList();
        }

        public PetPesoEntity? ObterPorId(decimal idPet, decimal pesoId)
        {
            return _context.Pesos
                .FirstOrDefault(p =>
                    p.IdPet == idPet &&
                    p.IdHistoricoPeso == pesoId);
        }

        public PetPesoEntity? ObterAtual(decimal idPet)
        {
            return _context.Pesos
                .AsNoTracking()
                .Where(p => p.IdPet == idPet)
                .OrderByDescending(p => p.DataMedicao)
                .ThenByDescending(p => p.IdHistoricoPeso)
                .FirstOrDefault();
        }

        public PetPesoEntity Cadastrar(PetPesoEntity peso)
        {
            _context.Pesos.Add(peso);
            _context.SaveChanges();

            return peso;
        }

        public void Atualizar(PetPesoEntity peso)
        {
            _context.Pesos.Update(peso);
            _context.SaveChanges();
        }

        public void Excluir(PetPesoEntity peso)
        {
            _context.Pesos.Remove(peso);
            _context.SaveChanges();
        }
    }
}