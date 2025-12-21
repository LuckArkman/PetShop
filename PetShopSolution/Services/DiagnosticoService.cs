using Data;
using DTOs;
using Interfaces;
using Microsoft.Extensions.Configuration;
using MongoDB.Driver;

namespace Services;

public class DiagnosticoService : IDiagnosticoService
{
    private readonly IConfiguration _configuration;
    protected IMongoCollection<Diagnostico> _collection;
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
        _collection = _mongoDatabase.GetCollection<Diagnostico>(_collectionName);
    }
    public DiagnosticoService(IConfiguration configuration)
    {
        _cfg = configuration;
        _db = new DiagnosticoDBMongo(_cfg["MongoDbSettings:ConnectionString"], "Diagnostico");
        _db.GetOrCreateDatabase();
    }

    public async Task<Diagnostico?> GetDiagnostico(string _object, CancellationToken cancellationToken)
    {
        var collection = _db.GetDatabase().GetCollection<Diagnostico>("Diagnostico");
        var filter = Builders<Diagnostico>.Filter.Eq(u => u.Id, _object);
        var character = collection.Find(filter).FirstOrDefault();
        return character as Diagnostico;
    }

    public async Task<Diagnostico?> InsetDiagnostico(Diagnostico _object, CancellationToken cancellationToken)
    {
        var collection = _db.GetDatabase().GetCollection<Diagnostico>("Diagnostico");
        collection.InsertOne(_object);
        return _object as Diagnostico;
    }

    public async Task<Diagnostico?> UpdateDiagnostico(Diagnostico _object, CancellationToken cancellationToken)
    {
        var obj = await GetDiagnostico(_object.Id, CancellationToken.None) as Diagnostico;
        var collection = _db.GetDatabase().GetCollection<Diagnostico>("Diagnostico");

        // Create a filter to find the document by Id
        var filter = Builders<Diagnostico>.Filter.Eq(u => u.Id, _object.Id);

        var update = Builders<Diagnostico>.Update
            .Set(u => u.Id, _object.Id)
            .Set(u => u.animalId, _object.animalId)
            .Set(u => u._data, _object._data)
            .Set(u => u.doencaOuCondicao, _object.doencaOuCondicao)
            .Set(u => u.sintomasObservados, _object.sintomasObservados)
            .Set(u => u.examesSolicitados, _object.examesSolicitados)
            .Set(u => u.condutaTratamento, _object.condutaTratamento)
            .Set(u => u.gravidade, _object.gravidade);

        // Perform the update
        var result = collection.UpdateOne(filter, update);

        if (result.ModifiedCount > 0)
        {
            Console.WriteLine("Diagnostico updated successfully.");
        }
        else
        {
            return null;
        }
        var ob = await GetDiagnostico(_object.Id, CancellationToken.None) as Diagnostico;
        return ob;
    }

    public async Task<bool?> RemoveDiagnostico(string Id, CancellationToken cancellationToken)
    {
        var collection = _db.GetDatabase().GetCollection<Diagnostico>("Diagnostico");
        // Cria filtro para encontrar o documento pelo id
        var filter = Builders<Diagnostico>.Filter.Eq(u => u.Id, Id);

        // Remove o documento
        var result = await collection.DeleteOneAsync(filter, cancellationToken);

        if (result.DeletedCount > 0)
        {
            Console.WriteLine($"Diagnostico com id {Id} removido com sucesso.");
            return true;
        }
        else
        {
            Console.WriteLine($"Nenhum Diagnostico encontrado com id {Id}.");
        }

        return false;
    }

    public async Task<List<Diagnostico>?> GetAllRelatorios(string animalId, CancellationToken none)
    {
        try
        {
            var collection = _db.GetDatabase().GetCollection<Diagnostico>("Diagnostico");

            // Cria filtro para buscar apenas as medicações do animal
            var filtro = Builders<Diagnostico>.Filter.Eq(m => m.animalId, animalId);

            var _relatorios = await collection
                .Find(filtro)
                .ToListAsync(none);

            return _relatorios;
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Erro ao buscar Relatorios para o Diagnostico {animalId}: {ex.Message}");
            return new List<Diagnostico>(); // Evita retornar null
        }
    }
}