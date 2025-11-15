using DTOs;

namespace Interfaces;

public interface IAnimalService
{
    Task<List<Animal>?> GetAllAnimals(CancellationToken cancellationToken);
    Task<Animal?> GetObject(string _object, CancellationToken cancellationToken);

    Task<Animal?> InsetObject(Animal _object, CancellationToken cancellationToken);

    Task<Animal?> UpdateObject(Animal _object, CancellationToken cancellationToken);

    Task<bool> RemoveObject(string _object, CancellationToken cancellationToken);
    Task<List<Animal>?> GetAnimalsInList(ICollection<string> animals, CancellationToken none);
}