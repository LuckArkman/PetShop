using Data;
using DTOs;
using Interfaces;
using Microsoft.Extensions.Configuration;
using MongoDB.Driver;

namespace Services;

public class MedicacaoService : BaseMongoService<Medicacao>, IMedicacaoService
{
    public MedicacaoService(ITenantService tenantService, IConfiguration configuration)
        : base(tenantService, configuration)
    {
    }

    public async Task<List<Medicacao>?> GetAllMedicacoes(string _object, CancellationToken cancellationToken)
    {
        try
        {
            var filtro = Builders<Medicacao>.Filter.Eq(m => m.animalId, _object);
            var medicacoes = await GetCollection()
                .Find(filtro)
                .ToListAsync(cancellationToken);

            return medicacoes;
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Erro ao buscar medicações para o animal {_object}: {ex.Message}");
            return new List<Medicacao>();
        }
    }

    public async Task<Medicacao?> GetMedicacao(string _object, CancellationToken cancellationToken)
    {
        var filter = Builders<Medicacao>.Filter.Eq(u => u.Id, _object);
        var character = await GetCollection().Find(filter).FirstOrDefaultAsync(cancellationToken);
        return character;
    }

    public async Task<Medicacao?> InsetMedicacao(Medicacao _object, CancellationToken cancellationToken)
    {
        await GetCollection().InsertOneAsync(_object, cancellationToken: cancellationToken);
        return _object;
    }

    public async Task<Medicacao?> UpdateMedicacao(Medicacao _object, CancellationToken cancellationToken)
    {
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

        var result = await GetCollection().UpdateOneAsync(filter, update, cancellationToken: cancellationToken);

        if (result.ModifiedCount > 0)
        {
            Console.WriteLine("Medicacao updated successfully.");
        }
        else
        {
            return null;
        }

        return await GetMedicacao(_object.Id!, cancellationToken);
    }

    public async Task<bool?> RemoveMedicacao(Medicacao _object, CancellationToken cancellationToken)
    {
        try
        {
            var filtro = Builders<Medicacao>.Filter.Eq(m => m.Id, _object.Id);
            var resultado = await GetCollection().DeleteOneAsync(filtro, cancellationToken);
            return resultado.DeletedCount > 0;
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Erro ao remover a medicação {_object.Id}: {ex.Message}");
            return false;
        }
    }
}