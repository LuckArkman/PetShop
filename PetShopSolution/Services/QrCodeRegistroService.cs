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
    public QrCodeRegistroService(IConfiguration configuration)
    {
        _cfg = configuration;
        _db = new QrCodeRegistroDB(_cfg["MongoDbSettings:ConnectionString"], "QrCodeRegistro");
        _db.GetOrCreateDatabase();
    }
    public async Task<QrCodeRegistro?> GetObject(string _object, CancellationToken cancellationToken)
    {
        var collection = _db.GetDatabase().GetCollection<QrCodeRegistro>("QrCodeRegistro");
        
        var filter = Builders<QrCodeRegistro>.Filter.Eq(u => u.AnimalId, _object);
        
        var character = collection.Find(filter).FirstOrDefault();

        return character as QrCodeRegistro;
    }

    public async Task<QrCodeRegistro?> InsetObject(QrCodeRegistro _object, CancellationToken cancellationToken)
    {
        var collection = _db.GetDatabase().GetCollection<QrCodeRegistro>("QrCodeRegistro");
        collection.InsertOne(_object);
        return _object as QrCodeRegistro;
    }

    public async Task<QrCodeRegistro?> UpdateObject(QrCodeRegistro _object, CancellationToken cancellationToken)
    {
        var obj = await GetObject(_object.Id, CancellationToken.None) as QrCodeRegistro;
        var collection = _db.GetDatabase().GetCollection<QrCodeRegistro>("QrCodeRegistro");

        // Create a filter to find the document by Id
        var filter = Builders<QrCodeRegistro>.Filter.Eq(u => u.Id, _object.Id);

        var update = Builders<QrCodeRegistro>.Update
            .Set(u => u.Id, _object.Id)
            .Set(u => u.AnimalId, _object.AnimalId)
            .Set(u => u.QrCodeBase64, _object.QrCodeBase64)
            .Set(u => u.DataGeracao, _object.DataGeracao);

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
        var ob = await GetObject(_object.Id, CancellationToken.None) as QrCodeRegistro;
        return ob;
    }

    public Task RemoveObject(QrCodeRegistro _object, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }
}