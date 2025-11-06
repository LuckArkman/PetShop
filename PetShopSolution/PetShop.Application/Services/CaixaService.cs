using MongoDB.Driver;
using PetShop.Application.Data;
using PetShop.Application.DTOs;
using PetShop.Application.Enums;
using PetShop.Application.Interfaces;
using PetShop.Application.Singletons;

namespace PetShop.Application.Services;

public class CaixaService : ICaixaService
{
    CaixaDB _db {get; set;}
    public CaixaService()
    {
        _db = new CaixaDB(Singleton.Instance()!.src, "Caixa");
        _db.GetOrCreateDatabase();
    }

    public async Task<Pagamento?> GetById(string id, CancellationToken cancellationToken)
    {
        var collection = _db.GetDatabase().GetCollection<Pagamento>("Caixa");
        var filter = Builders<Pagamento>.Filter.Eq(a => a.id, id);
        return await collection.Find(filter).FirstOrDefaultAsync(cancellationToken);
    }

    public async Task<IEnumerable<Pagamento>?> GetAllTodayPaidsCompletes(DateTime dataConsulta, CancellationToken cancellationToken)
    {
        var collection = _db.GetDatabase().GetCollection<Pagamento>("Caixa");

        var inicioDoDia = dataConsulta.Date;
        var fimDoDia = inicioDoDia.AddDays(1);

        var filtro = Builders<Pagamento>.Filter.And(
            Builders<Pagamento>.Filter.Gte(a => a.createdAt, inicioDoDia),
            Builders<Pagamento>.Filter.Lt(a => a.status, PaidStatus.Complete)
        );

        return await collection.Find(filtro).ToListAsync(cancellationToken);
    }

    public async Task<IEnumerable<Pagamento>?> GetAllTodayPaidsPending(DateTime dataConsulta, CancellationToken cancellationToken)
    {
        var collection = _db.GetDatabase().GetCollection<Pagamento>("Caixa");

        var inicioDoDia = dataConsulta.Date;
        var fimDoDia = inicioDoDia.AddDays(1);

        var filtro = Builders<Pagamento>.Filter.And(
            Builders<Pagamento>.Filter.Gte(a => a.createdAt, inicioDoDia),
            Builders<Pagamento>.Filter.Lt(a => a.status, PaidStatus.pending)
        );

        return await collection.Find(filtro).ToListAsync(cancellationToken);
    }

    public async Task<IEnumerable<Pagamento>?> GetAllTodayPaidsCanceled(DateTime dataConsulta, CancellationToken cancellationToken)
    {
        var collection = _db.GetDatabase().GetCollection<Pagamento>("Caixa");

        var inicioDoDia = dataConsulta.Date;
        var fimDoDia = inicioDoDia.AddDays(1);

        var filtro = Builders<Pagamento>.Filter.And(
            Builders<Pagamento>.Filter.Gte(a => a.createdAt, inicioDoDia),
            Builders<Pagamento>.Filter.Lt(a => a.status, PaidStatus.Canceled)
        );

        return await collection.Find(filtro).ToListAsync(cancellationToken);
    }

    public async Task<Pagamento?> GetByCliente(string cpf, CancellationToken cancellationToken)
    {
        var collection = _db.GetDatabase().GetCollection<Pagamento>("Caixa");
        var filter = Builders<Pagamento>.Filter.Eq(a => a.cpf, cpf);
        return await collection.Find(filter).FirstOrDefaultAsync(cancellationToken);
    }

    public async Task<Pagamento?> Create(Pagamento agendamento, CancellationToken cancellationToken)
    {
        var collection = _db.GetDatabase().GetCollection<Pagamento>("Caixa");
        await collection.InsertOneAsync(agendamento, cancellationToken: cancellationToken);
        return agendamento;
    }

    public async Task<Pagamento?> UpdateStatus(string id, PaidStatus status, CancellationToken cancellationToken)
    {
        var collection = _db.GetDatabase().GetCollection<Pagamento>("Caixa");
        var filter = Builders<Pagamento>.Filter.Eq(a => a.id, id);
        var update = Builders<Pagamento>.Update.Set<PaidStatus>(a => a.status, status);

        var result = await collection.UpdateOneAsync(filter, update, cancellationToken: cancellationToken);
        return result.ModifiedCount > 0 ? await GetById(id, cancellationToken) : null;
    }

    public async Task<bool> DeleteByDateTime(DateTime dataHora, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }

    public async Task<bool> Delete(string id, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }
}