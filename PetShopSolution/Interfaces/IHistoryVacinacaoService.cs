using DTOs;

namespace Interfaces;

public interface IHistoryVacinacaoService
{
    Task<HistoryVacinacao?> GetObject(string _object, CancellationToken cancellationToken);

    Task<HistoryVacinacao?> InsetObject(HistoryVacinacao _object, CancellationToken cancellationToken);

    Task<HistoryVacinacao?> UpdateObject(HistoryVacinacao _object, CancellationToken cancellationToken);

    Task RemoveObject(object _object, CancellationToken cancellationToken);
    Task<HistoryVacinacao> GetHistoricoAnimal(string animalId, CancellationToken none);
}