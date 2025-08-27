using MongoDB.Driver;
using PetShop.Application.Data;
using PetShop.Application.DTOs;
using PetShop.Application.Interfaces;
using PetShop.Application.Singletons;

namespace PetShop.Application.Services;

public class AnimalGeolocationHistoryService : IAnimalGeolocationHistoryService
{
    public AnimalGeolocationHistoryDB _db { get; set; }
    public AnimalGeolocationHistoryService()
    {
        _db = new AnimalGeolocationHistoryDB(Singleton.Instance().src, "AnimalGeolocationHistory");
        _db.GetOrCreateDatabase();
    }
    public async Task<object?> GetObject(string _object, CancellationToken cancellationToken)
    {
        var collection = _db.GetDatabase().GetCollection<AnimalGeolocationHistory>("AnimalGeolocationHistory");
        
        var filter = Builders<AnimalGeolocationHistory>.Filter.Eq(u => u.Id, _object);
        
        var character = collection.Find(filter).FirstOrDefault();

        return character as AnimalGeolocationHistory;
    }

    public async Task<object?> InsetObject(AnimalGeolocationHistory _object, CancellationToken cancellationToken)
    {
        var collection = _db.GetDatabase().GetCollection<AnimalGeolocationHistory>("AnimalGeolocationHistory");
        collection.InsertOne(_object);
        return _object as AnimalGeolocationHistory;
    }

    public async Task<object?> UpdateObject(AnimalGeolocationHistory _object, CancellationToken cancellationToken)
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

    public Task RemoveObject(object _object, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }
}