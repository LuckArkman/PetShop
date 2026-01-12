using DTOs;

namespace Interfaces;

public interface IPaymentGateway
{
    Task<PaymentResponse> CreatePaymentAsync(Order order, Responsavel _responsavel, string paymentMethod);
    Task<PaymentResponse?> GetPaymentAsync(string transactionId);
}