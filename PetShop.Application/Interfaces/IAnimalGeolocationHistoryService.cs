using PetShop.Application.DTOs;

namespace PetShop.Application.Interfaces;

public interface IAnimalGeolocationHistoryService
{
    Task<object?> GetObject(string _object, CancellationToken cancellationToken);

    Task<object?> InsetObject(AnimalGeolocationHistory _object, CancellationToken cancellationToken);

    Task<object?> UpdateObject(AnimalGeolocationHistory _object, CancellationToken cancellationToken);

    Task RemoveObject(object _object, CancellationToken cancellationToken);
}