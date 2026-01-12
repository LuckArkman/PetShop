using DTOs;

namespace Interfaces;

public interface IRelatorioClinicoService
{
    void InitializeCollection(string connectionString,
        string databaseName,
        string collectionName);
    Task<Relatorio?> GetRelatorio(string _object, CancellationToken cancellationToken);

    Task<Relatorio?> InsetRelatorio(Relatorio _object, CancellationToken cancellationToken);

    Task<Relatorio?> UpdateRelatorio(Relatorio _object, CancellationToken cancellationToken);

    Task<bool?> RemoveRelatorio(Relatorio _object, CancellationToken cancellationToken);
    Task<List<Relatorio>?> GetAllRelatorios(string animalId, CancellationToken none);
    Task<List<Relatorio>?> GetAllVeterinarioRelatorios(string id, CancellationToken none);
}