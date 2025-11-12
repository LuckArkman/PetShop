using Microsoft.Extensions.Configuration;
using MongoDB.Driver;
using PetShop.Application.Data;
using PetShop.Application.DTOs;
using PetShop.Application.Interfaces;
using PetShop.Application.Singletons;

namespace PetShop.Application.Services;

public class MedicacaoService : IMedicacaoService
{
    public MedicacaolDBMongo _db { get; set; }
    private readonly IConfiguration _cfg;
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

    public async Task<object?> GetMedicacao(string _object, CancellationToken cancellationToken)
    {
        var collection = _db.GetDatabase().GetCollection<Medicacao>("Medicacao");
        var filter = Builders<Medicacao>.Filter.Eq(u => u.Id, _object);
        var character = collection.Find(filter).FirstOrDefault();

        return character as Medicacao;
    }

    public async Task<object?> InsetMedicacao(Medicacao _object, CancellationToken cancellationToken)
    {
        var collection = _db.GetDatabase().GetCollection<Medicacao>("Medicacao");
        // Insert the user object into the collection
        collection.InsertOne(_object);
        return _object as Medicacao;
    }

    public async Task<object?> UpdateMedicacao(Medicacao _object, CancellationToken cancellationToken)
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

        var ob = await GetMedicacao(_object.Id, CancellationToken.None) as Responsavel;
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