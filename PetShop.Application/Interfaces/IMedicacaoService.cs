using PetShop.Application.DTOs;

namespace PetShop.Application.Interfaces;

public interface IMedicacaoService
{
    Task<List<Medicacao>?> GetAllMedicacoes(string _object,CancellationToken cancellationToken);
    Task<object?> GetMedicacao(string _object, CancellationToken cancellationToken);

    Task<object?> InsetMedicacao(Medicacao _object, CancellationToken cancellationToken);

    Task<object?> UpdateMedicacao(Medicacao _object, CancellationToken cancellationToken);

    Task<bool?> RemoveMedicacao(Medicacao _object, CancellationToken cancellationToken);
}