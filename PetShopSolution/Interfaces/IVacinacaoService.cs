using DTOs;

namespace Interfaces;

public interface IVacinacaoService
{
    void InitializeCollection(string? connectionString,
        string? databaseName,
        string? collectionName);
    Task<object?> GetObject(string _object, CancellationToken cancellationToken);

    Task<object?> InsetObject(Vacinacao _object, CancellationToken cancellationToken);

    Task<object?> UpdateObject(Vacinacao _object, CancellationToken cancellationToken);

    Task<bool> RemoveObject(Vacinacao _object, CancellationToken cancellationToken);
}