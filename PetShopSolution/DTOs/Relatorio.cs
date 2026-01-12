using System;
using MongoDB.Bson.Serialization.Attributes;

namespace DTOs;

public class Relatorio
{
    public string Id { get; set; } = Guid.NewGuid().ToString();
    
    public string animalId { get; set; }

    [BsonElement("dataAtendimento")]
    public DateTime _data { get; set; } = DateTime.UtcNow;

    [BsonElement("sintomas")]
    
    public string Sintomas { get; set; }

    [BsonElement("tratamento")]
    public string Tratamento { get; set; }

    [BsonElement("observacoes")]
    public string? Observacoes { get; set; }

    [BsonElement("veterinarioId")] // Quem atendeu o animal
    public string VeterinarioId { get; set; }
}