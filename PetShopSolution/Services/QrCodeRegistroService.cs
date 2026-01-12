using Data;
using DTOs;
using Interfaces;
using Microsoft.Extensions.Configuration;
using MongoDB.Driver;

namespace Services;

public class QrCodeRegistroService : IQrCodeRegistroService
{
    private readonly IConfiguration _configuration;
    protected IMongoCollection<QrCodeRegistro> _collection;
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
        _collection = _mongoDatabase.GetCollection<QrCodeRegistro>(_collectionName);
    }
    public async Task<QrCodeRegistro?> GetObject(string _object, CancellationToken cancellationToken)
    {
        var filter = Builders<QrCodeRegistro>.Filter.Eq(u => u.AnimalId, _object);
        var character = _collection.Find(filter).FirstOrDefault();
        return character as QrCodeRegistro;
    }

    public async Task<QrCodeRegistro?> InsetObject(QrCodeRegistro _object, CancellationToken cancellationToken)
    {
        await _collection.InsertOneAsync(_object);
        return _object as QrCodeRegistro;
    }

    public async Task<QrCodeRegistro?> UpdateObject(QrCodeRegistro _object, CancellationToken cancellationToken)
    {
        var filter = Builders<QrCodeRegistro>.Filter.Eq(u => u.Id, _object.Id);
        var update = Builders<QrCodeRegistro>.Update
            .Set(u => u.Id, _object.Id)
            .Set(u => u.AnimalId, _object.AnimalId)
            .Set(u => u.QrCodeBase64, _object.QrCodeBase64)
            .Set(u => u.DataGeracao, _object.DataGeracao);

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
        var ob = await GetObject(_object.Id, CancellationToken.None) as QrCodeRegistro;
        return ob;
    }

    public Task RemoveObject(QrCodeRegistro _object, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }
}