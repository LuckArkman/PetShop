using Data;
using DTOs;
using Interfaces;
using Microsoft.Extensions.Configuration;
using MongoDB.Driver;

namespace Services;

public class HistoryVacinacaoService : BaseMongoService<HistoryVacinacao>, IHistoryVacinacaoService
{
    public HistoryVacinacaoService(ITenantService tenantService, IConfiguration configuration)
        : base(tenantService, configuration)
    {
    }

    public async Task<HistoryVacinacao?> GetObject(string _object, CancellationToken cancellationToken)
    {
        var filter = Builders<HistoryVacinacao>.Filter.Eq(u => u.id, _object);
        var character = await GetCollection().Find(filter).FirstOrDefaultAsync(cancellationToken);
        return character;
    }

    public async Task<HistoryVacinacao?> InsetObject(HistoryVacinacao _object, CancellationToken cancellationToken)
    {
        await GetCollection().InsertOneAsync(_object, cancellationToken: cancellationToken);
        return _object;
    }

    public async Task<HistoryVacinacao?> UpdateObject(HistoryVacinacao obj, CancellationToken cancellationToken)
    {
        var filter = Builders<HistoryVacinacao>.Filter.Eq(u => u.id, obj.id);
        var update = Builders<HistoryVacinacao>.Update
            .Set(u => u._animalId, obj._animalId)
            .Set(u => u._Vacinacao, obj._Vacinacao);

        var result = await GetCollection().UpdateOneAsync(filter, update, cancellationToken: cancellationToken);

        if (result.ModifiedCount > 0)
        {
            return await GetObject(obj.id!, cancellationToken);
        }

        return null;
    }

    public Task RemoveObject(object _object, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }

    public async Task<HistoryVacinacao?> GetHistoricoAnimal(string animalId, CancellationToken cancellationToken)
    {
        var filter = Builders<HistoryVacinacao>.Filter.Eq(u => u._animalId, animalId);
        var character = await GetCollection().Find(filter).FirstOrDefaultAsync(cancellationToken);
        return character;
    }
}