using Amandaba.Domain.Entities;
using Amandaba.Domain.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Amandaba.Infrastructure.Data.Repositories
{
    public class VacinaRepository : IVacinaRepository
    {
        private readonly ApplicationContext _context;

        public VacinaRepository(ApplicationContext context)
        {
            _context = context;
        }

        public IEnumerable<VacinaEntity> ObterCatalogo()
        {
            return _context.Vacinas
                .AsNoTracking()
                .OrderBy(v => v.Nome)
                .ToList();
        }

        public VacinaEntity? ObterVacinaPorId(decimal vacinaId)
        {
            return _context.Vacinas
                .AsNoTracking()
                .FirstOrDefault(v => v.IdVacina == vacinaId);
        }

        public bool ExistePet(decimal petId)
        {
            return _context.Pets
                .Any(p => p.IdPet == petId);
        }

        public IEnumerable<PetVacinaEntity> ObterAplicacoesPorPet(decimal petId)
        {
            return _context.PetVacinas
                .AsNoTracking()
                .Include(v => v.Vacina)
                .Where(v => v.IdPet == petId)
                .OrderByDescending(v => v.DataAplicacao)
                .ToList();
        }

        public PetVacinaEntity? ObterAplicacaoPorId(
            decimal petId,
            decimal aplicacaoId)
        {
            return _context.PetVacinas
                .Include(v => v.Vacina)
                .FirstOrDefault(v =>
                    v.IdPet == petId &&
                    v.IdAplicacaoVacina == aplicacaoId);
        }

        public PetVacinaEntity CadastrarAplicacao(PetVacinaEntity aplicacao)
        {
            _context.PetVacinas.Add(aplicacao);
            _context.SaveChanges();

            return aplicacao;
        }

        public void AtualizarAplicacao(PetVacinaEntity aplicacao)
        {
            _context.SaveChanges();
        }

        public void ExcluirAplicacao(PetVacinaEntity aplicacao)
        {
            _context.PetVacinas.Remove(aplicacao);
            _context.SaveChanges();
        }
    }
}