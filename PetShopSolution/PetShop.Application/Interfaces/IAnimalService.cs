using PetShop.Application.DTOs;

namespace PetShop.Application.Interfaces;

public interface IAnimalService
{
    Task<List<Animal>?> GetAllAnimals(CancellationToken cancellationToken);
    Task<object?> GetObject(string _object, CancellationToken cancellationToken);

    Task<object?> InsetObject(Animal _object, CancellationToken cancellationToken);

    Task<object?> UpdateObject(Animal _object, CancellationToken cancellationToken);

    Task RemoveObject(object _object, CancellationToken cancellationToken);
    Task<object?> GetAnimalsInList(ICollection<string> animals, CancellationToken none);
}