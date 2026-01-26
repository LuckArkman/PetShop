using Data;
using DTOs;
using Interfaces;
using Microsoft.Extensions.Configuration;
using MongoDB.Driver;

namespace Services;

public class RelatorioClinicoService : IRelatorioClinicoService
{
    private readonly IConfiguration _configuration;
    protected IMongoCollection<Relatorio> _collection;
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
        _collection = _mongoDatabase.GetCollection<Relatorio>(_collectionName);
    }

    public async Task<Relatorio?> GetRelatorio(string _object, CancellationToken cancellationToken)
    {
        var filter = Builders<Relatorio>.Filter.Eq(u => u.Id, _object);
        var character = _collection.Find(filter).FirstOrDefault();
        return character as Relatorio;
    }

    public async Task<Relatorio?> InsetRelatorio(Relatorio _object, CancellationToken cancellationToken)
    {
        await _collection.InsertOneAsync(_object);
        return _object as Relatorio;
    }

    public async Task<Relatorio?> UpdateRelatorio(Relatorio _object, CancellationToken cancellationToken)
    {
        var filter = Builders<Relatorio>.Filter.Eq(u => u.Id, _object.Id);
        var update = Builders<Relatorio>.Update
            .Set(u => u.Id, _object.Id)
            .Set(u => u.animalId, _object.animalId)
            .Set(u => u._data, _object._data)
            .Set(u => u.Sintomas, _object.Sintomas)
            .Set(u => u.Tratamento, _object.Tratamento)
            .Set(u => u.Observacoes, _object.Observacoes)
            .Set(u => u.VeterinarioId, _object.VeterinarioId);

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
        var ob = await GetRelatorio(_object.Id, CancellationToken.None) as Relatorio;
        return ob;
    }

    public async Task<List<Relatorio>?> GetAllRelatorios(string animalId, CancellationToken none)
    {
        try
        {
            var filtro = Builders<Relatorio>.Filter.Eq(m => m.animalId, animalId);
            var _relatorios = await _collection
                .Find(filtro)
                .ToListAsync(none);
            return _relatorios;
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Erro ao buscar Relatorios para o animal {animalId}: {ex.Message}");
            return new List<Relatorio>(); // Evita retornar null
        }
    }

    public async Task<List<Relatorio>?> GetAllVeterinarioRelatorios(string id, CancellationToken none)
    {
        var filtro = Builders<Relatorio>.Filter.Eq(m => m.VeterinarioId, id);
        var _relatorios = await _collection
            .Find(filtro)
            .ToListAsync(none);

        return _relatorios;
    }

    public async Task<bool?> RemoveRelatorio(Relatorio _object, CancellationToken cancellationToken)
    {
        var filter = Builders<Relatorio>.Filter.Eq(a => a.Id, _object.Id);
        var result = await _collection.DeleteOneAsync(filter, cancellationToken);
        return result.DeletedCount > 0;
    }
}