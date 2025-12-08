using DTOs;

namespace Interfaces;

public interface IResponsavelService
{
    Task<List<Responsavel>?> GetAllResponsavel(CancellationToken cancellationToken);
    Task<Responsavel?> GetObject(string mail, CancellationToken cancellationToken);
    Task<Responsavel?> GetResponsavelId(string _rg, CancellationToken cancellationToken);
    Task<Responsavel?> InsetObject(Responsavel _object, CancellationToken cancellationToken);

    Task<Responsavel?> UpdateObject(Responsavel _object, CancellationToken cancellationToken);

    Task<bool> RemoveAsync(string _object, CancellationToken cancellationToken);
    Task<Responsavel?> FindByEmailAsync(string modelCredencial, CancellationToken cancellationToken);
    Task<List<Responsavel>?> GetAllResponsaveis(ICollection<string> resResponsaveis, CancellationToken none);
}