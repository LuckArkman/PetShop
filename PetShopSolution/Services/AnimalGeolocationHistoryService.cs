using Data;
using DTOs;
using Interfaces;
using Microsoft.Extensions.Configuration;
using MongoDB.Driver;

namespace Services;

public class AnimalGeolocationHistoryService : IAnimalGeolocationHistoryService
{
    public MongoDataController _db { get; set; }
    
    private readonly IConfiguration _configuration;
    protected IMongoCollection<AnimalGeolocationHistory> _collection;
    public string _collectionName { get; set; }
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
        _collection = _mongoDatabase.GetCollection<AnimalGeolocationHistory>(_collectionName);
    }
    public async Task<AnimalGeolocationHistory?> GetObject(string _object, CancellationToken cancellationToken)
    {
        var filter = Builders<AnimalGeolocationHistory>.Filter.Eq(u => u.AnimalId, _object);
        var character = _collection.Find(filter).FirstOrDefault();
        return character as AnimalGeolocationHistory;
    }

    public async Task<AnimalGeolocationHistory?> InsetObject(AnimalGeolocationHistory _object, CancellationToken cancellationToken)
    { 
        await _collection.InsertOneAsync(_object);
        return _object as AnimalGeolocationHistory;
    }

    public async Task<AnimalGeolocationHistory?> UpdateObject(AnimalGeolocationHistory _object, CancellationToken cancellationToken)
    {
        // Create a filter to find the document by Id
        var filter = Builders<AnimalGeolocationHistory>.Filter.Eq(u => u.Id, _object.Id);
        var update = Builders<AnimalGeolocationHistory>.Update
            .Set(u => u.Id, _object.Id)
            .Set(u => u.AnimalId, _object.AnimalId)
            .Set(u => u.Locations, _object.Locations);

        // Perform the update
        var result = _collection.UpdateOne(filter, update);

        if (result.ModifiedCount > 0)
        {
            Console.WriteLine("User updated successfully.");
        }
        else
        {
            return null;
        }
        var ob = await GetObject(_object.Id, CancellationToken.None) as AnimalGeolocationHistory;
        return ob;
    }

    public Task RemoveObject(AnimalGeolocationHistory _object, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }
}