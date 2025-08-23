using PetShop.Application.DTOs;
using PetShop.Application.Interfaces;

namespace PetShop.Application.Services;

public class MedicoVeterinarioService : IMedicoVeterinarioService
{
    public Task<object?> GetObject(string _object, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }

    public Task<object?> InsetObject(MedicoVeterinario _object, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }

    public Task<object?> UpdateObject(MedicoVeterinario _object, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }

    public Task RemoveObject(object _object, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }
}