using Data;
using DTOs;
using Interfaces;
using Microsoft.Extensions.Configuration;
using MongoDB.Driver;

namespace Services;

public class HistoryVacinacaoService : IHistoryVacinacaoService
{
    private readonly IConfiguration _configuration;
    protected IMongoCollection<HistoryVacinacao> _collection;
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
        _collection = _mongoDatabase.GetCollection<HistoryVacinacao>(_collectionName);
    }
    public async Task<HistoryVacinacao?> GetObject(string _object, CancellationToken cancellationToken)
    {
        var filter = Builders<HistoryVacinacao>.Filter.Eq(u => u.id, _object);
        var character = _collection.Find(filter).FirstOrDefault();
        return character as HistoryVacinacao;
    }

    public async Task<HistoryVacinacao?> InsetObject(HistoryVacinacao _object, CancellationToken cancellationToken)
    {
        await _collection.InsertOneAsync(_object);
        return _object as HistoryVacinacao;
    }

    public async Task<HistoryVacinacao?> UpdateObject(HistoryVacinacao obj, CancellationToken cancellationToken)
    {
        var filter = Builders<HistoryVacinacao>.Filter.Eq(u => u.id, obj.id);
        var update = Builders<HistoryVacinacao>.Update
            .Set(u => u._animalId, obj._animalId)
            .Set(u => u._Vacinacao, obj._Vacinacao);

        var result = await _collection.UpdateOneAsync(filter, update, cancellationToken: cancellationToken);

        if (result.ModifiedCount > 0)
        {
            return await GetObject(obj.id, cancellationToken);
        }

        return null;
    }


    public Task RemoveObject(object _object, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }

    public async Task<HistoryVacinacao?> GetHistoricoAnimal(string animalId, CancellationToken none)
    {
        var filter = Builders<HistoryVacinacao>.Filter.Eq(u => u._animalId, animalId);
        var character = _collection.Find(filter).FirstOrDefault();
        return character as HistoryVacinacao;
    }
}