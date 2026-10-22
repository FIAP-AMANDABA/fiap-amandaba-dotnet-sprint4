using Amandaba.API.Application.Interfaces;
using Microsoft.Extensions.Configuration;
using MongoDB.Driver;

namespace Amandaba.API.Infrastructure.Mongo;

// MongoDB.Driver: consulta e substitui um documento por pet.
// Se Mongo nao estiver configurado, apenas estes endpoints falham (Oracle segue funcionando).
public class PesoMongoRepository : IPesoMongoRepository
{
    private readonly IMongoCollection<PesoHistoricoDocumento> _colecao;
    public PesoMongoRepository(IConfiguration config)
    {
        var uri = config["MongoDb:ConnectionString"];
        if (string.IsNullOrWhiteSpace(uri))
            throw new InvalidOperationException("Configure MongoDb:ConnectionString para usar este endpoint.");
        var cliente = new MongoClient(uri);
        var banco = cliente.GetDatabase(config["MongoDb:DatabaseName"] ?? "amandaba");
        _colecao = banco.GetCollection<PesoHistoricoDocumento>(config["MongoDb:CollectionName"] ?? "historico_pesos");
    }
    public PesoHistoricoDocumento? ObterPorPet(decimal petId) =>
        _colecao.Find(d => d.PetId == petId).FirstOrDefault();

    public void Salvar(PesoHistoricoDocumento documento) =>
        _colecao.ReplaceOne(d => d.Id == documento.Id, documento, new ReplaceOptions { IsUpsert = true });
}
