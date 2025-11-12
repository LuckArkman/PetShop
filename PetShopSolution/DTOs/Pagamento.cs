using Enums;

namespace DTOs;

public class Pagamento
{
    public string id { get; set; }
    public string cpf { get; set; }
    public PaidType type { get; set; } = PaidType.money;
    public decimal amount { get; set; }
    public PaidStatus status { get; set; } = PaidStatus.pending;
    public string pixKey { get; set; } = string.Empty;
    public string pixKeyType { get; set; } = string.Empty;
    public string qrCode { get; set; } = string.Empty;
    public string qrCodeImageUrl { get; set; } = string.Empty;
    public DateTime createdAt { get; set; } = DateTime.UtcNow;
    public DateTime? paidAt { get; set; }
}