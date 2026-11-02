using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System.Text.Json.Serialization;

namespace Amandaba.API.Domain.Entities;
public class PesoHistoricoDocumento
{
    [BsonId]
    [BsonRepresentation(BsonType.String)]
    [JsonPropertyName("_id")]
    public string Id { get; set; } = string.Empty;

    [BsonElement("petId")]
    public decimal PetId { get; set; }

    [BsonElement("atualizadoEm")]
    public DateTime AtualizadoEm { get; set; }

    [BsonElement("historico")]
    public List<MedicaoPesoDocumento> Historico { get; set; } = new();
}
public class MedicaoPesoDocumento
{
    [BsonElement("pesoId")]
    public decimal PesoId { get; set; }

    [BsonElement("peso")]
    public decimal Peso { get; set; }

    [BsonElement("dataMedicao")]
    public DateTime DataMedicao { get; set; }
}
