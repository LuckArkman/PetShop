using PetShop.Application.DTOs;

namespace PetShop.Application.Interfaces;

public interface IRelatorioClinicoService
{
    Task<object?> GetRelatorio(string _object, CancellationToken cancellationToken);

    Task<object?> InsetRelatorio(Relatorio _object, CancellationToken cancellationToken);

    Task<object?> UpdateRelatorio(Relatorio _object, CancellationToken cancellationToken);

    Task<bool?> RemoveRelatorio(Relatorio _object, CancellationToken cancellationToken);
    Task<List<Relatorio>?> GetAllRelatorios(string animalId, CancellationToken none);
}