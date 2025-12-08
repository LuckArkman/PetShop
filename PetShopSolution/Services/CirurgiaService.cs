using Data;
using DTOs;
using Interfaces;
using Microsoft.Extensions.Configuration;
using MongoDB.Driver;

namespace Services;

public class CirurgiaService : ICirurgiaService
{
    public CirurgiaDBMongo _db { get; set; }
    private readonly IConfiguration _cfg;
    public CirurgiaService(IConfiguration configuration)
    {
        _cfg = configuration;
        _db = new CirurgiaDBMongo(_cfg["MongoDbSettings:ConnectionString"], "Cirurgia");
        _db.GetOrCreateDatabase();
    }

    public async Task<Cirurgia?> GetCirurgia(string _object, CancellationToken cancellationToken)
    {
        var collection = _db.GetDatabase().GetCollection<Cirurgia>("Cirurgia");
        var filter = Builders<Cirurgia>.Filter.Eq(u => u.Id, _object);
        var character = collection.Find(filter).FirstOrDefault();
        return character as Cirurgia;
    }

    public async Task<Cirurgia?> InsetCirurgia(Cirurgia _object, CancellationToken cancellationToken)
    {
        var collection = _db.GetDatabase().GetCollection<Cirurgia>("Cirurgia");
        collection.InsertOne(_object);
        return _object as Cirurgia;
    }

    public async Task<Cirurgia?> UpdateCirurgia(Cirurgia _object, CancellationToken cancellationToken)
    {
        var obj = await GetCirurgia(_object.Id, CancellationToken.None) as Cirurgia;
        var collection = _db.GetDatabase().GetCollection<Cirurgia>("Cirurgia");

        // Create a filter to find the document by Id
        var filter = Builders<Cirurgia>.Filter.Eq(u => u.Id, _object.Id);

        var update = Builders<Cirurgia>.Update
            .Set(u => u.Id, _object.Id)
            .Set(u => u.animalId, _object.animalId)
            .Set(u => u.data, _object.data)
            .Set(u => u.tipo, _object.tipo)
            .Set(u => u.motivo, _object.motivo)
            .Set(u => u.procedimentoRealizado, _object.procedimentoRealizado)
            .Set(u => u.relatorio, _object.relatorio)
            .Set(u => u.posOperatorioAcompanhamento, _object.posOperatorioAcompanhamento)
            .Set(u => u.dataAlta, _object.dataAlta);

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
        var ob = await GetCirurgia(_object.Id, CancellationToken.None) as Cirurgia;
        return ob;
    }

    public async Task<bool?> RemoveCirurgia(string Id, CancellationToken cancellationToken)
    {
        var collection = _db.GetDatabase().GetCollection<Cirurgia>("Cirurgia");
        // Cria filtro para encontrar o documento pelo id
        var filter = Builders<Cirurgia>.Filter.Eq(u => u.Id, Id);
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

    public async Task<List<Cirurgia>?> GetAllCirurgias(string animalId, CancellationToken none)
    {
        try
        {
            var collection = _db.GetDatabase().GetCollection<Cirurgia>("Cirurgia");

            // Cria filtro para buscar apenas as medicações do animal
            var filtro = Builders<Cirurgia>.Filter.Eq(m => m.animalId, animalId);

            var _relatorios = await collection
                .Find(filtro)
                .ToListAsync(none);

            return _relatorios;
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Erro ao buscar Relatorios para o Diagnostico {animalId}: {ex.Message}");
            return new List<Cirurgia>(); // Evita retornar null
        }
    }
}