using PetShop.Application.DTOs;
using PetShop.Application.Interfaces;

namespace PetShop.Application.Services;

public class HistoricoClinicoService : IHistoricoClinicoService
{
    public Task<object?> GetObject(string _object, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }

    public Task<object?> InsetObject(HistoricoClinico _object, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }

    public Task<object?> UpdateObject(HistoricoClinico _object, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }

    public Task RemoveObject(object _object, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }
}