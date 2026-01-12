using DTOs;
using Enums;

namespace Interfaces;

public interface ICaixaService
{
    void InitializeCollection(string connectionString,
        string databaseName,
        string collectionName);
    Task<Pagamento?> GetById(string id, CancellationToken cancellationToken);
    Task<IEnumerable<Pagamento>?> GetAllTodayPaidsCompletes(DateTime dataConsulta, CancellationToken cancellationToken);
    Task<IEnumerable<Pagamento>?> GetAllTodayPaidsPending(DateTime dataConsulta, CancellationToken cancellationToken);
    Task<IEnumerable<Pagamento>?> GetAllTodayPaidsCanceled(DateTime dataConsulta, CancellationToken cancellationToken);
    Task<Pagamento?> GetByCliente(string cpf, CancellationToken cancellationToken);
    Task<Pagamento?> Create(Pagamento agendamento, CancellationToken cancellationToken);
    Task<Pagamento?> UpdateStatusWebhook(long id, PaidStatus status, CancellationToken cancellationToken);
    Task<Pagamento?> UpdateStatus(string id, PaidStatus status, CancellationToken cancellationToken);
    Task<Pagamento?> GetPaymentId(long id, CancellationToken cancellationToken);
    Task<bool> Delete(string id, CancellationToken cancellationToken);
}