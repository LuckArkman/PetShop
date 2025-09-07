using PetShop.Application.DTOs;

namespace PetShop.Application.Interfaces;

public interface IMedicoVeterinarioService
{
    Task<List<MedicoVeterinario>?> GetAllMedicoVeterinario(CancellationToken cancellationToken);
    Task<object?> GetObject(string _object, CancellationToken cancellationToken);

    Task<object?> InsetObject(MedicoVeterinario _object, CancellationToken cancellationToken);

    Task<object?> UpdateObject(MedicoVeterinario _object, CancellationToken cancellationToken);

    Task<bool> RemoveAsync(object _object, CancellationToken cancellationToken);
    Task<object?> FindByCRMVAsync(string modelCredencial, CancellationToken cancellationToken);
}