using DTOs;

namespace Interfaces;

public interface ICirurgiaService
{
    void InitializeCollection(string? connectionString,
        string? databaseName,
        string? collectionName);
    Task<Cirurgia?> GetCirurgia(string _object, CancellationToken cancellationToken);
    Task<Cirurgia?> InsetCirurgia(Cirurgia _object, CancellationToken cancellationToken);
    Task<Cirurgia?> UpdateCirurgia(Cirurgia _object, CancellationToken cancellationToken);
    Task<bool?> RemoveCirurgia(string Id, CancellationToken cancellationToken);
    Task<List<Cirurgia>?> GetAllCirurgias(string animalId, CancellationToken none);
}