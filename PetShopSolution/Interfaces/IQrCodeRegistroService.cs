using DTOs;

namespace Interfaces;

public interface IQrCodeRegistroService
{
    Task<QrCodeRegistro?> GetObject(string _object, CancellationToken cancellationToken);

    Task<QrCodeRegistro?> InsetObject(QrCodeRegistro _object, CancellationToken cancellationToken);

    Task<QrCodeRegistro?> UpdateObject(QrCodeRegistro _object, CancellationToken cancellationToken);

    Task RemoveObject(QrCodeRegistro _object, CancellationToken cancellationToken);
}