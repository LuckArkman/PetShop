using DTOs;
using Enums;

namespace Interfaces;

public interface ICaixaService
{
    Task<Pagamento?> GetById(string id, CancellationToken cancellationToken);
    Task<IEnumerable<Pagamento>?> GetAllTodayPaidsCompletes(DateTime dataConsulta, CancellationToken cancellationToken);
    Task<IEnumerable<Pagamento>?> GetAllTodayPaidsPending(DateTime dataConsulta, CancellationToken cancellationToken);
    Task<IEnumerable<Pagamento>?> GetAllTodayPaidsCanceled(DateTime dataConsulta, CancellationToken cancellationToken);
    Task<Pagamento?> GetByCliente(string cpf, CancellationToken cancellationToken);
    Task<Pagamento?> Create(Pagamento agendamento, CancellationToken cancellationToken);
    Task<Pagamento?> UpdateStatus(string id, PaidStatus status, CancellationToken cancellationToken);
    Task<bool> DeleteByDateTime(DateTime dataHora, CancellationToken cancellationToken);
    Task<bool> Delete(string id, CancellationToken cancellationToken);
}