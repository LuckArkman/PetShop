using PetShop.Application.DTOs;

namespace PetShop.Application.Interfaces;

public interface IResponsavelService
{
    Task<List<Responsavel>?> GetAllResponsavel(CancellationToken cancellationToken);
    Task<object?> GetObject(string _object, CancellationToken cancellationToken);
    Task<object?> GetResponsavelId(string _object, CancellationToken cancellationToken);
    Task<object?> InsetObject(Responsavel _object, CancellationToken cancellationToken);

    Task<object?> UpdateObject(Responsavel _object, CancellationToken cancellationToken);

    Task<bool> RemoveAsync(string _object, CancellationToken cancellationToken);
    Task<object?> FindByEmailAsync(string modelCredencial, CancellationToken cancellationToken);
}