using Data;
using DTOs;
using Interfaces;
using Microsoft.Extensions.Configuration;
using MongoDB.Driver;

namespace Services;

public class Repositorio : IRepositorio<Order>
{
    private readonly IConfiguration _configuration;
    protected IMongoCollection<Order> _collection;
    private IRepositorio<Order> _repositorioImplementation;
    public string _collectionName { get; set; }
    private MongoDataController _db { get; set; }
    private IMongoDatabase _mongoDatabase { get; set; }
    
    public void InitializeCollection(string connectionString,
        string databaseName,
        string collectionName)
    {
        _collectionName = collectionName;
        // Verifica se a conexão já foi estabelecida
        if (_collection != null) return;
        
        _db = new MongoDataController(connectionString, databaseName, _collectionName);
        _mongoDatabase = _db.GetDatabase();
        _collection = _mongoDatabase.GetCollection<Order>(_collectionName);
    }

    public async Task<Order?> InsertOneAsync(Order document)
    {
        await _collection.InsertOneAsync(document);
        return document;
    }

    public async Task<Order?> Update(string order, string status)
    {
        var filter = Builders<Order>.Filter.Eq(p => p.id, order);
        var update = Builders<Order>.Update
            .Set(a => a.Status, status)
            .Set(a => a.updateAt, DateTime.UtcNow);
        
        var result = await _collection.UpdateOneAsync(filter, update, cancellationToken: CancellationToken.None);
        return await GetByIdOrderAsync(order);
    }

    Task<Order?> IRepositorio<Order>.GetByIdOrderAsync(string id)
    {
        return GetByIdOrderAsync(id);
    }

    public async Task<Order?> GetByTransectionOrderAsync(string paymentId)
    {
        var filter = Builders<Order>.Filter.Eq(p => p.TransactionId, paymentId);
        return await _collection.Find(filter).FirstOrDefaultAsync();
    }

    public async Task<IEnumerable<Order>?> GetAllTodayPaidsCompletes(DateTime dataConsulta, CancellationToken cancellationToken)
    {
        var inicioDia = dataConsulta.Date;
        var fimDia = inicioDia.AddDays(1);

        var filter = Builders<Order>.Filter.And(
            Builders<Order>.Filter.Eq(o => o.Status, "Paid"),
            Builders<Order>.Filter.Gte(o => o.CreatedAt, inicioDia),
            Builders<Order>.Filter.Lt(o => o.CreatedAt, fimDia)
        );

        return await _collection.Find(filter).ToListAsync(cancellationToken);

    }

    public async Task<IEnumerable<Order>?> GetAllTodayPaidsPending(DateTime dataConsulta, CancellationToken cancellationToken)
    {
        var inicioDia = dataConsulta.Date;
        var fimDia = inicioDia.AddDays(1);

        var filter = Builders<Order>.Filter.And(
            Builders<Order>.Filter.Eq(o => o.Status, "Pending"),
            Builders<Order>.Filter.Gte(o => o.CreatedAt, inicioDia),
            Builders<Order>.Filter.Lt(o => o.CreatedAt, fimDia)
        );

        return await _collection.Find(filter).ToListAsync(cancellationToken);
    }

    public async Task<IEnumerable<Order>?> GetAllTodayPaidsCanceled(DateTime dataConsulta, CancellationToken cancellationToken)
    {
        var inicioDia = dataConsulta.Date;
        var fimDia = inicioDia.AddDays(1);

        var filter = Builders<Order>.Filter.And(
            Builders<Order>.Filter.Eq(o => o.Status, "Canceled"),
            Builders<Order>.Filter.Gte(o => o.CreatedAt, inicioDia),
            Builders<Order>.Filter.Lt(o => o.CreatedAt, fimDia)
        );

        return await _collection.Find(filter).ToListAsync(cancellationToken);
    }

    public Task<bool> Delete(string id, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }

    public async Task<Order?> GetByUserIdOrderAsync(string responsavelId)
    {
        var filter = Builders<Order>.Filter.Eq(p => p.UserId, responsavelId);
        return await _collection.Find(filter).FirstOrDefaultAsync();
    }

    private async Task<Order?> GetByIdOrderAsync(string id)
    {
        var filter = Builders<Order>.Filter.Eq(p => p.id, id);
        return await _collection.Find(filter).FirstOrDefaultAsync();
    }
}