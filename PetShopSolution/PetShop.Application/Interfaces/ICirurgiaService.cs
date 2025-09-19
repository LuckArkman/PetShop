using PetShop.Application.DTOs;

namespace PetShop.Application.Interfaces;

public interface ICirurgiaService
{
    Task<object?> GetCirurgia(string _object, CancellationToken cancellationToken);
    Task<object?> InsetCirurgia(Cirurgia _object, CancellationToken cancellationToken);
    Task<object?> UpdateCirurgia(Cirurgia _object, CancellationToken cancellationToken);
    Task<bool?> RemoveCirurgia(string Id, CancellationToken cancellationToken);
    Task<List<Cirurgia>?> GetAllCirurgias(string animalId, CancellationToken none);    
}