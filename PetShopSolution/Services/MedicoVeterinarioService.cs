using Data;
using DTOs;
using Interfaces;
using Microsoft.Extensions.Configuration;
using MongoDB.Driver;

namespace Services;

public class MedicoVeterinarioService : IMedicoVeterinarioService
{
    protected IMongoCollection<MedicoVeterinario> _collection;
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
        _collection = _mongoDatabase.GetCollection<MedicoVeterinario>(_collectionName);
    }

    public async Task<List<MedicoVeterinario>?> GetAllMedicoVeterinario(CancellationToken cancellationToken)
    {
        var _objts = await _collection
            .Find(Builders<MedicoVeterinario>.Filter.Empty)
            .ToListAsync(cancellationToken);

        return _objts;
    }

    public async Task<MedicoVeterinario?> GetObject(string _object, CancellationToken cancellationToken)
    {
        var filter = Builders<MedicoVeterinario>.Filter.Eq(u => u.CRMV, _object);

        // Find the document matching the filter
        var character = _collection.Find(filter).FirstOrDefault();

        return character as MedicoVeterinario;
    }

    public async Task<MedicoVeterinario?> InsetObject(MedicoVeterinario _object, CancellationToken cancellationToken)
    {
        await _collection.InsertOneAsync(_object);
        return _object as MedicoVeterinario;
    }

    public async Task<MedicoVeterinario?> UpdateObject(MedicoVeterinario _object, CancellationToken cancellationToken)
    {
        var filter = Builders<MedicoVeterinario>.Filter.Eq(u => u.Id, _object.Id);

        var update = Builders<MedicoVeterinario>.Update
            .Set(u => u.Nome, _object.Nome)
            .Set(u => u.CRMV, _object.CRMV)
            .Set(u => u.Especialidade, _object.Especialidade)
            .Set(u => u.Telefone, _object.Telefone)
            .Set(u => u.Email, _object.Email)
            .Set(u => u.Endereco,  _object.Endereco)
            .Set(u => u.Cidade, _object.Cidade)
            .Set(u => u.Estado, _object.Estado)
            .Set(u => u.CEP, _object.CEP);

        // Perform the update
        var result = await _collection.UpdateOneAsync(filter, update);

        if (result.ModifiedCount > 0)
        {
            Console.WriteLine("User updated successfully.");
        }
        else
        {
            return null;
        }
        var ob = await GetObject(_object.Id, CancellationToken.None) as MedicoVeterinario;
        return ob;
    }

    public async Task<bool> RemoveAsync(object _object, CancellationToken cancellationToken)
    {
        var filter = Builders<MedicoVeterinario>.Filter.Eq(u => u.CRMV, _object);
        var result = await _collection.DeleteOneAsync(filter, cancellationToken);

        return result.DeletedCount > 0;  
    }

    public async Task<MedicoVeterinario?> FindByCRMVAsync(string modelCredencial, CancellationToken cancellationToken)
    {
        var filter = Builders<MedicoVeterinario>.Filter.Eq(u => u.CRMV, modelCredencial);

        // Find the document matching the filter
        var character = _collection.Find(filter).FirstOrDefault();

        return character as MedicoVeterinario;
    }
}