using DTOs;

namespace Interfaces;

public interface IRepositorio<T>
{
    void InitializeCollection(string? connectionString,
        string? databaseName,
        string? collectionName);
    Task<T> InsertOneAsync(Order order);
    Task<T> Update(string order, string status);
    Task<T?> GetByIdOrderAsync(string id);
    Task<T?> GetByTransectionOrderAsync(string paymentId);
    Task<IEnumerable<Order>?> GetAllTodayPaidsCompletes(DateTime dataConsulta, CancellationToken cancellationToken);
    Task<IEnumerable<Order>?> GetAllTodayPaidsPending(DateTime dataConsulta, CancellationToken cancellationToken);
    Task<IEnumerable<Order>?> GetAllTodayPaidsCanceled(DateTime dataConsulta, CancellationToken cancellationToken);
    Task<bool> Delete(string id, CancellationToken cancellationToken);
    Task<Order?> GetByUserIdOrderAsync(string responsavelId);
}