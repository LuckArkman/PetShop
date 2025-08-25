using System.Drawing;
using System.Drawing.Imaging;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using QRCoder;
using System.IO;

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
    public string? QrCodeBase64 { get; set; } = string.Empty;

    /// <summary>
    /// Data em que o QR Code foi gerado
    /// </summary>
    [BsonDateTimeOptions(Kind = DateTimeKind.Utc)]
    public DateTime? DataGeracao { get; set; } = DateTime.UtcNow;

    public QrCodeRegistro()
    {
        DataGeracao =  DateTime.UtcNow;
        QrCodeBase64 = GenerateQrCodeAsBase64String(AnimalId);
    }
    
    public string GenerateQrCodeAsBase64String(string url, int pixelsPerModule = 20,
        QRCodeGenerator.ECCLevel eccLevel = QRCodeGenerator.ECCLevel.Q)
    {
        if (string.IsNullOrWhiteSpace(url))
            throw new ArgumentException("URL cannot be null or empty.", nameof(url));

        try
        {
            using var qrGenerator = new QRCodeGenerator();
            using var qrCodeData = qrGenerator.CreateQrCode(url, eccLevel);
            var qrCode = new PngByteQRCode(qrCodeData);
            byte[] qrCodeBytes = qrCode.GetGraphic(pixelsPerModule);

            return Convert.ToBase64String(qrCodeBytes);
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Erro ao gerar QR Code: {ex.Message}");
            return null;
        }
    }
}