using MongoDB.Driver;
using PetShop.Application.Data;
using PetShop.Application.DTOs;
using PetShop.Application.Interfaces;
using PetShop.Application.Singletons;

namespace PetShop.Application.Services;

public class AnimalService : IAnimalService
{
    public AnimalDBMongo _animalDb { get; set; }

    public AnimalService()
    {
        _animalDb = new AnimalDBMongo(Singleton.Instance().src, "Animal");
        _animalDb.GetOrCreateDatabase();
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
    
    public async Task<object?> GetObject(string _object, CancellationToken cancellationToken)
    {
        var collection = _animalDb.GetDatabase().GetCollection<Animal>("Animais");

        // Create a filter to find the document by Id
        var filter = Builders<Animal>.Filter.Eq(u => u.id, _object);

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

    public async Task RemoveObject(object _object, CancellationToken cancellationToken)
    {
        var collection = _animalDb.GetDatabase().GetCollection<Animal>("Animais");

        // Verifica se o objeto passado é um Animal ou uma string com o id
        string id = _object switch
        {
            Animal animal => animal.id,
            string strId => strId,
            _ => throw new ArgumentException("O objeto deve ser um Animal ou um id válido.")
        };

        // Cria filtro para encontrar o documento pelo id
        var filter = Builders<Animal>.Filter.Eq(u => u.id, id);

        // Remove o documento
        var result = await collection.DeleteOneAsync(filter, cancellationToken);

        if (result.DeletedCount > 0)
        {
            Console.WriteLine($"Animal com id {id} removido com sucesso.");
        }
        else
        {
            Console.WriteLine($"Nenhum animal encontrado com id {id}.");
        }
    }

    public async Task<object?> GetAnimalsInList(ICollection<string> ids, CancellationToken cancellationToken)
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