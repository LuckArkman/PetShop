using DTOs;

namespace Interfaces;

public interface IHistoryVacinacaoService
{
    void InitializeCollection(string? connectionString,
        string? databaseName,
        string? collectionName);
    Task<HistoryVacinacao?> GetObject(string _object, CancellationToken cancellationToken);

    Task<HistoryVacinacao?> InsetObject(HistoryVacinacao _object, CancellationToken cancellationToken);

    Task<HistoryVacinacao?> UpdateObject(HistoryVacinacao _object, CancellationToken cancellationToken);

    Task RemoveObject(object _object, CancellationToken cancellationToken);
    Task<HistoryVacinacao?> GetHistoricoAnimal(string animalId, CancellationToken cancellationToken);
}