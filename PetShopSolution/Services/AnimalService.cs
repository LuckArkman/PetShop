using Data;
using DTOs;
using Interfaces;
using Microsoft.Extensions.Configuration;
using MongoDB.Driver;

namespace Services;

public class AnimalService : IAnimalService
{
    public AnimalDBMongo _animalDb { get; set; }
    private readonly IMongoCollection<Animal> _collection;
    private readonly IConfiguration _cfg;

    public AnimalService(IConfiguration configuration)
    {
        _cfg = configuration;
        _animalDb = new AnimalDBMongo(_cfg["MongoDbSettings:ConnectionString"], "Animal");
        _animalDb.GetOrCreateDatabase();
        _collection = _animalDb.GetDatabase().GetCollection<Animal>("Animais");
    }
    public async Task<List<Animal>?> GetAllAnimals(CancellationToken cancellationToken)
    {

        // Busca todos os animais
        var animals = await _collection
            .Find(Builders<Animal>.Filter.Empty)
            .ToListAsync(cancellationToken);

        return animals;
    }
    
    public async Task<Animal?> GetObject(string _object, CancellationToken cancellationToken)
    {
        // Create a filter to find the document by Id
        var filter = Builders<Animal>.Filter.Eq(u => u.id, _object);

        // Find the document matching the filter
        var character = _collection.Find(filter).FirstOrDefault();

        return character as Animal;
    }

    public async Task<Animal?> InsetObject(Animal _object, CancellationToken cancellationToken)
    {
        _collection.InsertOne(_object);
        return _object as Animal;
    }

    public async Task<Animal?> UpdateObject(Animal _object, CancellationToken cancellationToken)
    {
        var obj = await GetObject(_object.id, CancellationToken.None) as Animal;

        // Create a filter to find the document by Id
        var filter = Builders<Animal>.Filter.Eq(u => u.id, _object.id);

        var update = Builders<Animal>.Update
            .Set(u => u.Nome, _object.Nome)
            .Set(u => u.Especie, _object.Especie)
            .Set(u => u.Raca, _object.Raca)
            .Set(u => u.Porte,  _object.Porte)
            .Set(u => u.responsaveis, _object.responsaveis);

        // Perform the update
        var result = _collection.UpdateOne(filter, update);

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

    public async Task<bool> RemoveObject(string _object, CancellationToken cancellationToken)
    {
        var filter = Builders<Animal>.Filter.Eq(u => u.id, _object);
        var result = await _collection.DeleteOneAsync(filter, cancellationToken);
        return result.DeletedCount > 0;  
    }

    public async Task<List<Animal>?> GetAnimalsInList(List<string> ids, CancellationToken cancellationToken)
    {
        var filter = Builders<Animal>.Filter.In(a => a.id, ids);
        var animals = await _collection.Find(filter).ToListAsync(cancellationToken);
        Console.WriteLine($"Mongo retornou {animals.Count} animais.");
        return animals;
    }
}