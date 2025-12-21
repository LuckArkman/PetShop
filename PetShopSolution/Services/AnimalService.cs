using Data;
using DTOs;
using Interfaces;
using Microsoft.Extensions.Configuration;
using MongoDB.Driver;

namespace Services;

public class AnimalService : IAnimalService
{
    private IMongoCollection<Animal> _collection { get; set; }
    public string _collectionName { get; set; }
    private MongoDataController _db { get; set; }
    private IMongoDatabase _mongoDatabase { get; set; }
    
    public void InitializeCollection(string connectionString,
        string databaseName,
        string collectionName)
    {
        _collectionName = collectionName;
        // Verifica se a conexão já foi estabelecida
        if (_collection != null) return;
        
        _db = new MongoDataController(connectionString, databaseName, _collectionName);
        _mongoDatabase = _db.GetDatabase();
        _collection = _mongoDatabase.GetCollection<Animal>(_collectionName);
    }
    public async Task<List<Animal>?> GetAllAnimals(CancellationToken cancellationToken)
    {
        var animals = await _collection
            .Find(Builders<Animal>.Filter.Empty)
            .ToListAsync(cancellationToken);

        return animals;
    }
    
    public async Task<Animal?> GetObject(string _object, CancellationToken cancellationToken)
    {
        var filter = Builders<Animal>.Filter.Eq(u => u.id, _object);
        var character = _collection.Find(filter).FirstOrDefault();
        return character as Animal;
    }

    public async Task<Animal?> InsetObject(Animal _object, CancellationToken cancellationToken)
    {
        await _collection.InsertOneAsync(_object);
        return _object as Animal;
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
            .Set(u => u.Raca, _object.Raca)
            .Set(u => u.Porte,  _object.Porte)
            .Set(u => u.responsaveis, _object.responsaveis);
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