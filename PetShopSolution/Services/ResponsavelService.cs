using Data;
using DTOs;
using Interfaces;
using Microsoft.Extensions.Configuration;
using MongoDB.Driver;

namespace Services;

public class ResponsavelService : IResponsavelService
{
    private readonly IConfiguration _configuration;
    protected IMongoCollection<Responsavel> _collection;
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
        _collection = _mongoDatabase.GetCollection<Responsavel>(_collectionName);
    }

    public async Task<List<Responsavel>?> GetAllResponsavel(CancellationToken cancellationToken)
    {
        // Busca todos os animais
        var _objts = await _collection
            .Find(Builders<Responsavel>.Filter.Empty)
            .ToListAsync(cancellationToken);

        return _objts;
    }

    public async Task<Responsavel?> GetObject(string mail, CancellationToken cancellationToken)
    {
        var filter = Builders<Responsavel>.Filter.Eq(u => u.Email, mail);
        var _responsavel = _collection.Find(filter).FirstOrDefault();
        return _responsavel as Responsavel;
    }
    
    public async Task<Responsavel?> GetResponsavelId(string _rg, CancellationToken cancellationToken)
    {
        var filter = Builders<Responsavel>.Filter.Eq(u => u.RG, _rg);
        var _responsavel = _collection.Find(filter).FirstOrDefault();
        return _responsavel as Responsavel;
    }

    public async Task<Responsavel?> InsetObject(Responsavel _object, CancellationToken cancellationToken)
    {
        await _collection.InsertOneAsync(_object);
        return _object as Responsavel;
    }

    public async Task<Responsavel?> UpdateObject(Responsavel _object, CancellationToken cancellationToken)
    {
        if (_object.Email != null)
        {
            var obj = await FindByEmailAsync(_object.Email, CancellationToken.None) as Responsavel;
        }
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
        var result = _collection.UpdateOne(filter, update);

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

    public async Task<Responsavel?> FindByEmailAsync(string modelCredencial, CancellationToken cancellationToken)
    {
        var filter = Builders<Responsavel>.Filter.Eq(u => u.Email, modelCredencial);
        var _responsavel = _collection.Find(filter).FirstOrDefault();
        return _responsavel as Responsavel;
    }

    public async Task<List<Responsavel>?> GetAllResponsaveis(ICollection<string> resResponsaveis, CancellationToken none)
    {
        var filter = Builders<Responsavel>.Filter.In(a => a.Id, resResponsaveis);
        var responsaveis = await _collection
            .Find(filter)
            .ToListAsync(none);

        return responsaveis;
    }

    public async Task<Responsavel?> GetResponsavelRg(string? agendamentoRg)
    {
        var filter = Builders<Responsavel>.Filter.Eq(u => u.RG, agendamentoRg);
        var _responsavel = _collection.Find(filter).FirstOrDefault();
        return _responsavel as Responsavel;
    }
}