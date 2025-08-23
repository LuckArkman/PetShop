using PetShop.Application.DTOs;
using PetShop.Application.Interfaces;

namespace PetShop.Application.Services;

public class QrCodeRegistroService : IQrCodeRegistroService
{
    public Task<object?> GetObject(string _object, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }

    public Task<object?> InsetObject(QrCodeRegistro _object, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }

    public Task<object?> UpdateObject(QrCodeRegistro _object, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }

    public Task RemoveObject(object _object, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }
}