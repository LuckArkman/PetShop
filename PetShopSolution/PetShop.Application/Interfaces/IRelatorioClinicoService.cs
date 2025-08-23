using PetShop.Application.DTOs;

namespace PetShop.Application.Interfaces;

public interface IRelatorioClinicoService
{
    Task<object?> GetObject(string _object, CancellationToken cancellationToken);

    Task<object?> InsetObject(RelatorioClinico _object, CancellationToken cancellationToken);

    Task<object?> UpdateObject(RelatorioClinico _object, CancellationToken cancellationToken);

    Task RemoveObject(object _object, CancellationToken cancellationToken);
}