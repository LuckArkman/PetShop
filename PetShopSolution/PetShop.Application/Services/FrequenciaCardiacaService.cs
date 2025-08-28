using MongoDB.Driver;
using PetShop.Application.Data;
using PetShop.Application.DTOs;
using PetShop.Application.Interfaces;
using PetShop.Application.Singletons;

namespace PetShop.Application.Services;

public class FrequenciaCardiacaService : IFrequenciaCardiaca
{
    public FrequenciaCardiacaDB _db { get; set; }
    public FrequenciaCardiacaService()
    {
        _db = new FrequenciaCardiacaDB(Singleton.Instance().src, "FrequenciaCardiaca");
        _db.GetOrCreateDatabase();
    }

    public async Task<object?> GetObject(string _object, CancellationToken cancellationToken)
    {
        var collection = _db.GetDatabase().GetCollection<FrequenciaCardiaca>("FrequenciaCardiaca");

        // Create a filter to find the document by Id
        var filter = Builders<FrequenciaCardiaca>.Filter.Eq(u => u.AnimalId, _object);

        // Find the document matching the filter
        var character = collection.Find(filter).FirstOrDefault();

        return character as FrequenciaCardiaca;
    }

    public async Task<object?> InsetObject(FrequenciaCardiaca _object, CancellationToken cancellationToken)
    {
        var collection = _db.GetDatabase().GetCollection<FrequenciaCardiaca>("FrequenciaCardiaca");
        // Insert the user object into the collection
        collection.InsertOne(_object);
        return _object as FrequenciaCardiaca;
    }

    public async Task<object?> UpdateObject(FrequenciaCardiaca _object, CancellationToken cancellationToken)
    {
        var obj = await GetObject(_object.Id, CancellationToken.None) as FrequenciaCardiaca;
        var collection = _db.GetDatabase().GetCollection<FrequenciaCardiaca>("FrequenciaCardiaca");

        // Create a filter to find the document by Id
        var filter = Builders<FrequenciaCardiaca>.Filter.Eq(u => u.Id, _object.Id);

        var update = Builders<FrequenciaCardiaca>.Update
            .Set(u => u.Id, _object.Id)
            .Set(u => u.AnimalId, _object.AnimalId)
            .Set(u => u.Horario, _object.Horario)
            .Set(u => u.BatimentosPorMinuto, _object.BatimentosPorMinuto)
            .Set(u => u.Observacao, _object.Observacao);

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
        var ob = await GetObject(_object.Id, CancellationToken.None) as FrequenciaCardiaca;
        return ob;
    }

    public Task RemoveObject(object _object, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }
}