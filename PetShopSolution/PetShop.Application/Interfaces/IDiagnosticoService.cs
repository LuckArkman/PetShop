using PetShop.Application.DTOs;

namespace PetShop.Application.Interfaces;

public interface IDiagnosticoService
{
    Task<object?> GetDiagnostico(string _object, CancellationToken cancellationToken);
    Task<object?> InsetDiagnostico(Diagnostico _object, CancellationToken cancellationToken);
    Task<object?> UpdateDiagnostico(Diagnostico _object, CancellationToken cancellationToken);
    Task<bool?> RemoveDiagnostico(string Id, CancellationToken cancellationToken);
    Task<List<Diagnostico>?> GetAllRelatorios(string animalId, CancellationToken none);
}