using MongoDB.Driver;
using PetShop.Application.Data;
using PetShop.Application.DTOs;
using PetShop.Application.Interfaces;
using PetShop.Application.Singletons;

namespace PetShop.Application.Services;

public class RitmoCircadianoService : IRitmoCircadianoService
{
    public RitmoCircadianoDB _db { get; set; } 
    public RitmoCircadianoService()
    {
        _db = new RitmoCircadianoDB(Singleton.Instance().src, "RitmoCircadiano");
        _db.GetOrCreateDatabase();
    }
    public async Task<object?> GetObject(string _object, CancellationToken cancellationToken)
    {
        var collection = _db.GetDatabase().GetCollection<RitmoCircadiano>("RitmoCircadiano");

        // Create a filter to find the document by Id
        var filter = Builders<RitmoCircadiano>.Filter.Eq(u => u.AnimalId, _object);

        // Find the document matching the filter
        var character = collection.Find(filter).FirstOrDefault();

        return character as RitmoCircadiano;
    }

    public async Task<object?> InsetObject(RitmoCircadiano _object, CancellationToken cancellationToken)
    {
        var collection = _db.GetDatabase().GetCollection<RitmoCircadiano>("RitmoCircadiano");
        // Insert the user object into the collection
        collection.InsertOne(_object);
        return _object as RitmoCircadiano;
    }

    public async Task<object?> UpdateObject(RitmoCircadiano _object, CancellationToken cancellationToken)
    {
        var obj = await GetObject(_object.Id, CancellationToken.None) as RitmoCircadiano;
        var collection = _db.GetDatabase().GetCollection<RitmoCircadiano>("RitmoCircadiano");

        // Create a filter to find the document by Id
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
        var result = collection.UpdateOne(filter, update);

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