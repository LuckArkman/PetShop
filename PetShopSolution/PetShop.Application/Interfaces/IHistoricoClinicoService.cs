using PetShop.Application.DTOs;

namespace PetShop.Application.Interfaces;

public interface IHistoricoClinicoService
{
    Task<object?> GetObject(string _object, CancellationToken cancellationToken);

    Task<object?> InsetObject(HistoricoClinico _object, CancellationToken cancellationToken);

    Task<object?> UpdateObject(HistoricoClinico _object, CancellationToken cancellationToken);

    Task RemoveObject(object _object, CancellationToken cancellationToken);
}