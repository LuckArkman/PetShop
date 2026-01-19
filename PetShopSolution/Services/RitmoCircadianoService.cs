using Data;
using DTOs;
using Interfaces;
using Microsoft.Extensions.Configuration;
using MongoDB.Driver;

namespace Services;

public class RitmoCircadianoService : BaseMongoService<RitmoCircadiano>, IRitmoCircadianoService
{
    public RitmoCircadianoService(ITenantService tenantService, IConfiguration configuration)
        : base(tenantService, configuration)
    {
    }

    public async Task<object?> GetObject(string _object, CancellationToken cancellationToken)
    {
        var filter = Builders<RitmoCircadiano>.Filter.Eq(u => u.AnimalId, _object);
        var character = await GetCollection().Find(filter).FirstOrDefaultAsync(cancellationToken);
        return character;
    }

    public async Task<object?> InsetObject(RitmoCircadiano _object, CancellationToken cancellationToken)
    {
        await GetCollection().InsertOneAsync(_object, cancellationToken: cancellationToken);
        return _object;
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
            .Set(u => u.Sonecas, _object.Sonecas)
            .Set(u => u.Refeicoes, _object.Refeicoes)
            .Set(u => u.AtividadeManha, _object.AtividadeManha)
            .Set(u => u.AtividadeTarde, _object.AtividadeTarde)
            .Set(u => u.AtividadeNoite, _object.AtividadeNoite)
            .Set(u => u.LuzManhaMin, _object.LuzManhaMin)
            .Set(u => u.TemperaturaAmbienteC, _object.TemperaturaAmbienteC)
            .Set(u => u.AguaMl, _object.AguaMl)
            .Set(u => u.Observacoes, _object.Observacoes)
            .Set(u => u.SonoNoturnoTotal, _object.SonoNoturnoTotal)
            .Set(u => u.SonecasTotal, _object.SonecasTotal)
            .Set(u => u.SonoTotal, _object.SonoTotal);

        var result = await GetCollection().UpdateOneAsync(filter, update, cancellationToken: cancellationToken);

        if (result.ModifiedCount > 0)
        {
            Console.WriteLine("RitmoCircadiano updated successfully.");
        }
        else
        {
            return null;
        }

        return await GetObject(_object.AnimalId!, cancellationToken);
    }

    public Task RemoveObject(object _object, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }
}