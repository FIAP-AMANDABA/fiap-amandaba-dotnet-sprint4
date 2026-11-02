using Amandaba.API.Application.Dtos.Pesos;
using Amandaba.Application.Dtos.Pesos;
using Amandaba.API.Application.Interfaces;
using Amandaba.Application.Interfaces;

namespace Amandaba.API.Application.UseCases;

public class PesoHistoricoUseCase : IPesoHistoricoUseCase
{
    private readonly IPesoUseCase _pesos;
    private readonly IPesoMongoRepository _mongo;
    public PesoHistoricoUseCase(IPesoUseCase pesos, IPesoMongoRepository mongo)
    { _pesos = pesos; _mongo = mongo; }

    public IEnumerable<PesoResponseDto> Exportar(decimal petId) => _pesos.ObterPorPet(petId);

    public int Sincronizar(decimal petId)
    {
        var lista = _pesos.ObterPorPet(petId).ToList();
        var doc = new PesoHistoricoDto
        {
            Id = $"pet-{petId}", PetId = petId, AtualizadoEm = DateTime.UtcNow,
            Historico = lista.Select(p => new MedicaoPesoDto
            { PesoId = p.IdHistoricoPeso, Peso = p.Peso, DataMedicao = p.DataMedicao }).ToList()
        };
        _mongo.Salvar(doc);
        return doc.Historico.Count;
    }

    public PesoHistoricoDto? ConsultarMongo(decimal petId) => _mongo.ObterPorPet(petId);
}
