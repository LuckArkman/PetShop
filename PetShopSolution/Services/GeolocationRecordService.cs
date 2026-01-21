using Data;
using DTOs;
using Interfaces;
using Microsoft.Extensions.Configuration;
using MongoDB.Driver;

namespace Services;

public class GeolocationRecordService : BaseMongoService<GeolocationRecord>, IGeolocationRecordService
{
    public GeolocationRecordService(ITenantService tenantService, IConfiguration configuration)
        : base(tenantService, configuration)
    {
    }

    public async Task<GeolocationRecord?> GetObject(string _object, CancellationToken cancellationToken)
    {
        var filter = Builders<GeolocationRecord>.Filter.Eq(u => u.Id, _object);
        var character = await GetCollection().Find(filter).FirstOrDefaultAsync(cancellationToken);
        return character;
    }

    public async Task<GeolocationRecord?> InsetObject(GeolocationRecord _object, CancellationToken cancellationToken)
    {
        await GetCollection().InsertOneAsync(_object, cancellationToken: cancellationToken);
        return _object;
    }

    public async Task<GeolocationRecord?> UpdateObject(GeolocationRecord _object, CancellationToken cancellationToken)
    {
        var filter = Builders<GeolocationRecord>.Filter.Eq(u => u.Id, _object.Id);

        var update = Builders<GeolocationRecord>.Update
            .Set(u => u.Id, _object.Id)
            .Set(u => u.AnimalId, _object.AnimalId)
            .Set(u => u.Latitude, _object.Latitude)
            .Set(u => u.Longitude, _object.Longitude)
            .Set(u => u.DataRegistro, _object.DataRegistro)
            .Set(u => u.Endereco, _object.Endereco)
            .Set(u => u.Observacoes, _object.Observacoes);

        var result = await GetCollection().UpdateOneAsync(filter, update, cancellationToken: cancellationToken);

        if (result.ModifiedCount > 0)
        {
            Console.WriteLine("GeolocationRecord updated successfully.");
        }
        else
        {
            return null;
        }

        return await GetObject(_object.Id, cancellationToken);
    }

    public Task RemoveObject(object _object, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }
}