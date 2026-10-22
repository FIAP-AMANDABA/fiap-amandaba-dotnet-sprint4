using Amandaba.Domain.Entities;
using Amandaba.Domain.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Amandaba.Infrastructure.Data.Repositories
{
    public class ConsultaRepository : IConsultaRepository
    {
        private readonly ApplicationContext _context;

        public ConsultaRepository(ApplicationContext context)
        {
            _context = context;
        }

        public bool ExistePet(decimal petId)
        {
            return _context.Pets
                .Any(p => p.IdPet == petId);
        }

        public IEnumerable<ConsultaEntity> ObterPorPet(
            decimal petId,
            string? status)
        {
            var query = _context.Consultas
                .AsNoTracking()
                .Where(c => c.IdPet == petId);

            if (!string.IsNullOrWhiteSpace(status))
            {
                query = query.Where(c => c.Status == status);
            }

            return query
                .OrderByDescending(c => c.DataConsulta)
                .ThenByDescending(c => c.IdConsulta)
                .ToList();
        }

        public ConsultaEntity? ObterPorId(
            decimal petId,
            decimal consultaId)
        {
            return _context.Consultas
                .FirstOrDefault(c =>
                    c.IdPet == petId &&
                    c.IdConsulta == consultaId);
        }

        public ConsultaEntity Cadastrar(ConsultaEntity consulta)
        {
            _context.Consultas.Add(consulta);
            _context.SaveChanges();

            return consulta;
        }

        public void Atualizar(ConsultaEntity consulta)
        {
            _context.Consultas.Update(consulta);
            _context.SaveChanges();
        }

        public void Excluir(ConsultaEntity consulta)
        {
            _context.Consultas.Remove(consulta);
            _context.SaveChanges();
        }
    }
}