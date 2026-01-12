using DTOs;

namespace Interfaces;

public interface IAtendenteService
{
    void InitializeCollection(string connectionString,
        string databaseName,
        string collectionName);
    Task<List<Atendente>?> GetAllAtendente(CancellationToken cancellationToken);
    Task<Atendente?> GetObject(string mail, CancellationToken cancellationToken);
    Task<Atendente?> GetAtendenteRG(string _rg, CancellationToken cancellationToken);
    Task<Atendente?> InsetObject(Atendente _object, CancellationToken cancellationToken);
    Task<Atendente?> UpdateObject(Atendente _object, CancellationToken cancellationToken);
    Task<bool> RemoveAsync(string _object, CancellationToken cancellationToken);
    Task<Atendente?> FindByEmailAsync(string modelCredencial, CancellationToken cancellationToken);
}