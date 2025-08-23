using PetShop.Application.DTOs;
using PetShop.Application.Interfaces;

namespace PetShop.Application.Services;

public class HistoryVacinacaoService : IHistoryVacinacaoService
{
    public Task<object?> GetObject(string _object, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }

    public Task<object?> InsetObject(HistoryVacinacao _object, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }

    public Task<object?> UpdateObject(HistoryVacinacao _object, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }

    public Task RemoveObject(object _object, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }
}