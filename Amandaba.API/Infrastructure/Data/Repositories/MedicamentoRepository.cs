using Amandaba.Domain.Entities;
using Amandaba.Domain.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Amandaba.Infrastructure.Data.Repositories
{
    public class MedicamentoRepository : IMedicamentoRepository
    {
        private readonly ApplicationContext _context;

        public MedicamentoRepository(ApplicationContext context)
        {
            _context = context;
        }

        public bool ExistePet(decimal petId)
        {
            return _context.Pets
                .Any(p => p.IdPet == petId);
        }

        public IEnumerable<PetMedicamentoEntity> ObterPorPet(
            decimal petId,
            string? status)
        {
            var query = _context.Medicamentos
                .AsNoTracking()
                .Where(m => m.IdPet == petId);

            if (!string.IsNullOrWhiteSpace(status))
            {
                query = query.Where(m => m.Status == status);
            }

            return query
                .OrderByDescending(m => m.DataInicio)
                .ThenByDescending(m => m.IdRegistroMedicamento)
                .ToList();
        }

        public PetMedicamentoEntity? ObterPorId(
            decimal petId,
            decimal medicamentoId)
        {
            return _context.Medicamentos
                .FirstOrDefault(m =>
                    m.IdPet == petId &&
                    m.IdRegistroMedicamento == medicamentoId);
        }

        public PetMedicamentoEntity Cadastrar(
            PetMedicamentoEntity medicamento)
        {
            _context.Medicamentos.Add(medicamento);
            _context.SaveChanges();

            return medicamento;
        }

        public void Atualizar(PetMedicamentoEntity medicamento)
        {
            _context.Medicamentos.Update(medicamento);
            _context.SaveChanges();
        }

        public void Excluir(PetMedicamentoEntity medicamento)
        {
            _context.Medicamentos.Remove(medicamento);
            _context.SaveChanges();
        }
    }
}