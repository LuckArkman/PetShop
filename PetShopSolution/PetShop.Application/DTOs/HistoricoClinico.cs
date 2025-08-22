using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace PetShop.Application.DTOs;

public class HistoricoClinico
{
    [BsonId]
    [BsonRepresentation(BsonType.ObjectId)]
    public string Id { get; set; } = string.Empty;

    // Relacionamento com o Animal
    [BsonRepresentation(BsonType.ObjectId)]
    public string AnimalId { get; set; } = string.Empty;

    // Lista de relatórios clínicos relacionados ao animal
    public List<RelatorioClinico> Relatorios { get; set; } = new();

    // Data da última atualização no histórico
    public DateTime UltimaAtualizacao { get; set; } = DateTime.UtcNow;
}