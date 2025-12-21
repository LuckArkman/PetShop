using Data;
using DTOs;
using Interfaces;
using Microsoft.Extensions.Configuration;
using MongoDB.Driver;

namespace Services;

public class RitmoCircadianoService : IRitmoCircadianoService
{
    private readonly IConfiguration _configuration;
    protected IMongoCollection<RitmoCircadiano> _collection;
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
        _collection = _mongoDatabase.GetCollection<RitmoCircadiano>(_collectionName);
    }
    public async Task<object?> GetObject(string _object, CancellationToken cancellationToken)
    {
        var filter = Builders<RitmoCircadiano>.Filter.Eq(u => u.AnimalId, _object);

        // Find the document matching the filter
        var character = _collection.Find(filter).FirstOrDefault();

        return character as RitmoCircadiano;
    }

    public async Task<object?> InsetObject(RitmoCircadiano _object, CancellationToken cancellationToken)
    {
        await _collection.InsertOneAsync(_object);
        return _object as RitmoCircadiano;
    }

    public async Task<object?> UpdateObject(RitmoCircadiano _object, CancellationToken cancellationToken)
    {
        var filter = Builders<RitmoCircadiano>.Filter.Eq(u => u.Id, _object.Id);
        var update = Builders<RitmoCircadiano>.Update
            .Set(u => u.Id, _object.Id)
            .Set(u => u.AnimalId, _object.AnimalId)
            .Set(u => u.Data, _object.Data)
            .Set(u => u.SonoInicio, _object.SonoInicio)
            .Set(u => u.SonoFim, _object.SonoFim)
            .Set(u => u.Sonecas,  _object.Sonecas)
            .Set(u => u.Refeicoes, _object.Refeicoes)
            .Set(u => u.AtividadeManha, _object.AtividadeManha)
            .Set(u => u.AtividadeTarde, _object.AtividadeTarde)
            .Set(u => u.AtividadeNoite, _object.AtividadeNoite)
            .Set(u => u.LuzManhaMin, _object.LuzManhaMin)
            .Set(u => u.TemperaturaAmbienteC, _object.TemperaturaAmbienteC)
            .Set(u => u.AguaMl,  _object.AguaMl)
            .Set(u => u.Observacoes, _object.Observacoes)
            .Set(u => u.SonoNoturnoTotal, _object.SonoNoturnoTotal)
            .Set(u => u.SonecasTotal, _object.SonecasTotal)
            .Set(u => u.SonoTotal, _object.SonoTotal);

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
        var ob = await GetObject(_object.Id, CancellationToken.None) as RitmoCircadiano;
        return ob;
    }

    public Task RemoveObject(object _object, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }
}