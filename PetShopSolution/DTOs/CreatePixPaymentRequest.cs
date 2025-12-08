namespace DTOs;

public record CreatePixPaymentRequest(
    decimal Amount,
    string Description,
    string ExternalReference,
    string mail
);