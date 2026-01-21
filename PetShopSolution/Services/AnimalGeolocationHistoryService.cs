using Data;
using DTOs;
using Interfaces;
using Microsoft.Extensions.Configuration;
using MongoDB.Driver;

namespace Services;

public class AnimalGeolocationHistoryService : BaseMongoService<AnimalGeolocationHistory>, IAnimalGeolocationHistoryService
{
    public AnimalGeolocationHistoryService(ITenantService tenantService, IConfiguration configuration)
        : base(tenantService, configuration)
    {
    }

    public async Task<AnimalGeolocationHistory?> GetObject(string _object, CancellationToken cancellationToken)
    {
        var filter = Builders<AnimalGeolocationHistory>.Filter.Eq(u => u.AnimalId, _object);
        var character = await GetCollection().Find(filter).FirstOrDefaultAsync(cancellationToken);
        return character;
    }

    public async Task<AnimalGeolocationHistory?> InsetObject(AnimalGeolocationHistory _object, CancellationToken cancellationToken)
    {
        await GetCollection().InsertOneAsync(_object, cancellationToken: cancellationToken);
        return _object;
    }

    public async Task<AnimalGeolocationHistory?> UpdateObject(AnimalGeolocationHistory _object, CancellationToken cancellationToken)
    {
        var filter = Builders<AnimalGeolocationHistory>.Filter.Eq(u => u.Id, _object.Id);
        var update = Builders<AnimalGeolocationHistory>.Update
            .Set(u => u.Id, _object.Id)
            .Set(u => u.AnimalId, _object.AnimalId)
            .Set(u => u.Locations, _object.Locations);

        var result = await GetCollection().UpdateOneAsync(filter, update, cancellationToken: cancellationToken);

        if (result.ModifiedCount > 0)
        {
            Console.WriteLine("AnimalGeolocationHistory updated successfully.");
        }
        else
        {
            return null;
        }

        return await GetObject(_object.AnimalId!, cancellationToken);
    }

    public Task RemoveObject(AnimalGeolocationHistory _object, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }
}