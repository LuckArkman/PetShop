using PetShop.Application.DTOs;

namespace PetShop.Application.Interfaces;

public interface IGeolocationRecordService
{
    Task<object?> GetObject(string _object, CancellationToken cancellationToken);

    Task<object?> InsetObject(GeolocationRecord _object, CancellationToken cancellationToken);

    Task<object?> UpdateObject(GeolocationRecord _object, CancellationToken cancellationToken);

    Task RemoveObject(object _object, CancellationToken cancellationToken);
}