using Data;
using DTOs;
using Interfaces;
using Microsoft.Extensions.Configuration;
using MongoDB.Driver;

namespace Services;

public class AnimalService : BaseMongoService<Animal>, IAnimalService
{
    public AnimalService(ITenantService tenantService, IConfiguration configuration)
        : base(tenantService, configuration)
    {
    }

    public async Task<List<Animal>?> GetAllAnimals(CancellationToken cancellationToken)
    {
        var animals = await GetCollection()
            .Find(Builders<Animal>.Filter.Empty)
            .ToListAsync(cancellationToken);

        return animals;
    }

    public async Task<Animal?> GetObject(string _object, CancellationToken cancellationToken)
    {
        var filter = Builders<Animal>.Filter.Eq(u => u.id, _object);
        var character = await GetCollection().Find(filter).FirstOrDefaultAsync(cancellationToken);
        return character;
    }

    public async Task<Animal?> InsetObject(Animal _object, CancellationToken cancellationToken)
    {
        await GetCollection().InsertOneAsync(_object, cancellationToken: cancellationToken);
        return _object;
    }

    public async Task<Animal?> UpdateObject(Animal _object, CancellationToken cancellationToken)
    {
        var filter = Builders<Animal>.Filter.Eq(u => u.id, _object.id);
        var update = Builders<Animal>.Update
            .Set(u => u.Nome, _object.Nome)
            .Set(u => u.Especie, _object.Especie)
            .Set(u => u.Raca, _object.Raca)
            .Set(u => u._idade.anos, _object._idade.anos)
            .Set(u => u._idade.meses, _object._idade.meses)
            .Set(u => u._peso.kilos, _object._peso.kilos)
            .Set(u => u._peso.gramas, _object._peso.gramas)
            .Set(u => u.Porte, _object.Porte)
            .Set(u => u.responsaveis, _object.responsaveis);

        var result = await GetCollection().UpdateOneAsync(filter, update, cancellationToken: cancellationToken);

        if (result.ModifiedCount > 0)
        {
            Console.WriteLine("Animal updated successfully.");
        }
        else
        {
            return null;
        }

        return await GetObject(_object.id, cancellationToken);
    }

    public async Task<bool> RemoveObject(string _object, CancellationToken cancellationToken)
    {
        var filter = Builders<Animal>.Filter.Eq(u => u.id, _object);
        var result = await GetCollection().DeleteOneAsync(filter, cancellationToken);
        return result.DeletedCount > 0;
    }

    public async Task<List<Animal>?> GetAnimalsInList(List<string> ids, CancellationToken cancellationToken)
    {
        var filter = Builders<Animal>.Filter.In(a => a.id, ids);
        var animals = await GetCollection().Find(filter).ToListAsync(cancellationToken);
        return animals;
    }
}