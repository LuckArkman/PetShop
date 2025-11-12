using PetShop.Application.DTOs;

namespace PetShop.Application.Interfaces;

public interface IHistoryVacinacaoService
{
    Task<object?> GetObject(string _object, CancellationToken cancellationToken);

    Task<object?> InsetObject(HistoryVacinacao _object, CancellationToken cancellationToken);

    Task<object?> UpdateObject(HistoryVacinacao _object, CancellationToken cancellationToken);

    Task RemoveObject(object _object, CancellationToken cancellationToken);
    Task<HistoryVacinacao> GetHistoricoAnimal(string animalId, CancellationToken none);
}