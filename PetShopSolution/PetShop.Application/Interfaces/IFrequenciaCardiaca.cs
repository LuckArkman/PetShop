using PetShop.Application.DTOs;

namespace PetShop.Application.Interfaces;

public interface IFrequenciaCardiaca
{
    Task<object?> GetObject(string _object, CancellationToken cancellationToken);

    Task<object?> InsetObject(FrequenciaCardiaca _object, CancellationToken cancellationToken);

    Task<object?> UpdateObject(FrequenciaCardiaca _object, CancellationToken cancellationToken);

    Task RemoveObject(object _object, CancellationToken cancellationToken);
}