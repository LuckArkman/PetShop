using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace PetShop.Application.DTOs;

public class RelatorioClinico
{
    public string Id { get; set; } = Guid.NewGuid().ToString();

    [BsonElement("animalId")]
    public string AnimalId { get; set; }

    [BsonElement("responsavelId")]
    public string ResponsavelId { get; set; }

    [BsonElement("dataAtendimento")]
    public DateTime _dataAtendimento { get; set; } = DateTime.UtcNow;

    [BsonElement("sintomas")]
    public string Sintomas { get; set; }

    [BsonElement("diagnostico")]
    public string Diagnostico { get; set; }

    [BsonElement("tratamento")]
    public string Tratamento { get; set; }

    [BsonElement("observacoes")]
    public string? Observacoes { get; set; }

    [BsonElement("veterinarioId")] // Quem atendeu o animal
    public string VeterinarioId { get; set; }
}