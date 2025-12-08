using DTOs;

namespace Interfaces;

public interface IGeolocationRecordService
{
    Task<GeolocationRecord?> GetObject(string _object, CancellationToken cancellationToken);

    Task<GeolocationRecord?> InsetObject(GeolocationRecord _object, CancellationToken cancellationToken);

    Task<GeolocationRecord?> UpdateObject(GeolocationRecord _object, CancellationToken cancellationToken);

    Task RemoveObject(object _object, CancellationToken cancellationToken);
}