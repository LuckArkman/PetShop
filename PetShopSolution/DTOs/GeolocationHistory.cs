using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace DTOs;

public class AnimalGeolocationHistory
{
    public string Id { get; set; } = Guid.NewGuid().ToString();
    public string AnimalId { get; set; }

    // Lista de registros de localização
    public List<GeolocationRecord> Locations { get; set; }

    public AnimalGeolocationHistory(string animalId, GeolocationRecord locations)
    {
        AnimalId = animalId;
        Locations = new List<GeolocationRecord>() { locations };
    }
}
