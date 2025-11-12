using PetShop.Application.DTOs;

namespace PetShop.Application.Interfaces;

public interface IHistoryFrequenciaCardiaca
{
    Task<object?> GetObject(string _object, CancellationToken cancellationToken);

    Task<object?> InsetObject(HistoryFrequenciaCardiaca _object, CancellationToken cancellationToken);

    Task<object?> UpdateObject(HistoryFrequenciaCardiaca _object, CancellationToken cancellationToken);

    Task RemoveObject(object _object, CancellationToken cancellationToken);
}