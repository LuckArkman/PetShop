using DTOs;

namespace Interfaces;

public interface IAnimalGeolocationHistoryService
{
    Task<AnimalGeolocationHistory?> GetObject(string _object, CancellationToken cancellationToken);

    Task<AnimalGeolocationHistory?> InsetObject(AnimalGeolocationHistory _object, CancellationToken cancellationToken);

    Task<AnimalGeolocationHistory?> UpdateObject(AnimalGeolocationHistory _object, CancellationToken cancellationToken);

    Task RemoveObject(AnimalGeolocationHistory _object, CancellationToken cancellationToken);
}