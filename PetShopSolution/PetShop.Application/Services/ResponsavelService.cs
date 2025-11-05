using MongoDB.Driver;
using PetShop.Application.Data;
using PetShop.Application.DTOs;
using PetShop.Application.Interfaces;
using PetShop.Application.Singletons;

namespace PetShop.Application.Services;

public class ResponsavelService : IResponsavelService
{
    public ResponsavelDBMongo _dbMongo { get; set; }
    private readonly IMongoCollection<Responsavel> _collection;

    public ResponsavelService()
    {
        _dbMongo = new ResponsavelDBMongo(Singleton.Instance().src, "Responsavel");
        _dbMongo.GetOrCreateDatabase();
        _collection = _dbMongo.GetDatabase().GetCollection<Responsavel>("Responsavel");
    }

    public async Task<List<Responsavel>?> GetAllResponsavel(CancellationToken cancellationToken)
    {
        var collection = _dbMongo.GetDatabase().GetCollection<Responsavel>("Responsavel");

        // Busca todos os animais
        var _objts = await collection
            .Find(Builders<Responsavel>.Filter.Empty)
            .ToListAsync(cancellationToken);

        return _objts;
    }

    public async Task<object?> GetObject(string mail, CancellationToken cancellationToken)
    {
        var collection = _dbMongo.GetDatabase().GetCollection<Responsavel>("Responsavel");
        
        var filter = Builders<Responsavel>.Filter.Eq(u => u.Email, mail);

        // Find the document matching the filter
        var _responsavel = collection.Find(filter).FirstOrDefault();

        return _responsavel as Responsavel;
    }
    
    public async Task<object?> GetResponsavelId(string cpf, CancellationToken cancellationToken)
    {
        var collection = _dbMongo.GetDatabase().GetCollection<Responsavel>("Responsavel");

        // Create a filter to find the document by Id
        var filter = Builders<Responsavel>.Filter.Eq(u => u.CPF, cpf);

        // Find the document matching the filter
        var _responsavel = collection.Find(filter).FirstOrDefault();

        return _responsavel as Responsavel;
    }

    public async Task<object?> InsetObject(Responsavel _object, CancellationToken cancellationToken)
    {
        var collection = _dbMongo.GetDatabase().GetCollection<Responsavel>("Responsavel");
        // Insert the user object into the collection
        collection.InsertOne(_object);
        return _object as Responsavel;
    }

    public async Task<object?> UpdateObject(Responsavel _object, CancellationToken cancellationToken)
    {
        if (_object.Email != null)
        {
            var obj = await FindByEmailAsync(_object.Email, CancellationToken.None) as Responsavel;
        }

        var collection = _dbMongo.GetDatabase().GetCollection<Responsavel>("Responsavel");

        // Create a filter to find the document by Id
        var filter = Builders<Responsavel>.Filter.Eq(u => u.Email, _object.Email);

        var update = Builders<Responsavel>.Update
            .Set(u => u.Email, _object.Email)
            .Set(u => u.FirstName, _object.FirstName)
            .Set(u => u.LastName, _object.LastName)
            .Set(u => u.CPF, _object.CPF)
            .Set(u => u.RG, _object.RG)
            .Set(u => u.Address, _object.Address)
            .Set(u => u.City, _object.City)
            .Set(u => u.State, _object.State)
            .Set(u => u.ZipCode, _object.ZipCode)
            .Set(u => u.PhoneNumber, _object.PhoneNumber)
            .Set(u => u.Animais, _object.Animais);

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

        var ob = await FindByEmailAsync(_object.Email, CancellationToken.None) as Responsavel;
        return ob;
    }

    public async Task<bool> RemoveAsync(string _object, CancellationToken cancellationToken)
    {
        var filter = Builders<Responsavel>.Filter.Eq(u => u.Email, _object);
        var result = await _collection.DeleteOneAsync(filter, cancellationToken);
        return result.DeletedCount > 0;  
    }

public async Task<object?> FindByEmailAsync(string modelCredencial, CancellationToken cancellationToken)
    {
        var collection = _dbMongo.GetDatabase().GetCollection<Responsavel>("Responsavel");

        // Create a filter to find the document by Id
        var filter = Builders<Responsavel>.Filter.Eq(u => u.Email, modelCredencial);
        
        // Find the document matching the filter
        var _responsavel = collection.Find(filter).FirstOrDefault();

        return _responsavel as Responsavel;
    }
}