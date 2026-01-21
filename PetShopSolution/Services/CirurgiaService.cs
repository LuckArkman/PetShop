using Data;
using DTOs;
using Interfaces;
using Microsoft.Extensions.Configuration;
using MongoDB.Driver;

namespace Services;

public class CirurgiaService : BaseMongoService<Cirurgia>, ICirurgiaService
{
    public CirurgiaService(ITenantService tenantService, IConfiguration configuration)
        : base(tenantService, configuration)
    {
    }

    public async Task<Cirurgia?> GetCirurgia(string _object, CancellationToken cancellationToken)
    {
        var filter = Builders<Cirurgia>.Filter.Eq(u => u.Id, _object);
        var character = await GetCollection().Find(filter).FirstOrDefaultAsync(cancellationToken);
        return character;
    }

    public async Task<Cirurgia?> InsetCirurgia(Cirurgia _object, CancellationToken cancellationToken)
    {
        await GetCollection().InsertOneAsync(_object, cancellationToken: cancellationToken);
        return _object;
    }

    public async Task<Cirurgia?> UpdateCirurgia(Cirurgia _object, CancellationToken cancellationToken)
    {
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

        var result = await GetCollection().UpdateOneAsync(filter, update, cancellationToken: cancellationToken);

        if (result.ModifiedCount > 0)
        {
            Console.WriteLine("Cirurgia updated successfully.");
        }
        else
        {
            return null;
        }

        return await GetCirurgia(_object.Id!, cancellationToken);
    }

    public async Task<bool?> RemoveCirurgia(string Id, CancellationToken cancellationToken)
    {
        var filter = Builders<Cirurgia>.Filter.Eq(u => u.Id, Id);
        var result = await GetCollection().DeleteOneAsync(filter, cancellationToken);
        return result.DeletedCount > 0;
    }

    public async Task<List<Cirurgia>?> GetAllCirurgias(string animalId, CancellationToken cancellationToken)
    {
        try
        {
            var filtro = Builders<Cirurgia>.Filter.Eq(m => m.animalId, animalId);
            var _relatorios = await GetCollection()
                .Find(filtro)
                .ToListAsync(cancellationToken);

            return _relatorios;
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Erro ao buscar Cirurgias para o animal {animalId}: {ex.Message}");
            return new List<Cirurgia>();
        }
    }
}