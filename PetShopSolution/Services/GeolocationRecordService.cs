using Data;
using DTOs;
using Interfaces;
using Microsoft.Extensions.Configuration;
using MongoDB.Driver;

namespace Services;

public class GeolocationRecordService : IGeolocationRecordService
{
    private readonly IConfiguration _configuration;
    protected IMongoCollection<GeolocationRecord> _collection;
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
        _collection = _mongoDatabase.GetCollection<GeolocationRecord>(_collectionName);
    }

    public GeolocationRecordService(IConfiguration configuration)
    {
        _cfg = configuration;
        _db = new GeolocationRecordDB(_cfg["MongoDbSettings:ConnectionString"], "GeolocationRecord");
        _db.GetOrCreateDatabase();
    }
    public async Task<GeolocationRecord?> GetObject(string _object, CancellationToken cancellationToken)
    {
        var collection = _db.GetDatabase().GetCollection<GeolocationRecord>("GeolocationRecord");
        
        var filter = MongoDB.Driver.Builders<GeolocationRecord>.Filter.Eq(u => u.Id, _object);
        
        var character = collection.Find(filter).FirstOrDefault();

        return character as GeolocationRecord;
    }

    public async Task<GeolocationRecord?> InsetObject(GeolocationRecord _object, CancellationToken cancellationToken)
    {
        var collection = _db.GetDatabase().GetCollection<GeolocationRecord>("GeolocationRecord");
        collection.InsertOne(_object);
        return _object as GeolocationRecord;
    }

    public async Task<GeolocationRecord?> UpdateObject(GeolocationRecord _object, CancellationToken cancellationToken)
    {
        var obj = await GetObject(_object.Id, CancellationToken.None) as GeolocationRecord;
        var collection = _db.GetDatabase().GetCollection<GeolocationRecord>("GeolocationRecord");

        // Create a filter to find the document by Id
        var filter = MongoDB.Driver.Builders<GeolocationRecord>.Filter.Eq(u => u.Id, _object.Id);

        var update = MongoDB.Driver.Builders<GeolocationRecord>.Update
            .Set(u => u.Id, _object.Id)
            .Set(u => u.AnimalId, _object.AnimalId)
            .Set(u => u.Latitude, _object.Latitude)
            .Set(u => u.Longitude, _object.Longitude)
            .Set(u => u.DataRegistro, _object.DataRegistro)
            .Set(u => u.Endereco, _object.Endereco)
            .Set(u => u.Observacoes, _object.Observacoes);

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
        var ob = await GetObject(_object.Id, CancellationToken.None) as GeolocationRecord;
        return ob;
    }

    public async Task RemoveObject(object _object, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }
}