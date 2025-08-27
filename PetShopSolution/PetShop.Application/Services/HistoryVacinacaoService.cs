using MongoDB.Driver;
using PetShop.Application.Data;
using PetShop.Application.DTOs;
using PetShop.Application.Interfaces;
using PetShop.Application.Singletons;

namespace PetShop.Application.Services;

public class HistoryVacinacaoService : IHistoryVacinacaoService
{
    public HistoryVacinacaoDB _db { get; set; }
    public HistoryVacinacaoService()
    {
        _db = new HistoryVacinacaoDB(Singleton.Instance().src, "HistoryVacinacao");
        _db.GetOrCreateDatabase();
    }
    public async Task<object?> GetObject(string _object, CancellationToken cancellationToken)
    {
        var collection = _db.GetDatabase().GetCollection<HistoryVacinacao>("HistoryVacinacao");
        
        var filter = Builders<HistoryVacinacao>.Filter.Eq(u => u.id, _object);
        
        var character = collection.Find(filter).FirstOrDefault();

        return character as HistoryVacinacao;
    }

    public async Task<object?> InsetObject(HistoryVacinacao _object, CancellationToken cancellationToken)
    {
        var collection = _db.GetDatabase().GetCollection<HistoryVacinacao>("HistoryVacinacao");
        collection.InsertOne(_object);
        return _object as HistoryVacinacao;
    }

    public async Task<object?> UpdateObject(HistoryVacinacao _object, CancellationToken cancellationToken)
    {
        var obj = await GetObject(_object.id, CancellationToken.None) as HistoryVacinacao;
        var collection = _db.GetDatabase().GetCollection<HistoryVacinacao>("Animais");

        // Create a filter to find the document by Id
        var filter = Builders<HistoryVacinacao>.Filter.Eq(u => u.id, _object.id);

        var update = Builders<HistoryVacinacao>.Update
            .Set(u => u.id, _object.id)
            .Set(u => u._animalId, _object._animalId)
            .Set(u => u._Vacinacao, _object._Vacinacao);

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

    public Task RemoveObject(object _object, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }
}