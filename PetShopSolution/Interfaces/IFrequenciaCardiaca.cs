using DTOs;

namespace Interfaces;

public interface IFrequenciaCardiaca
{
    void InitializeCollection(string connectionString,
        string databaseName,
        string collectionName);
    Task<FrequenciaCardiaca?> GetObject(string _object, CancellationToken cancellationToken);

    Task<FrequenciaCardiaca?> InsetObject(FrequenciaCardiaca _object, CancellationToken cancellationToken);

    Task<FrequenciaCardiaca?> UpdateObject(FrequenciaCardiaca _object, CancellationToken cancellationToken);

    Task RemoveObject(object _object, CancellationToken cancellationToken);
}