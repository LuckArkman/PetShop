using MongoDB.Driver;
using PetShop.Application.Data;
using PetShop.Application.DTOs;
using PetShop.Application.Interfaces;
using PetShop.Application.Singletons;

namespace PetShop.Application.Services;

public class HistoricoClinicoService : IHistoricoClinicoService
{
    public HistoricoClinicoDB _db { get; set; }
    public HistoricoClinicoService()
    {
        _db = new HistoricoClinicoDB(Singleton.Instance().src, "HistoricoClinico");
        _db.GetOrCreateDatabase();
    }
    public async Task<object?> GetObject(string _object, CancellationToken cancellationToken)
    {
        var collection = _db.GetDatabase().GetCollection<HistoricoClinico>("HistoricoClinico");
        
        var filter = MongoDB.Driver.Builders<HistoricoClinico>.Filter.Eq(u => u.Id, _object);
        
        var character = collection.Find(filter).FirstOrDefault();

        return character as HistoricoClinico;
    }

    public async Task<object?> InsetObject(HistoricoClinico _object, CancellationToken cancellationToken)
    {
        var collection = _db.GetDatabase().GetCollection<HistoricoClinico>("HistoricoClinico");
        collection.InsertOne(_object);
        return _object as HistoricoClinico;
    }

    public async Task<object?> UpdateObject(HistoricoClinico _object, CancellationToken cancellationToken)
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