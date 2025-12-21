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
    public HistoricoClinicoService(IConfiguration configuration)
    {
        _cfg = configuration;
        _db = new HistoricoClinicoDB(_cfg["MongoDbSettings:ConnectionString"], "HistoricoClinico");
        _db.GetOrCreateDatabase();
    }
    public async Task<HistoricoClinico?> GetObject(string _object, CancellationToken cancellationToken)
    {
        var collection = _db.GetDatabase().GetCollection<HistoricoClinico>("HistoricoClinico");
        
        var filter = MongoDB.Driver.Builders<HistoricoClinico>.Filter.Eq(u => u.Id, _object);
        
        var character = collection.Find(filter).FirstOrDefault();

        return character as HistoricoClinico;
    }

    public async Task<HistoricoClinico?> InsetObject(HistoricoClinico _object, CancellationToken cancellationToken)
    {
        var collection = _db.GetDatabase().GetCollection<HistoricoClinico>("HistoricoClinico");
        collection.InsertOne(_object);
        return _object as HistoricoClinico;
    }

    public async Task<HistoricoClinico?> UpdateObject(HistoricoClinico _object, CancellationToken cancellationToken)
    {
        var obj = await GetObject(_object.Id, CancellationToken.None) as HistoricoClinico;
        var collection = _db.GetDatabase().GetCollection<HistoricoClinico>("HistoricoClinico");

        // Create a filter to find the document by Id
        var filter = MongoDB.Driver.Builders<HistoricoClinico>.Filter.Eq(u => u.Id, _object.Id);

        var update = MongoDB.Driver.Builders<HistoricoClinico>.Update
            .Set(u => u.Id, _object.Id)
            .Set(u => u.AnimalId, _object.AnimalId)
            .Set(u => u.Relatorios, _object.Relatorios)
            .Set(u => u.UltimaAtualizacao, _object.UltimaAtualizacao);

        // Perform the update
        var result = collection.UpdateOne(filter, update);

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