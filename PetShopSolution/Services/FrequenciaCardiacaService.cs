using Data;
using DTOs;
using Interfaces;
using Microsoft.Extensions.Configuration;
using MongoDB.Driver;

namespace Services;

public class FrequenciaCardiacaService : IFrequenciaCardiaca
{
    private readonly IConfiguration _configuration;
    protected IMongoCollection<FrequenciaCardiaca> _collection;
    private readonly IConfiguration _cfg;
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
        _collection = _mongoDatabase.GetCollection<FrequenciaCardiaca>(_collectionName);
    }

    public async Task<FrequenciaCardiaca?> GetObject(string _object, CancellationToken cancellationToken)
    {
        var filter = Builders<FrequenciaCardiaca>.Filter.Eq(u => u.AnimalId, _object);
        var character = _collection.Find(filter).FirstOrDefault();
        return character as FrequenciaCardiaca;
    }

    public async Task<FrequenciaCardiaca?> InsetObject(FrequenciaCardiaca _object, CancellationToken cancellationToken)
    {
        await _collection.InsertOneAsync(_object);
        return _object as FrequenciaCardiaca;
    }

    public async Task<FrequenciaCardiaca?> UpdateObject(FrequenciaCardiaca _object, CancellationToken cancellationToken)
    {
        var filter = Builders<FrequenciaCardiaca>.Filter.Eq(u => u.Id, _object.Id);

        var update = Builders<FrequenciaCardiaca>.Update
            .Set(u => u.Id, _object.Id)
            .Set(u => u.AnimalId, _object.AnimalId)
            .Set(u => u.Horario, _object.Horario)
            .Set(u => u.Peso, _object.Peso)
            .Set(u => u.Especie, _object.Especie)
            .Set(u => u.Raca, _object.Raca)
            .Set(u => u.Porte, _object.Porte)
            .Set(u => u.BatimentosPorMinuto, _object.BatimentosPorMinuto)
            .Set(u => u.Observacao, _object.Observacao);

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
        var ob = await GetObject(_object.Id, CancellationToken.None) as FrequenciaCardiaca;
        return ob;
    }

    public Task RemoveObject(object _object, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }
}