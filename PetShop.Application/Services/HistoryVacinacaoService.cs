using Microsoft.Extensions.Configuration;
using MongoDB.Driver;
using PetShop.Application.Data;
using PetShop.Application.DTOs;
using PetShop.Application.Interfaces;
using PetShop.Application.Singletons;

namespace PetShop.Application.Services;

public class HistoryVacinacaoService : IHistoryVacinacaoService
{
    public HistoryVacinacaoDB _db { get; set; }
    private readonly IConfiguration _cfg;
    public HistoryVacinacaoService(IConfiguration configuration)
    {
        _cfg = configuration;
        _db = new HistoryVacinacaoDB(_cfg["MongoDbSettings:ConnectionString"], "HistoryVacinacao");
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

    public async Task<object?> UpdateObject(HistoryVacinacao obj, CancellationToken cancellationToken)
    {
        var collection = _db.GetDatabase().GetCollection<HistoryVacinacao>("HistoryVacinacao");

        var filter = Builders<HistoryVacinacao>.Filter.Eq(u => u.id, obj.id);
        var update = Builders<HistoryVacinacao>.Update
            .Set(u => u._animalId, obj._animalId)
            .Set(u => u._Vacinacao, obj._Vacinacao);

        var result = await collection.UpdateOneAsync(filter, update, cancellationToken: cancellationToken);

        if (result.ModifiedCount > 0)
        {
            return await GetObject(obj.id, cancellationToken);
        }

        return null;
    }


    public Task RemoveObject(object _object, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }

    public async Task<HistoryVacinacao?> GetHistoricoAnimal(string animalId, CancellationToken none)
    {
        var collection = _db.GetDatabase().GetCollection<HistoryVacinacao>("HistoryVacinacao");
        
        var filter = Builders<HistoryVacinacao>.Filter.Eq(u => u._animalId, animalId);
        
        var character = collection.Find(filter).FirstOrDefault();

        return character as HistoryVacinacao;
    }
}