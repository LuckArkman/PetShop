namespace DTOs;

public record CreateCardPaymentRequest(
    decimal Amount,
    string Description,
    string ExternalReference,
    string PayerEmail,
    string CardToken,
    int Installments,
    string PaymentMethodId // ex: "visa"
);