using MongoDB.Driver;
using PetShop.Application.Data;
using PetShop.Application.DTOs;
using PetShop.Application.Interfaces;
using PetShop.Application.Singletons;

namespace PetShop.Application.Services;

public class HistoryFrequenciaCardiacaService : IHistoryFrequenciaCardiaca
{
    public HistoryFrequenciaCardiacaDB _db { get; set; }
    public HistoryFrequenciaCardiacaService()
    {
        _db = new HistoryFrequenciaCardiacaDB(Singleton.Instance().src, "HistoryFrequenciaCardiaca");
        _db.GetOrCreateDatabase();
    }

    public async Task<object?> GetObject(string _object, CancellationToken cancellationToken)
    {
        var collection = _db.GetDatabase().GetCollection<HistoryFrequenciaCardiaca>("HistoryFrequenciaCardiaca");

        // Create a filter to find the document by Id
        var filter = Builders<HistoryFrequenciaCardiaca>.Filter.Eq(u => u.AnimalId, _object);

        // Find the document matching the filter
        var character = collection.Find(filter).FirstOrDefault();

        return character as HistoryFrequenciaCardiaca;
    }

    public async Task<object?> InsetObject(HistoryFrequenciaCardiaca _object, CancellationToken cancellationToken)
    {
        var collection = _db.GetDatabase().GetCollection<HistoryFrequenciaCardiaca>("HistoryFrequenciaCardiaca");
        // Insert the user object into the collection
        collection.InsertOne(_object);
        return _object as HistoryFrequenciaCardiaca;
    }

    public async Task<object?> UpdateObject(HistoryFrequenciaCardiaca _object, CancellationToken cancellationToken)
    {
        var obj = await GetObject(_object.id, CancellationToken.None) as HistoryFrequenciaCardiaca;
        var collection = _db.GetDatabase().GetCollection<HistoryFrequenciaCardiaca>("HistoryFrequenciaCardiaca");

        // Create a filter to find the document by Id
        var filter = Builders<HistoryFrequenciaCardiaca>.Filter.Eq(u => u.id, _object.id);

        var update = Builders<HistoryFrequenciaCardiaca>.Update
            .Set(u => u.id, _object.id)
            .Set(u => u.AnimalId, _object.AnimalId)
            .Set(u => u._frequenciaCardiaca, _object._frequenciaCardiaca);

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
        var ob = await GetObject(_object.id, CancellationToken.None) as HistoryFrequenciaCardiaca;
        return ob;
    }

    public Task RemoveObject(object _object, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }
}