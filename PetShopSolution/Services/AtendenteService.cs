using Data;
using DTOs;
using Interfaces;
using Microsoft.Extensions.Configuration;
using MongoDB.Driver;

namespace Services;

public class AtendenteService : IAtendenteService
{
    public MongoDataController _db { get; set; }
    private IMongoCollection<Atendente> _collection;
    public string _collectionName { get; set; }
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
        _collection = _mongoDatabase.GetCollection<Atendente>(_collectionName);
    }

    public async Task<List<Atendente>?> GetAllAtendente(CancellationToken cancellationToken)
    {
        // Busca todos os animais
        var _objts = await _collection
            .Find(Builders<Atendente>.Filter.Empty)
            .ToListAsync(cancellationToken);

        return _objts;
    }

    public async Task<Atendente?> GetObject(string mail, CancellationToken cancellationToken)
    {
        var filter = Builders<Atendente>.Filter.Eq(u => u.email, mail);

        // Find the document matching the filter
        var _Atendente = _collection.Find(filter).FirstOrDefault();

        return _Atendente as Atendente;
    }

    public async Task<Atendente?> GetAtendenteRG(string _rg, CancellationToken cancellationToken)
    {

        // Create a filter to find the document by Id
        var filter = Builders<Atendente>.Filter.Eq(u => u.RG, _rg);

        // Find the document matching the filter
        var _responsavel = _collection.Find(filter).FirstOrDefault();

        return _responsavel as Atendente;
    }

    public async Task<Atendente?> InsetObject(Atendente _object, CancellationToken cancellationToken)
    {
        var collection = _db.GetDatabase().GetCollection<Atendente>("Atendente");
        // Insert the user object into the collection
        collection.InsertOne(_object);
        return _object as Atendente;
    }

    public async Task<Atendente?> UpdateObject(Atendente _object, CancellationToken cancellationToken)
    {
        if (_object.email != null)
        {
            var obj = await FindByEmailAsync(_object.email, CancellationToken.None) as Atendente;
        }

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
        var result = _collection.UpdateOne(filter, update);

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

    public async Task<Atendente?> FindByEmailAsync(string modelCredencial, CancellationToken cancellationToken)
    {
        var collection = _db.GetDatabase().GetCollection<Atendente>("Atendente");
        // Create a filter to find the document by Id
        var filter = Builders<Atendente>.Filter.Eq(u => u.email, modelCredencial);
        // Find the document matching the filter
        var _atendente = collection.Find(filter).FirstOrDefault();
        return _atendente as Atendente;
    }
}