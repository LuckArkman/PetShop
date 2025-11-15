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
        _collection = _animalDb.GetDatabase().GetCollection<Animal>("Animal");
    }
    public async Task<List<Animal>?> GetAllAnimals(CancellationToken cancellationToken)
    {
        var collection = _animalDb.GetDatabase().GetCollection<Animal>("Animais");

        // Busca todos os animais
        var animals = await collection
            .Find(Builders<Animal>.Filter.Empty)
            .ToListAsync(cancellationToken);

        return animals;
    }
    
    public async Task<Animal?> GetObject(string _object, CancellationToken cancellationToken)
    {
        var collection = _animalDb.GetDatabase().GetCollection<Animal>("Animais");

        // Create a filter to find the document by Id
        var filter = Builders<Animal>.Filter.Eq(u => u.id, _object);

        // Find the document matching the filter
        var character = collection.Find(filter).FirstOrDefault();

        return character as Animal;
    }

    public async Task<Animal?> InsetObject(Animal _object, CancellationToken cancellationToken)
    {
        var collection = _animalDb.GetDatabase().GetCollection<Animal>("Animais");
        // Insert the user object into the collection
        collection.InsertOne(_object);
        return _object as Animal;
    }

    public async Task<Animal?> UpdateObject(Animal _object, CancellationToken cancellationToken)
    {
        var obj = await GetObject(_object.id, CancellationToken.None) as Animal;
        var collection = _animalDb.GetDatabase().GetCollection<Animal>("Animais");

        // Create a filter to find the document by Id
        var filter = Builders<Animal>.Filter.Eq(u => u.id, _object.id);

        var update = Builders<Animal>.Update
            .Set(u => u.Nome, _object.Nome)
            .Set(u => u.Especie, _object.Especie)
            .Set(u => u.Raca, _object.Raca)
            .Set(u => u.Idade, _object.Idade)
            .Set(u => u.Peso, _object.Peso)
            .Set(u => u.Porte,  _object.Porte)
            .Set(u => u.responsaveis, _object.responsaveis);

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

    public async Task<bool> RemoveObject(string _object, CancellationToken cancellationToken)
    {
        var filter = Builders<Animal>.Filter.Eq(u => u.id, _object);
        var result = await _collection.DeleteOneAsync(filter, cancellationToken);
        return result.DeletedCount > 0;  
    }

    public async Task<List<Animal>?> GetAnimalsInList(ICollection<string> ids, CancellationToken cancellationToken)
    {
        var collection = _animalDb.GetDatabase().GetCollection<Animal>("Animais");

        // Cria um filtro para pegar apenas os animais cujo id esteja na lista recebida
        var filter = Builders<Animal>.Filter.In(a => a.id, ids);

        // Busca todos os animais que correspondem
        var animals = await collection
            .Find(filter)
            .ToListAsync(cancellationToken);

        return animals;
    }
}