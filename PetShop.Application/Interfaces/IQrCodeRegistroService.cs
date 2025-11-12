using PetShop.Application.DTOs;

namespace PetShop.Application.Interfaces;

public interface IQrCodeRegistroService
{
    Task<object?> GetObject(string _object, CancellationToken cancellationToken);

    Task<object?> InsetObject(QrCodeRegistro _object, CancellationToken cancellationToken);

    Task<object?> UpdateObject(QrCodeRegistro _object, CancellationToken cancellationToken);

    Task RemoveObject(object _object, CancellationToken cancellationToken);
}