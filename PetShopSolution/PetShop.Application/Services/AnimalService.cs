using MongoDB.Driver;
using PetShop.Application.Data;
using PetShop.Application.DTOs;
using PetShop.Application.Interfaces;

namespace PetShop.Application.Services;

public class AnimalService : IAnimalService
{
    public AnimalDBMongo _animalDb { get; set; }
    public async Task<object?> GetObject(string _object, CancellationToken cancellationToken)
    {
        var collection = _animalDb.GetDatabase().GetCollection<Animal>("Animais");

        // Create a filter to find the document by Id
        var filter = Builders<Animal>.Filter.Eq(u => u.Id, _object);

        // Find the document matching the filter
        var character = collection.Find(filter).FirstOrDefault();

        return character as Animal;
    }

    public async Task<object?> InsetObject(Animal _object, CancellationToken cancellationToken)
    {
        var collection = _animalDb.GetDatabase().GetCollection<Animal>("Animais");
        // Insert the user object into the collection
        collection.InsertOne(_object);
        return _object as Animal;
    }

    public async Task<object?> UpdateObject(Animal _object, CancellationToken cancellationToken)
    {
        var obj = await GetObject(_object.Id, CancellationToken.None) as Animal;
        var collection = _animalDb.GetDatabase().GetCollection<Animal>("Animais");

        // Create a filter to find the document by Id
        var filter = Builders<Animal>.Filter.Eq(u => u.Id, _object.Id);

        var update = Builders<Animal>.Update
            .Set(u => u.Nome, _object.Nome)
            .Set(u => u.Especie, _object.Especie)
            .Set(u => u.Raca, _object.Raca)
            .Set(u => u.Idade, _object.Idade)
            .Set(u => u.Peso, _object.Peso)
            .Set(u => u.Porte,  _object.Porte)
            .Set(u => u.ResponsavelId, _object.ResponsavelId);

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
        var ob = await GetObject(_object.Id, CancellationToken.None) as Animal;
        return ob;
    }

    public async Task RemoveObject(object _object, CancellationToken cancellationToken)
        => await Task.FromResult<object?>(null);
}