using DTOs;

namespace Interfaces;

public interface IAnimalService
{
    void InitializeCollection(string connectionString,
        string databaseName,
        string collectionName);
    Task<List<Animal>?> GetAllAnimals(CancellationToken cancellationToken);
    Task<Animal?> GetObject(string _object, CancellationToken cancellationToken);

    Task<Animal?> InsetObject(Animal _object, CancellationToken cancellationToken);

    Task<Animal?> UpdateObject(Animal _object, CancellationToken cancellationToken);

    Task<bool> RemoveObject(string _object, CancellationToken cancellationToken);
    Task<List<Animal>?> GetAnimalsInList(List<string> animals, CancellationToken none);
}