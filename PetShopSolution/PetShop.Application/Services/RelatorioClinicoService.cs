using PetShop.Application.DTOs;
using PetShop.Application.Interfaces;

namespace PetShop.Application.Services;

public class RelatorioClinicoService : IRelatorioClinicoService
{
    public Task<object?> GetObject(string _object, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }

    public Task<object?> InsetObject(RelatorioClinico _object, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }

    public Task<object?> UpdateObject(RelatorioClinico _object, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }

    public Task RemoveObject(object _object, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }
}