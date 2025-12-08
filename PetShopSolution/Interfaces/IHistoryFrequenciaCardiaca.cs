using DTOs;

namespace Interfaces;

public interface IHistoryFrequenciaCardiaca
{
    Task<HistoryFrequenciaCardiaca?> GetObject(string _object, CancellationToken cancellationToken);

    Task<HistoryFrequenciaCardiaca?> InsetObject(HistoryFrequenciaCardiaca _object, CancellationToken cancellationToken);

    Task<HistoryFrequenciaCardiaca?> UpdateObject(HistoryFrequenciaCardiaca _object, CancellationToken cancellationToken);

    Task RemoveObject(object _object, CancellationToken cancellationToken);
}