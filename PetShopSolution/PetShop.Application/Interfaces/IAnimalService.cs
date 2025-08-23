using PetShop.Application.DTOs;

namespace PetShop.Application.Interfaces;

public interface IAnimalService
{
    Task<object?> GetObject(string _object, CancellationToken cancellationToken);

    Task<object?> InsetObject(Animal _object, CancellationToken cancellationToken);

    Task<object?> UpdateObject(Animal _object, CancellationToken cancellationToken);

    Task RemoveObject(object _object, CancellationToken cancellationToken);
}