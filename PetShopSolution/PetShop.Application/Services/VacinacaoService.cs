using MongoDB.Driver;
using PetShop.Application.Data;
using PetShop.Application.DTOs;
using PetShop.Application.Interfaces;
using PetShop.Application.Singletons;

namespace PetShop.Application.Services;

public class VacinacaoService : IVacinacaoService
{
    public VacinacaoDB _db { get; set; }
    public VacinacaoService()
    {
        _db = new VacinacaoDB(Singleton.Instance().src, "Vacinacao");
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
            .Set(u => u._veterinarioId, _object._veterinarioId);

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