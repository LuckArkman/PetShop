using Data;
using DTOs;
using Interfaces;
using Microsoft.Extensions.Configuration;
using MongoDB.Driver;

namespace Services;

public class AtendimentoService : BaseMongoService<Atendimento>, IAtendimentoService
{
    public AtendimentoService(ITenantService tenantService, IConfiguration configuration)
        : base(tenantService, configuration)
    {
    }

    public async Task<Atendimento?> GetById(string id, CancellationToken cancellationToken)
    {
        var filter = Builders<Atendimento>.Filter.Eq(a => a.id, id);
        return await GetCollection().Find(filter).FirstOrDefaultAsync(cancellationToken);
    }

    public async Task<IEnumerable<Atendimento>> GetByAnimal(string animalId, CancellationToken cancellationToken)
    {
        var filter = Builders<Atendimento>.Filter.Eq(a => a.animalId, animalId);
        return await GetCollection().Find(filter).ToListAsync(cancellationToken);
    }

    public async Task<Atendimento> Create(Atendimento atendimento, CancellationToken cancellationToken)
    {
        await GetCollection().InsertOneAsync(atendimento, cancellationToken: cancellationToken);
        return atendimento;
    }

    public async Task<Atendimento?> Update(Atendimento atendimento, CancellationToken cancellationToken)
    {
        var filter = Builders<Atendimento>.Filter.Eq(a => a.id, atendimento.id);

        var update = Builders<Atendimento>.Update
            .Set(a => a.descricao, atendimento.descricao)
            .Set(a => a.observacoes, atendimento.observacoes)
            .Set(a => a.dataAtendimento, atendimento.dataAtendimento);

        var result = await GetCollection().UpdateOneAsync(filter, update, cancellationToken: cancellationToken);
        return result.ModifiedCount > 0 ? await GetById(atendimento.id!, cancellationToken) : null;
    }

    public async Task<bool> Delete(string id, CancellationToken cancellationToken)
    {
        var result = await GetCollection().DeleteOneAsync(a => a.id == id, cancellationToken);
        return result.DeletedCount > 0;
    }
}
