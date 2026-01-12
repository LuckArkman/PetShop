using System;
using System.Drawing;
using System.Drawing.Imaging;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using QRCoder;
using System.IO;

namespace DTOs;

public class QrCodeRegistro
{
    public string Id { get; set; }
    public string AnimalId { get; set; }

    /// <summary>
    /// QR Code em formato Base64 que permitirá acessar a ficha veterinária do animal
    /// </summary>
    public string? QrCodeBase64 { get; set; }

    /// <summary>
    /// Data em que o QR Code foi gerado
    /// </summary>
    [BsonDateTimeOptions(Kind = DateTimeKind.Utc)]
    public DateTime? DataGeracao { get; set; } = DateTime.UtcNow;

    public QrCodeRegistro()
    {
        
    }

    public QrCodeRegistro(string id, string animalId)
    {
        Id = id;
        AnimalId = animalId;
        QrCodeBase64 = GenerateQrCodeAsBase64String();
    }

    public string GenerateQrCodeAsBase64String()
    {
        Console.WriteLine(AnimalId);
        int pixelsPerModule = 20;
        QRCodeGenerator.ECCLevel eccLevel = QRCodeGenerator.ECCLevel.Q;
        
        if (string.IsNullOrWhiteSpace(AnimalId))
            throw new ArgumentException("AnimalId cannot be null or empty.", nameof(AnimalId));

        try
        {
            using var qrGenerator = new QRCodeGenerator();
            using var qrCodeData = qrGenerator.CreateQrCode(AnimalId, eccLevel);
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