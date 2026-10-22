using Amandaba.API.Application.Interfaces;
using Amandaba.API.Domain.Interfaces;
using Amandaba.Domain.Entities;

namespace Amandaba.API.Application.UseCases;
public class CompartilhamentoUseCase : ICompartilhamentoUseCase
{
    private readonly ICompartilhamentoRepository _repo;
    public CompartilhamentoUseCase(ICompartilhamentoRepository repo) { _repo = repo; }

    public PetVeterinarioEntity Compartilhar(decimal petId, decimal veterinarioId)
    {
        var vet = _repo.BuscarVeterinario(veterinarioId);
        if (vet is null || vet.Status != "ATIVO") throw new ArgumentException("Veterinario inexistente ou inativo.");
        var vinculo = _repo.BuscarVinculo(petId, veterinarioId);
        if (vinculo is null) vinculo = new PetVeterinarioEntity { IdPet = petId, IdVeterinario = veterinarioId };
        vinculo.Status = "ATIVO";
        vinculo.DataCompartilhamento = DateTime.UtcNow;
        _repo.Salvar(vinculo);
        return vinculo;
    }

    public IEnumerable<PetVeterinarioEntity> Listar(decimal petId) => _repo.ListarPorPet(petId);

    public bool Revogar(decimal petId, decimal veterinarioId)
    {
        var vinculo = _repo.BuscarVinculo(petId, veterinarioId);
        if (vinculo is null) return false;
        vinculo.Status = "REVOGADO";
        _repo.Salvar(vinculo);
        return true;
    }

    public IEnumerable<PetEntity> ListarPetsVeterinario(decimal veterinarioId) => _repo.ListarPetsVeterinario(veterinarioId);
}
