using PetShop.Application.DTOs;
using PetShop.Application.Enums;

namespace PetShop.Application.Interfaces;

public interface IAgendamentoService
{
    Task<Agendamento?> GetById(string id, CancellationToken cancellationToken);
    Task<IEnumerable<Agendamento>> GetByCliente(string clienteId, CancellationToken cancellationToken);
    Task<Agendamento> Create(Agendamento agendamento, CancellationToken cancellationToken);
    Task<Agendamento?> UpdateStatus(string id, Status status, CancellationToken cancellationToken);
    Task<bool> DeleteByDateTime(DateTime dataHora, CancellationToken cancellationToken);
    Task<bool> Delete(string id, CancellationToken cancellationToken);
}