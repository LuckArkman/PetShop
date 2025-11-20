using Data;
using DTOs;
using Enums;
using Interfaces;
using Microsoft.Extensions.Configuration;
using MongoDB.Driver;

namespace Services;

public class CaixaService : ICaixaService
{
    CaixaDB _db {get; set;}
    private readonly IConfiguration _cfg;
    public CaixaService(IConfiguration configuration)
    {
        _cfg = configuration;
        _db = new CaixaDB(_cfg["MongoDbSettings:ConnectionString"], "Caixa");
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
            Builders<Pagamento>.Filter.Lt(a => a.Status, PaidStatus.Complete)
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
            Builders<Pagamento>.Filter.Lt(a => a.Status, PaidStatus.pending)
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
            Builders<Pagamento>.Filter.Lt(a => a.Status, PaidStatus.Canceled)
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

    public async Task<Pagamento?> UpdateStatusWebhook(long id, PaidStatus status, CancellationToken cancellationToken)
    {
        var collection = _db.GetDatabase().GetCollection<Pagamento>("Caixa");
        var filter = Builders<Pagamento>.Filter.Eq(a => a.payment, id);
        var update = Builders<Pagamento>.Update.Set<PaidStatus>(a => a.Status, status);

        var result = await collection.UpdateOneAsync(filter, update, cancellationToken: cancellationToken);
        return result.ModifiedCount > 0 ? await GetPaymentId(id, cancellationToken) : null;
    }

    public async Task<Pagamento?> UpdateStatus(string id, PaidStatus status, CancellationToken cancellationToken)
    {
        var collection = _db.GetDatabase().GetCollection<Pagamento>("Caixa");
        var filter = Builders<Pagamento>.Filter.Eq(a => a.id, id);
        var update = Builders<Pagamento>.Update.Set<PaidStatus>(a => a.Status, status);

        var result = await collection.UpdateOneAsync(filter, update, cancellationToken: cancellationToken);
        return result.ModifiedCount > 0 ? await GetById(id, cancellationToken) : null;
    }

    public async Task<Pagamento?> GetPaymentId(long id, CancellationToken cancellationToken)
    {
        var collection = _db.GetDatabase().GetCollection<Pagamento>("Caixa");
        var filter = Builders<Pagamento>.Filter.Eq(a => a.payment, id);
        return await collection.Find(filter).FirstOrDefaultAsync(cancellationToken);
    }

    public async Task<bool> Delete(string id, CancellationToken cancellationToken)
    {
        var collection = _db.GetDatabase().GetCollection<Pagamento>("Caixa");
        // Create a filter to find the document by Id
        var filter = Builders<Pagamento>.Filter.Eq(u => u.id, id);
        var result = await collection.DeleteManyAsync(filter, cancellationToken);
        return result.DeletedCount > 0;  
    }
}