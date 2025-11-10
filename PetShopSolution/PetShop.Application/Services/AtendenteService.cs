using MongoDB.Driver;
using PetShop.Application.Data;
using PetShop.Application.DTOs;
using PetShop.Application.Interfaces;
using PetShop.Application.Singletons;

namespace PetShop.Application.Services;

public class AtendenteService : IAtendenteService
{
    public AtendenteDB _db { get; set; }
    private readonly IMongoCollection<Responsavel> _collection;
    public AtendenteService()
    {
        _db = new AtendenteDB(Singleton.Instance().src, "Atendente");
        _db.GetOrCreateDatabase();
        _collection = _db.GetDatabase().GetCollection<Responsavel>("Atendente");
    }

    public async Task<List<Atendente>?> GetAllAtendente(CancellationToken cancellationToken)
    {
        var collection = _db.GetDatabase().GetCollection<Atendente>("Atendente");

        // Busca todos os animais
        var _objts = await collection
            .Find(Builders<Atendente>.Filter.Empty)
            .ToListAsync(cancellationToken);

        return _objts;
    }

    public async Task<object?> GetObject(string mail, CancellationToken cancellationToken)
    {
        var collection = _db.GetDatabase().GetCollection<Atendente>("Atendente");
        
        var filter = Builders<Atendente>.Filter.Eq(u => u.email, mail);

        // Find the document matching the filter
        var _Atendente = collection.Find(filter).FirstOrDefault();

        return _Atendente as Atendente;
    }

    public async Task<object?> GetAtendenteRG(string _rg, CancellationToken cancellationToken)
    {
        var collection = _db.GetDatabase().GetCollection<Atendente>("Atendente");

        // Create a filter to find the document by Id
        var filter = Builders<Atendente>.Filter.Eq(u => u.RG, _rg);

        // Find the document matching the filter
        var _responsavel = collection.Find(filter).FirstOrDefault();

        return _responsavel as Atendente;
    }

    public async Task<object?> InsetObject(Atendente _object, CancellationToken cancellationToken)
    {
        var collection = _db.GetDatabase().GetCollection<Atendente>("Atendente");
        // Insert the user object into the collection
        collection.InsertOne(_object);
        return _object as Atendente;
    }

    public async Task<object?> UpdateObject(Atendente _object, CancellationToken cancellationToken)
    {
        if (_object.email != null)
        {
            var obj = await FindByEmailAsync(_object.email, CancellationToken.None) as Atendente;
        }

        var collection = _db.GetDatabase().GetCollection<Atendente>("Atendente");

        // Create a filter to find the document by Id
        var filter = Builders<Atendente>.Filter.Eq(u => u.email, _object.email);

        var update = Builders<Atendente>.Update
            .Set(u => u.email, _object.email)
            .Set(u => u.nome, _object.nome)
            .Set(u => u.LastName, _object.LastName)
            .Set(u => u.CPF, _object.CPF)
            .Set(u => u.RG, _object.RG)
            .Set(u => u.Address, _object.Address)
            .Set(u => u.City, _object.City)
            .Set(u => u.State, _object.State)
            .Set(u => u.ZipCode, _object.ZipCode)
            .Set(u => u.PhoneNumber, _object.PhoneNumber);

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

        var ob = await FindByEmailAsync(_object.email, CancellationToken.None) as Atendente;
        return ob;
    }

    public async Task<bool> RemoveAsync(string mail, CancellationToken cancellationToken)
    {
        var collection = _db.GetDatabase().GetCollection<Atendente>("Atendente");
        // Create a filter to find the document by Id
        var filter = Builders<Atendente>.Filter.Eq(u => u.email, mail);
        var result = await collection.DeleteManyAsync(filter, cancellationToken);
        return result.DeletedCount > 0;  
    }

    public async Task<object?> FindByEmailAsync(string modelCredencial, CancellationToken cancellationToken)
    {
        var collection = _db.GetDatabase().GetCollection<Atendente>("Atendente");
        // Create a filter to find the document by Id
        var filter = Builders<Atendente>.Filter.Eq(u => u.email, modelCredencial);
        // Find the document matching the filter
        var _responsavel = collection.Find(filter).FirstOrDefault();
        return _responsavel as Atendente;
    }
}