using Data;
using DTOs;
using Interfaces;
using Microsoft.Extensions.Configuration;
using MongoDB.Driver;

namespace Services;

public class MedicacaoService : IMedicacaoService
{
    private readonly IConfiguration _configuration;
    protected IMongoCollection<Medicacao> _collection;
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
        _collection = _mongoDatabase.GetCollection<Medicacao>(_collectionName);
    }
    public MedicacaoService(IConfiguration configuration)
    {
        _cfg = configuration;
        _db = new MedicacaolDBMongo(_cfg["MongoDbSettings:ConnectionString"], "Medicacao");
        _db.GetOrCreateDatabase();
    }

    public async Task<List<Medicacao>?> GetAllMedicacoes(string _object,CancellationToken cancellationToken)
    {
        try
        {
            var collection = _db.GetDatabase().GetCollection<Medicacao>("Medicacao");

            // Cria filtro para buscar apenas as medicações do animal
            var filtro = Builders<Medicacao>.Filter.Eq(m => m.animalId, _object);

            var medicacoes = await collection
                .Find(filtro)
                .ToListAsync(cancellationToken);

            return medicacoes;
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Erro ao buscar medicações para o animal {_object}: {ex.Message}");
            return new List<Medicacao>(); // Evita retornar null
        }
    }

    public async Task<Medicacao?> GetMedicacao(string _object, CancellationToken cancellationToken)
    {
        var collection = _db.GetDatabase().GetCollection<Medicacao>("Medicacao");
        var filter = Builders<Medicacao>.Filter.Eq(u => u.Id, _object);
        var character = collection.Find(filter).FirstOrDefault();

        return character as Medicacao;
    }

    public async Task<Medicacao?> InsetMedicacao(Medicacao _object, CancellationToken cancellationToken)
    {
        var collection = _db.GetDatabase().GetCollection<Medicacao>("Medicacao");
        // Insert the user object into the collection
        collection.InsertOne(_object);
        return _object as Medicacao;
    }

    public async Task<Medicacao?> UpdateMedicacao(Medicacao _object, CancellationToken cancellationToken)
    {
        if (string.IsNullOrEmpty(_object.Id))
        {
            var obj = await GetMedicacao(_object.Id, CancellationToken.None) as Medicacao;
        }

        var collection = _db.GetDatabase().GetCollection<Medicacao>("Medicacao");

        // Create a filter to find the document by Id
        var filter = Builders<Medicacao>.Filter.Eq(u => u.Id, _object.Id);

        var update = Builders<Medicacao>.Update
            .Set(u => u.Id, _object.Id)
            .Set(u => u.animalId, _object.animalId)
            .Set(u => u.data, _object.data)
            .Set(u => u.nome, _object.nome)
            .Set(u => u.dosagem, _object.dosagem)
            .Set(u => u.duracao, _object.duracao)
            .Set(u => u.indicacao, _object.indicacao)
            .Set(u => u.observacoes, _object.observacoes);

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

        var ob = await GetMedicacao(_object.Id, CancellationToken.None) as Medicacao;
        return ob;
    }

    public async Task<bool?> RemoveMedicacao(Medicacao _object, CancellationToken cancellationToken)
    {
        try
        {
            var collection = _db.GetDatabase().GetCollection<Medicacao>("Medicacao");

            // Filtro para encontrar a medicação pelo ID
            var filtro = Builders<Medicacao>.Filter.Eq(m => m.Id, _object.Id);

            var resultado = await collection.DeleteOneAsync(filtro, cancellationToken);
            return resultado.DeletedCount > 0;
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Erro ao remover a medicação {_object.Id}: {ex.Message}");
            return false;
        }
    }
}