using Data;
using DTOs;
using Interfaces;
using Microsoft.Extensions.Configuration;
using MongoDB.Driver;

namespace Services;

public class VacinacaoService : BaseMongoService<Vacinacao>, IVacinacaoService
{
    public VacinacaoService(ITenantService tenantService, IConfiguration configuration)
        : base(tenantService, configuration)
    {
    }

    public async Task<object?> GetObject(string _object, CancellationToken cancellationToken)
    {
        var filter = Builders<Vacinacao>.Filter.Eq(u => u.id, _object);
        var character = await GetCollection().Find(filter).FirstOrDefaultAsync(cancellationToken);
        return character;
    }

    public async Task<object?> InsetObject(Vacinacao _object, CancellationToken cancellationToken)
    {
        await GetCollection().InsertOneAsync(_object, cancellationToken: cancellationToken);
        return _object;
    }

    public async Task<object?> UpdateObject(Vacinacao _object, CancellationToken cancellationToken)
    {
        var filter = Builders<Vacinacao>.Filter.Eq(u => u.id, _object.id);
        var update = Builders<Vacinacao>.Update
            .Set(u => u.id, _object.id)
            .Set(u => u.AnimalId, _object.AnimalId)
            .Set(u => u._dataVacinacao, _object._dataVacinacao)
            .Set(u => u.Tipo, _object.Tipo)
            .Set(u => u.Relatorio, _object.Relatorio)
            .Set(u => u._veterinarioCRMV, _object._veterinarioCRMV);

        var result = await GetCollection().UpdateOneAsync(filter, update, cancellationToken: cancellationToken);

        if (result.ModifiedCount > 0)
        {
            Console.WriteLine("Vacinacao updated successfully.");
        }
        else
        {
            return null;
        }

        return await GetObject(_object.id!, cancellationToken);
    }

    public async Task<bool> RemoveObject(Vacinacao _object, CancellationToken cancellationToken)
    {
        var filter = Builders<Vacinacao>.Filter.Eq(u => u.id, _object.id);
        var result = await GetCollection().DeleteOneAsync(filter, cancellationToken);
        return result.DeletedCount > 0;
    }
}