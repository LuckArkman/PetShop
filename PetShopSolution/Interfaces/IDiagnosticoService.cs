using DTOs;

namespace Interfaces;

public interface IDiagnosticoService
{
    void InitializeCollection(string? connectionString,
        string? databaseName,
        string? collectionName);
    Task<Diagnostico?> GetDiagnostico(string _object, CancellationToken cancellationToken);
    Task<Diagnostico?> InsetDiagnostico(Diagnostico _object, CancellationToken cancellationToken);
    Task<Diagnostico?> UpdateDiagnostico(Diagnostico _object, CancellationToken cancellationToken);
    Task<bool?> RemoveDiagnostico(string Id, CancellationToken cancellationToken);
    Task<List<Diagnostico>?> GetAllRelatorios(string animalId, CancellationToken none);
}