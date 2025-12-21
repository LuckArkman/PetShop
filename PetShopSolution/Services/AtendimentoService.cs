using Data;
using DTOs;
using Interfaces;
using Microsoft.Extensions.Configuration;
using MongoDB.Driver;

namespace Services;

public class AtendimentoService : IAtendimentoService
{
    private MongoDataController _db { get; set; }
    protected IMongoCollection<Atendimento> _collection;
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
        _collection = _mongoDatabase.GetCollection<Atendimento>(_collectionName);
    }

    public async Task<Atendimento?> GetById(string id, CancellationToken cancellationToken)
    {
        var collection = _db.GetDatabase().GetCollection<Atendimento>("Atendimento");
        var filter = Builders<Atendimento>.Filter.Eq(a => a.id, id);
        return await collection.Find(filter).FirstOrDefaultAsync(cancellationToken);
    }

    public async Task<IEnumerable<Atendimento>> GetByAnimal(string animalId, CancellationToken cancellationToken)
    {
        var filter = Builders<Atendimento>.Filter.Eq(a => a.animalId, animalId);
        return await _collection.Find(filter).ToListAsync(cancellationToken);
    }

    public async Task<Atendimento> Create(Atendimento atendimento, CancellationToken cancellationToken)
    {
        await _collection.InsertOneAsync(atendimento, cancellationToken: cancellationToken);
        return atendimento;
    }

    public async Task<Atendimento?> Update(Atendimento atendimento, CancellationToken cancellationToken)
    {
        var filter = Builders<Atendimento>.Filter.Eq(a => a.id, atendimento.id);

        var update = Builders<Atendimento>.Update
            .Set(a => a.descricao, atendimento.descricao)
            .Set(a => a.observacoes, atendimento.observacoes)
            .Set(a => a.dataAtendimento, atendimento.dataAtendimento);

        var result = await _collection.UpdateOneAsync(filter, update, cancellationToken: cancellationToken);
        return result.ModifiedCount > 0 ? await GetById(atendimento.id, cancellationToken) : null;
    }

    public async Task<bool> Delete(string id, CancellationToken cancellationToken)
    {
        var result = await _collection.DeleteOneAsync(a => a.id == id, cancellationToken);
        return result.DeletedCount > 0;
    }
}
