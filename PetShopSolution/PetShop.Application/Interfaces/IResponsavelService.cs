using PetShop.Application.DTOs;

namespace PetShop.Application.Interfaces;

public interface IResponsavelService
{
    Task<object?> GetObject(string _object, CancellationToken cancellationToken);

    Task<object?> InsetObject(Responsavel _object, CancellationToken cancellationToken);

    Task<object?> UpdateObject(Responsavel _object, CancellationToken cancellationToken);

    Task RemoveObject(object _object, CancellationToken cancellationToken);
}