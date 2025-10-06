using MongoDB.Driver;
using PetShop.Application.Data;
using PetShop.Application.DTOs;
using PetShop.Application.Interfaces;
using PetShop.Application.Singletons;

namespace PetShop.Application.Services;

public class AtendimentoService : IAtendimentoService
{
    private readonly AtendimentoDB _db;

    public AtendimentoService()
    {
        _db = new AtendimentoDB(Singleton.Instance()!.src, "Atendimento");
        _db.GetOrCreateDatabase();
    }

    public async Task<Atendimento?> GetById(string id, CancellationToken cancellationToken)
    {
        var collection = _db.GetDatabase().GetCollection<Atendimento>("Atendimento");
        var filter = Builders<Atendimento>.Filter.Eq(a => a.id, id);
        return await collection.Find(filter).FirstOrDefaultAsync(cancellationToken);
    }

    public async Task<IEnumerable<Atendimento>> GetByAnimal(string animalId, CancellationToken cancellationToken)
    {
        var collection = _db.GetDatabase().GetCollection<Atendimento>("Atendimento");
        var filter = Builders<Atendimento>.Filter.Eq(a => a.animalId, animalId);
        return await collection.Find(filter).ToListAsync(cancellationToken);
    }

    public async Task<Atendimento> Create(Atendimento atendimento, CancellationToken cancellationToken)
    {
        var collection = _db.GetDatabase().GetCollection<Atendimento>("Atendimento");
        await collection.InsertOneAsync(atendimento, cancellationToken: cancellationToken);
        return atendimento;
    }

    public async Task<Atendimento?> Update(Atendimento atendimento, CancellationToken cancellationToken)
    {
        var collection = _db.GetDatabase().GetCollection<Atendimento>("Atendimento");
        var filter = Builders<Atendimento>.Filter.Eq(a => a.id, atendimento.id);

        var update = Builders<Atendimento>.Update
            .Set(a => a.descricao, atendimento.descricao)
            .Set(a => a.observacoes, atendimento.observacoes)
            .Set(a => a.dataAtendimento, atendimento.dataAtendimento);

        var result = await collection.UpdateOneAsync(filter, update, cancellationToken: cancellationToken);
        return result.ModifiedCount > 0 ? await GetById(atendimento.id, cancellationToken) : null;
    }

    public async Task<bool> Delete(string id, CancellationToken cancellationToken)
    {
        var collection = _db.GetDatabase().GetCollection<Atendimento>("Atendimento");
        var result = await collection.DeleteOneAsync(a => a.id == id, cancellationToken);
        return result.DeletedCount > 0;
    }
}
