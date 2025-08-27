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

    public async Task<object?> GetObject(string _object, CancellationToken cancellationToken)
    {
        var collection = _db.GetDatabase().GetCollection<RelatorioClinico>("RelatorioClinico");
        
        var filter = Builders<RelatorioClinico>.Filter.Eq(u => u.Id, _object);
        
        var character = collection.Find(filter).FirstOrDefault();

        return character as RelatorioClinico;
    }

    public async Task<object?> InsetObject(RelatorioClinico _object, CancellationToken cancellationToken)
    {
        var collection = _db.GetDatabase().GetCollection<RelatorioClinico>("RelatorioClinico");
        collection.InsertOne(_object);
        return _object as RelatorioClinico;
    }

    public async Task<object?> UpdateObject(RelatorioClinico _object, CancellationToken cancellationToken)
    {
        var obj = await GetObject(_object.Id, CancellationToken.None) as RelatorioClinico;
        var collection = _db.GetDatabase().GetCollection<RelatorioClinico>("RelatorioClinico");

        // Create a filter to find the document by Id
        var filter = Builders<RelatorioClinico>.Filter.Eq(u => u.Id, _object.Id);

        var update = Builders<RelatorioClinico>.Update
            .Set(u => u.Id, _object.Id)
            .Set(u => u.AnimalId, _object.AnimalId)
            .Set(u => u.ResponsavelId, _object.ResponsavelId)
            .Set(u => u._dataAtendimento, _object._dataAtendimento)
            .Set(u => u.Sintomas, _object.Sintomas)
            .Set(u => u.Diagnostico, _object.Diagnostico)
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
        var ob = await GetObject(_object.Id, CancellationToken.None) as RelatorioClinico;
        return ob;
    }

    public async Task RemoveObject(object _object, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }
}