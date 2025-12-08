using DTOs;

namespace Interfaces;

public interface IMedicoVeterinarioService
{
    Task<List<MedicoVeterinario>?> GetAllMedicoVeterinario(CancellationToken cancellationToken);
    Task<MedicoVeterinario?> GetObject(string _object, CancellationToken cancellationToken);

    Task<MedicoVeterinario?> InsetObject(MedicoVeterinario _object, CancellationToken cancellationToken);

    Task<MedicoVeterinario?> UpdateObject(MedicoVeterinario _object, CancellationToken cancellationToken);

    Task<bool> RemoveAsync(object _object, CancellationToken cancellationToken);
    Task<MedicoVeterinario?> FindByCRMVAsync(string modelCredencial, CancellationToken cancellationToken);
}