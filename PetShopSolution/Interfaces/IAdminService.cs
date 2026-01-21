using DTOs;

namespace Interfaces;

public interface IAdminService
{
    void InitializeCollection(string? connectionString,
        string? databaseName,
        string? collectionName);
    Task<RegisterViewModel?> GetObject(string mail, CancellationToken cancellationToken);
    Task<RegisterViewModel?> InsetObject(RegisterViewModel _object, CancellationToken cancellationToken);
    Task<RegisterViewModel?> UpdateObject(RegisterViewModel _object, CancellationToken cancellationToken);
    Task<bool> RemoveAsync(string _object, CancellationToken cancellationToken);
    Task<RegisterViewModel?> FindByEmailAsync(string modelCredencial, CancellationToken cancellationToken);
}