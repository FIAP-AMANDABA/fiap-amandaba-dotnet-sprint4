using Amandaba.API.Application.Interfaces;
using Amandaba.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace Amandaba.Infrastructure.Data.Repositories;

// Consulta os vinculos no Oracle, nunca aceita um ID livre enviado pelo cliente como identidade.
public class PetAccessService : IPetAccessService
{
    private readonly ApplicationContext _db;
    public PetAccessService(ApplicationContext db) { _db = db; }

    public decimal? ObterIdUsuario(ClaimsPrincipal usuario)
    {
        var claim = usuario.FindFirst("userId")?.Value;
        return decimal.TryParse(claim, out var id) && id > 0 ? id : null;
    }

    public decimal? ObterIdTutor(decimal idUsuario) => _db.Tutores
        .AsNoTracking().Where(t => t.IdUsuario == idUsuario)
        .Select(t => (decimal?)t.IdTutor).FirstOrDefault();

    public decimal? ObterIdVeterinario(decimal idUsuario) => _db.Veterinarios
        .AsNoTracking().Where(v => v.IdUsuario == idUsuario && v.Status == "ATIVO")
        .Select(v => (decimal?)v.IdVeterinario).FirstOrDefault();

    public bool ExistePet(decimal petId) => _db.Pets.AsNoTracking().Any(p => p.IdPet == petId);

    public bool PodeAcessarPet(decimal idUsuario, decimal idPet, bool somenteTutor, string perfil)
    {
        var ativo = _db.Usuarios.AsNoTracking().Any(u => u.IdUsuario == idUsuario && u.Status == "ATIVO");
        if (!ativo) return false;

        var pet = _db.Pets.AsNoTracking().Where(p => p.IdPet == idPet)
            .Select(p => new { p.IdTutor }).FirstOrDefault();
        if (pet is null) return false;

        var perfilTutor = _db.Tutores.AsNoTracking().Any(t => t.IdUsuario == idUsuario && t.IdTutor == pet.IdTutor);
        if (perfil == "TUTOR" && perfilTutor) return true;
        if (somenteTutor || perfil != "VETERINARIO") return false;

        return _db.PetVeterinarios.AsNoTracking().Any(pv =>
            pv.IdPet == idPet && pv.Status == "ATIVO" &&
            _db.Veterinarios.Any(v => v.IdVeterinario == pv.IdVeterinario &&
                v.IdUsuario == idUsuario && v.Status == "ATIVO"));
    }
}
