using MongoDB.Driver;
using PetShop.Application.Data;
using PetShop.Application.DTOs;
using PetShop.Application.Interfaces;
using PetShop.Application.Singletons;

namespace PetShop.Application.Services;

public class RelatorioClinicoService : IRelatorioClinicoService
{
    public RelatorioClinicoDB _db { get; set; }

    public RelatorioClinicoService()
    {
        _db = new RelatorioClinicoDB(Singleton.Instance().src, "RelatorioClinico");
        _db.GetOrCreateDatabase();
    }

    public async Task<object?> GetRelatorio(string _object, CancellationToken cancellationToken)
    {
        var collection = _db.GetDatabase().GetCollection<Relatorio>("RelatorioClinico");
        
        var filter = Builders<Relatorio>.Filter.Eq(u => u.Id, _object);
        
        var character = collection.Find(filter).FirstOrDefault();

        return character as Relatorio;
    }

    public async Task<object?> InsetRelatorio(Relatorio _object, CancellationToken cancellationToken)
    {
        var collection = _db.GetDatabase().GetCollection<Relatorio>("RelatorioClinico");
        collection.InsertOne(_object);
        return _object as Relatorio;
    }

    public async Task<object?> UpdateRelatorio(Relatorio _object, CancellationToken cancellationToken)
    {
        var obj = await GetRelatorio(_object.Id, CancellationToken.None) as Relatorio;
        var collection = _db.GetDatabase().GetCollection<Relatorio>("RelatorioClinico");

        // Create a filter to find the document by Id
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
        var result = collection.UpdateOne(filter, update);

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
            var collection = _db.GetDatabase().GetCollection<Relatorio>("RelatorioClinico");

            // Cria filtro para buscar apenas as medicações do animal
            var filtro = Builders<Relatorio>.Filter.Eq(m => m.animalId, animalId);

            var _relatorios = await collection
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

    public async Task<bool?> RemoveRelatorio(Relatorio _object, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }
}