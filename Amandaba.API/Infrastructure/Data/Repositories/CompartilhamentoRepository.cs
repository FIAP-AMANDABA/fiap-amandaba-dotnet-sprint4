using Amandaba.API.Domain.Interfaces;
using Amandaba.Domain.Entities;
using Amandaba.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace Amandaba.Infrastructure.Data.Repositories;
public class CompartilhamentoRepository : ICompartilhamentoRepository
{
    private readonly ApplicationContext _db;
    public CompartilhamentoRepository(ApplicationContext db) { _db = db; }
    public VeterinarioEntity? BuscarVeterinario(decimal id) => _db.Veterinarios
        .AsNoTracking().FirstOrDefault(v => v.IdVeterinario == id);
    public PetVeterinarioEntity? BuscarVinculo(decimal petId, decimal veterinarioId) =>
        _db.PetVeterinarios.FirstOrDefault(p => p.IdPet == petId && p.IdVeterinario == veterinarioId);
    public IEnumerable<PetVeterinarioEntity> ListarPorPet(decimal petId) => _db.PetVeterinarios
        .AsNoTracking().Where(p => p.IdPet == petId).OrderBy(p => p.IdVeterinario).ToList();
    public IEnumerable<PetEntity> ListarPetsVeterinario(decimal veterinarioId) => _db.Pets
        .AsNoTracking().Include(p => p.Especie)
        .Where(p => _db.PetVeterinarios.Any(v => v.IdVeterinario == veterinarioId && v.IdPet == p.IdPet && v.Status == "ATIVO"))
        .OrderBy(p => p.Nome).ToList();
    public (IEnumerable<PetEntity> data, int rowsCount) ListarPetsVeterinarioPaginado(
        decimal veterinarioId, int deslocamento, int registrosRetornados)
    {
        var consulta = _db.Pets.AsNoTracking().Include(p => p.Especie)
            .Where(p => _db.PetVeterinarios.Any(v => v.IdVeterinario == veterinarioId &&
                v.IdPet == p.IdPet && v.Status == "ATIVO"));
        var total = consulta.Count();
        return (consulta.OrderBy(p => p.Nome).ThenBy(p => p.IdPet)
            .Skip(deslocamento).Take(registrosRetornados).ToList(), total);
    }

    public void Salvar(PetVeterinarioEntity vinculo)
    {
        if (vinculo.IdPetVeterinario == 0) _db.PetVeterinarios.Add(vinculo);
        _db.SaveChanges();
    }
}
