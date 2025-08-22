using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace PetShop.Application.DTOs;

public class QrCodeRegistro
{
    [BsonId]
    [BsonRepresentation(BsonType.ObjectId)]
    public string Id { get; set; } = string.Empty;

    /// <summary>
    /// ID do animal ao qual o QR Code está vinculado
    /// </summary>
    [BsonRepresentation(BsonType.ObjectId)]
    public string AnimalId { get; set; } = string.Empty;

    /// <summary>
    /// QR Code em formato Base64 que permitirá acessar a ficha veterinária do animal
    /// </summary>
    public string QrCodeBase64 { get; set; } = string.Empty;

    /// <summary>
    /// Data em que o QR Code foi gerado
    /// </summary>
    [BsonDateTimeOptions(Kind = DateTimeKind.Utc)]
    public DateTime DataGeracao { get; set; } = DateTime.UtcNow;
}