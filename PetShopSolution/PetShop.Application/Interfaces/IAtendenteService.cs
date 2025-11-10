using PetShop.Application.DTOs;

namespace PetShop.Application.Interfaces;

public interface IAtendenteService
{
    Task<List<Atendente>?> GetAllAtendente(CancellationToken cancellationToken);
    Task<object?> GetObject(string mail, CancellationToken cancellationToken);
    Task<object?> GetAtendenteRG(string _rg, CancellationToken cancellationToken);
    Task<object?> InsetObject(Atendente _object, CancellationToken cancellationToken);

    Task<object?> UpdateObject(Atendente _object, CancellationToken cancellationToken);

    Task<bool> RemoveAsync(string _object, CancellationToken cancellationToken);
    Task<object?> FindByEmailAsync(string modelCredencial, CancellationToken cancellationToken);
}