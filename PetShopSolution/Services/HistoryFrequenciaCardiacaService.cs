using Data;
using DTOs;
using Interfaces;
using Microsoft.Extensions.Configuration;
using MongoDB.Driver;

namespace Services;

public class HistoryFrequenciaCardiacaService : IHistoryFrequenciaCardiaca
{
    public HistoryFrequenciaCardiacaDB _db { get; set; }
    private readonly IConfiguration _cfg;
    public HistoryFrequenciaCardiacaService(IConfiguration configuration)
    {
        _cfg = configuration;
        _db = new HistoryFrequenciaCardiacaDB(_cfg["MongoDbSettings:ConnectionString"], "HistoryFrequenciaCardiaca");
        _db.GetOrCreateDatabase();
    }

    public async Task<HistoryFrequenciaCardiaca?> GetObject(string _object, CancellationToken cancellationToken)
    {
        var collection = _db.GetDatabase().GetCollection<HistoryFrequenciaCardiaca>("HistoryFrequenciaCardiaca");

        // Create a filter to find the document by Id
        var filter = Builders<HistoryFrequenciaCardiaca>.Filter.Eq(u => u.AnimalId, _object);

        // Find the document matching the filter
        var character = collection.Find(filter).FirstOrDefault();

        return character as HistoryFrequenciaCardiaca;
    }

    public async Task<HistoryFrequenciaCardiaca?> InsetObject(HistoryFrequenciaCardiaca _object, CancellationToken cancellationToken)
    {
        var collection = _db.GetDatabase().GetCollection<HistoryFrequenciaCardiaca>("HistoryFrequenciaCardiaca");
        // Insert the user object into the collection
        collection.InsertOne(_object);
        return _object as HistoryFrequenciaCardiaca;
    }

    public async Task<HistoryFrequenciaCardiaca?> UpdateObject(HistoryFrequenciaCardiaca _object, CancellationToken cancellationToken)
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