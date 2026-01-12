using DTOs;

namespace Interfaces;

public interface IGeolocationRecordService
{
    void InitializeCollection(string connectionString,
        string databaseName,
        string collectionName);
    Task<GeolocationRecord?> GetObject(string _object, CancellationToken cancellationToken);

    Task<GeolocationRecord?> InsetObject(GeolocationRecord _object, CancellationToken cancellationToken);

    Task<GeolocationRecord?> UpdateObject(GeolocationRecord _object, CancellationToken cancellationToken);

    Task RemoveObject(object _object, CancellationToken cancellationToken);
}