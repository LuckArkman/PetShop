using Data;
using DTOs;
using Interfaces;
using Microsoft.Extensions.Configuration;
using MongoDB.Driver;

namespace Services;

public class AnimalGeolocationHistoryService : IAnimalGeolocationHistoryService
{
    public AnimalGeolocationHistoryDB _db { get; set; }
    private readonly IConfiguration _cfg;
    public AnimalGeolocationHistoryService(IConfiguration configuration)
    {
        _cfg = configuration;
        _db = new AnimalGeolocationHistoryDB(_cfg["MongoDbSettings:ConnectionString"], "AnimalGeolocationHistory");
        _db.GetOrCreateDatabase();
    }
    public async Task<AnimalGeolocationHistory?> GetObject(string _object, CancellationToken cancellationToken)
    {
        var collection = _db.GetDatabase().GetCollection<AnimalGeolocationHistory>("AnimalGeolocationHistory");
        
        var filter = Builders<AnimalGeolocationHistory>.Filter.Eq(u => u.Id, _object);
        
        var character = collection.Find(filter).FirstOrDefault();

        return character as AnimalGeolocationHistory;
    }

    public async Task<AnimalGeolocationHistory?> InsetObject(AnimalGeolocationHistory _object, CancellationToken cancellationToken)
    {
        var collection = _db.GetDatabase().GetCollection<AnimalGeolocationHistory>("AnimalGeolocationHistory");
        collection.InsertOne(_object);
        return _object as AnimalGeolocationHistory;
    }

    public async Task<AnimalGeolocationHistory?> UpdateObject(AnimalGeolocationHistory _object, CancellationToken cancellationToken)
    {
        var obj = await GetObject(_object.Id, CancellationToken.None) as AnimalGeolocationHistory;
        var collection = _db.GetDatabase().GetCollection<AnimalGeolocationHistory>("Animais");

        // Create a filter to find the document by Id
        var filter = Builders<AnimalGeolocationHistory>.Filter.Eq(u => u.Id, _object.Id);

        var update = Builders<AnimalGeolocationHistory>.Update
            .Set(u => u.Id, _object.Id)
            .Set(u => u.AnimalId, _object.AnimalId)
            .Set(u => u.Locations, _object.Locations);

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
        var ob = await GetObject(_object.Id, CancellationToken.None) as AnimalGeolocationHistory;
        return ob;
    }

    public Task RemoveObject(AnimalGeolocationHistory _object, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }
}