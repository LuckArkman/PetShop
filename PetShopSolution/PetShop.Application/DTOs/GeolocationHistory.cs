using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace PetShop.Application.DTOs;

public class AnimalGeolocationHistory
{
    [BsonId]
    [BsonRepresentation(BsonType.ObjectId)]
    public string Id { get; set; } = string.Empty;

    // Referência ao animal
    [BsonRepresentation(BsonType.ObjectId)]
    public string AnimalId { get; set; } = string.Empty;

    // Lista de registros de localização
    public List<GeolocationRecord> Locations { get; set; } = new List<GeolocationRecord>();
}
