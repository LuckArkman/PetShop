using DTOs;

namespace Interfaces;

public interface IHistoricoClinicoService
{
    Task<HistoricoClinico?> GetObject(string _object, CancellationToken cancellationToken);

    Task<HistoricoClinico?> InsetObject(HistoricoClinico _object, CancellationToken cancellationToken);

    Task<HistoricoClinico?> UpdateObject(HistoricoClinico _object, CancellationToken cancellationToken);

    Task RemoveObject(object _object, CancellationToken cancellationToken);
}