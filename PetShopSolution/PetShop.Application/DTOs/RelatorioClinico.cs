using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace PetShop.Application.DTOs;

public class RelatorioClinico
{
    [BsonId]
    [BsonRepresentation(BsonType.ObjectId)]
    public string Id { get; set; } = string.Empty;

    [BsonElement("animalId")]
    [BsonRepresentation(BsonType.ObjectId)]
    public string AnimalId { get; set; } = string.Empty;

    [BsonElement("responsavelId")]
    [BsonRepresentation(BsonType.ObjectId)]
    public string ResponsavelId { get; set; } = string.Empty;

    [BsonElement("dataAtendimento")]
    public DateTime _dataAtendimento { get; set; } = DateTime.UtcNow;

    [BsonElement("sintomas")]
    public string Sintomas { get; set; } = string.Empty;

    [BsonElement("diagnostico")]
    public string Diagnostico { get; set; } = string.Empty;

    [BsonElement("tratamento")]
    public string Tratamento { get; set; } = string.Empty;

    [BsonElement("observacoes")]
    public string? Observacoes { get; set; }

    [BsonElement("veterinarioId")] // Quem atendeu o animal
    [BsonRepresentation(BsonType.ObjectId)]
    public string VeterinarioId { get; set; } = string.Empty;

    public RelatorioClinico()
    {
        Id =  Guid.NewGuid().ToString();
        _dataAtendimento =  DateTime.UtcNow;
    }
}