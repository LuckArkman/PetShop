using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace DTOs;

public class GeolocationRecord
{
    public string Id { get; set; }
    public string AnimalId { get; set; }

    /// <summary>
    /// Latitude da localização atual do animal
    /// </summary>
    public double Latitude { get; set; }

    /// <summary>
    /// Longitude da localização atual do animal
    /// </summary>
    public double Longitude { get; set; }

    /// <summary>
    /// Data e hora do registro da localização
    /// </summary>
    public DateTime DataRegistro { get; set; } = DateTime.UtcNow;

    /// <summary>
    /// Endereço aproximado (opcional, se for feita geocodificação)
    /// </summary>
    public string? Endereco { get; set; }

    /// <summary>
    /// Observações adicionais (ex: local do resgate, visto pela última vez, etc.)
    /// </summary>
    public string? Observacoes { get; set; }

    public GeolocationRecord()
    {
        Id =  Guid.NewGuid().ToString();
    }
}