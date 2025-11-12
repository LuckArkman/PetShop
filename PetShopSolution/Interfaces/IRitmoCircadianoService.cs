using DTOs;

namespace Interfaces;

public interface IRitmoCircadianoService
{
    Task<object?> GetObject(string _object, CancellationToken cancellationToken);

    Task<object?> InsetObject(RitmoCircadiano _object, CancellationToken cancellationToken);

    Task<object?> UpdateObject(RitmoCircadiano _object, CancellationToken cancellationToken);

    Task RemoveObject(object _object, CancellationToken cancellationToken);
}