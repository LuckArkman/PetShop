using PetShop.Application.DTOs;

namespace PetShop.Application.Interfaces;

public interface IVacinacaoService
{
    Task<object?> GetObject(string _object, CancellationToken cancellationToken);

    Task<object?> InsetObject(Vacinacao _object, CancellationToken cancellationToken);

    Task<object?> UpdateObject(Vacinacao _object, CancellationToken cancellationToken);

    Task RemoveObject(object _object, CancellationToken cancellationToken);
}