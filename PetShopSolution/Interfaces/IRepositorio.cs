using DTOs;

namespace Interfaces;

public interface IRepositorio<T>
{
    void InitializeCollection(string connectionString,
        string databaseName,
        string collectionName);
    Task<T> InsertOneAsync(Order order);
    Task<T> Update(string order,string status);
    Task<T?> GetByIdOrderAsync(string id);
    Task<T?> GetByTransectionOrderAsync(string paymentId);
}