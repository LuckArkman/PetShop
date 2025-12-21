using Data;
using DTOs;
using Interfaces;
using Microsoft.Extensions.Configuration;
using MongoDB.Driver;

namespace Services;

public class HistoricoClinicoService : IHistoricoClinicoService
{
    private readonly IConfiguration _configuration;
    protected IMongoCollection<HistoricoClinico> _collection;
    private readonly IConfiguration _cfg;
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
        _collection = _mongoDatabase.GetCollection<HistoricoClinico>(_collectionName);
    }
    public async Task<HistoricoClinico?> GetObject(string _object, CancellationToken cancellationToken)
    {
        var filter = Builders<HistoricoClinico>.Filter.Eq(u => u.Id, _object);
        var character = _collection.Find(filter).FirstOrDefault();
        return character as HistoricoClinico;
    }

    public async Task<HistoricoClinico?> InsetObject(HistoricoClinico _object, CancellationToken cancellationToken)
    {
        await _collection.InsertOneAsync(_object);
        return _object as HistoricoClinico;
    }

    public async Task<HistoricoClinico?> UpdateObject(HistoricoClinico _object, CancellationToken cancellationToken)
    {
        var filter = MongoDB.Driver.Builders<HistoricoClinico>.Filter.Eq(u => u.Id, _object.Id);
        var update = MongoDB.Driver.Builders<HistoricoClinico>.Update
            .Set(u => u.Id, _object.Id)
            .Set(u => u.AnimalId, _object.AnimalId)
            .Set(u => u.Relatorios, _object.Relatorios)
            .Set(u => u.UltimaAtualizacao, _object.UltimaAtualizacao);

        // Perform the update
        var result = await _collection.UpdateOneAsync(filter, update);

        if (result.ModifiedCount > 0)
        {
            Console.WriteLine("User updated successfully.");
        }
        else
        {
            return null;
        }
        var ob = await GetObject(_object.Id, CancellationToken.None) as HistoricoClinico;
        return ob;
    }

    public async Task RemoveObject(object _object, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }
}