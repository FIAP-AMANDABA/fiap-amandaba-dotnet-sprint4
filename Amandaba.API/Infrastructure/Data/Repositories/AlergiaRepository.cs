using Amandaba.Domain.Entities;
using Amandaba.Domain.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Amandaba.Infrastructure.Data.Repositories
{
    public class AlergiaRepository : IAlergiaRepository
    {
        private readonly ApplicationContext _context;

        public AlergiaRepository(ApplicationContext context)
        {
            _context = context;
        }

        public bool ExistePet(decimal petId)
        {
            return _context.Pets
                .Any(p => p.IdPet == petId);
        }

        public IEnumerable<PetAlergiaEntity> ObterPorPet(decimal petId)
        {
            return _context.Alergias
                .AsNoTracking()
                .Where(a => a.IdPet == petId)
                .OrderByDescending(a => a.DataIdentificacao)
                .ThenByDescending(a => a.IdRegistroAlergia)
                .ToList();
        }

        public PetAlergiaEntity? ObterPorId(
            decimal petId,
            decimal alergiaId)
        {
            return _context.Alergias
                .FirstOrDefault(a =>
                    a.IdPet == petId &&
                    a.IdRegistroAlergia == alergiaId);
        }

        public PetAlergiaEntity Cadastrar(PetAlergiaEntity alergia)
        {
            _context.Alergias.Add(alergia);
            _context.SaveChanges();

            return alergia;
        }

        public void Atualizar(PetAlergiaEntity alergia)
        {
            _context.Alergias.Update(alergia);
            _context.SaveChanges();
        }

        public void Excluir(PetAlergiaEntity alergia)
        {
            _context.Alergias.Remove(alergia);
            _context.SaveChanges();
        }
    }
}