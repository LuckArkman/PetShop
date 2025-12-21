using Data;
using DTOs;
using Interfaces;
using Microsoft.Extensions.Configuration;
using MongoDB.Driver;

namespace Services;

public class VacinacaoService : IVacinacaoService
{
    private readonly IConfiguration _configuration;
    protected IMongoCollection<Vacinacao> _collection;
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
        _collection = _mongoDatabase.GetCollection<Vacinacao>(_collectionName);
    }
    public VacinacaoService(IConfiguration configuration)
    {
        _cfg = configuration;
        _db = new VacinacaoDB(_cfg["MongoDbSettings:ConnectionString"], "Vacinacao");
        _db.GetOrCreateDatabase();
    }
    public async Task<object?> GetObject(string _object, CancellationToken cancellationToken)
    {
        var collection = _db.GetDatabase().GetCollection<Vacinacao>("Vacinacao");
        
        var filter = Builders<Vacinacao>.Filter.Eq(u => u.id, _object);
        
        var character = collection.Find(filter).FirstOrDefault();

        return character as Vacinacao;
    }

    public async Task<object?> InsetObject(Vacinacao _object, CancellationToken cancellationToken)
    {
        var collection = _db.GetDatabase().GetCollection<Vacinacao>("HistoryVacinacao");
        collection.InsertOne(_object);
        return _object as Vacinacao;
    }

    public async Task<object?> UpdateObject(Vacinacao _object, CancellationToken cancellationToken)
    {
        var obj = await GetObject(_object.id, CancellationToken.None) as Vacinacao;
        var collection = _db.GetDatabase().GetCollection<Vacinacao>("Animais");

        // Create a filter to find the document by Id
        var filter = Builders<Vacinacao>.Filter.Eq(u => u.id, _object.id);

        var update = Builders<Vacinacao>.Update
            .Set(u => u.id, _object.id)
            .Set(u => u.AnimalId, _object.AnimalId)
            .Set(u => u._dataVacinacao, _object._dataVacinacao)
            .Set(u => u.Tipo, _object.Tipo)
            .Set(u => u.Relatorio, _object.Relatorio)
            .Set(u => u._veterinarioCRMV, _object._veterinarioCRMV);

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
        var ob = await GetObject(_object.id, CancellationToken.None) as Animal;
        return ob;
    }

    public async Task<bool> RemoveObject(Vacinacao _object, CancellationToken cancellationToken)
    {
        var filter = Builders<Vacinacao>.Filter.Eq(u => u.id, _object.id);
        var _collection = _db.GetDatabase().GetCollection<Vacinacao>("Vacinacao");
        var result = await _collection.DeleteOneAsync(filter, cancellationToken);
        return result.DeletedCount > 0;  
    }
}