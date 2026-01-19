using Data;
using DTOs;
using Interfaces;
using Microsoft.Extensions.Configuration;
using MongoDB.Driver;

namespace Services;

public class DiagnosticoService : BaseMongoService<Diagnostico>, IDiagnosticoService
{
    public DiagnosticoService(ITenantService tenantService, IConfiguration configuration)
        : base(tenantService, configuration)
    {
    }

    public async Task<Diagnostico?> GetDiagnostico(string _object, CancellationToken cancellationToken)
    {
        var filter = Builders<Diagnostico>.Filter.Eq(u => u.Id, _object);
        var character = await GetCollection().Find(filter).FirstOrDefaultAsync(cancellationToken);
        return character;
    }

    public async Task<Diagnostico?> InsetDiagnostico(Diagnostico _object, CancellationToken cancellationToken)
    {
        await GetCollection().InsertOneAsync(_object, cancellationToken: cancellationToken);
        return _object;
    }

    public async Task<Diagnostico?> UpdateDiagnostico(Diagnostico _object, CancellationToken cancellationToken)
    {
        var filter = Builders<Diagnostico>.Filter.Eq(u => u.Id, _object.Id);
        var update = Builders<Diagnostico>.Update
            .Set(u => u.Id, _object.Id)
            .Set(u => u.animalId, _object.animalId)
            .Set(u => u._data, _object._data)
            .Set(u => u.doencaOuCondicao, _object.doencaOuCondicao)
            .Set(u => u.sintomasObservados, _object.sintomasObservados)
            .Set(u => u.examesSolicitados, _object.examesSolicitados)
            .Set(u => u.condutaTratamento, _object.condutaTratamento)
            .Set(u => u.gravidade, _object.gravidade);

        var result = await GetCollection().UpdateOneAsync(filter, update, cancellationToken: cancellationToken);

        if (result.ModifiedCount > 0)
        {
            Console.WriteLine("Diagnostico updated successfully.");
        }
        else
        {
            return null;
        }

        return await GetDiagnostico(_object.Id!, cancellationToken);
    }

    public async Task<bool?> RemoveDiagnostico(string Id, CancellationToken cancellationToken)
    {
        var filter = Builders<Diagnostico>.Filter.Eq(u => u.Id, Id);
        var result = await GetCollection().DeleteOneAsync(filter, cancellationToken);
        return result.DeletedCount > 0;
    }

    public async Task<List<Diagnostico>?> GetAllRelatorios(string animalId, CancellationToken cancellationToken)
    {
        try
        {
            var filtro = Builders<Diagnostico>.Filter.Eq(m => m.animalId, animalId);
            var _relatorios = await GetCollection()
                .Find(filtro)
                .ToListAsync(cancellationToken);

            return _relatorios;
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Erro ao buscar Relatorios para o Diagnostico {animalId}: {ex.Message}");
            return new List<Diagnostico>();
        }
    }
}