using Data;
using DTOs;
using Interfaces;
using Microsoft.Extensions.Configuration;
using MongoDB.Driver;

namespace Services;

public class Repositorio : BaseMongoService<Order>, IRepositorio<Order>
{
    public Repositorio(ITenantService tenantService, IConfiguration configuration)
        : base(tenantService, configuration)
    {
    }

    public async Task<Order> InsertOneAsync(Order document)
    {
        await GetCollection().InsertOneAsync(document);
        return document;
    }

    public async Task<Order> Update(string order, string status)
    {
        var filter = Builders<Order>.Filter.Eq(p => p.id, order);
        var update = Builders<Order>.Update
            .Set(a => a.Status, status)
            .Set(a => a.updateAt, DateTime.UtcNow);

        await GetCollection().UpdateOneAsync(filter, update);
        return (await GetByIdOrderAsync(order))!;
    }

    public async Task<Order?> GetByIdOrderAsync(string id)
    {
        var filter = Builders<Order>.Filter.Eq(p => p.id, id);
        return await GetCollection().Find(filter).FirstOrDefaultAsync();
    }

    public async Task<Order?> GetByTransectionOrderAsync(string paymentId)
    {
        var filter = Builders<Order>.Filter.Eq(p => p.TransactionId, paymentId);
        return await GetCollection().Find(filter).FirstOrDefaultAsync();
    }

    public async Task<IEnumerable<Order>?> GetAllTodayPaidsCompletes(DateTime dataConsulta, CancellationToken cancellationToken)
    {
        var inicioDia = dataConsulta.Date;
        var fimDia = inicioDia.AddDays(1);

        var filter = Builders<Order>.Filter.And(
            Builders<Order>.Filter.Eq(o => o.Status, "Paid"),
            Builders<Order>.Filter.Gte(o => o.CreatedAt, inicioDia),
            Builders<Order>.Filter.Lt(o => o.CreatedAt, fimDia)
        );

        return await GetCollection().Find(filter).ToListAsync(cancellationToken);
    }

    public async Task<IEnumerable<Order>?> GetAllTodayPaidsPending(DateTime dataConsulta, CancellationToken cancellationToken)
    {
        var inicioDia = dataConsulta.Date;
        var fimDia = inicioDia.AddDays(1);

        var filter = Builders<Order>.Filter.And(
            Builders<Order>.Filter.Eq(o => o.Status, "Pending"),
            Builders<Order>.Filter.Gte(o => o.CreatedAt, inicioDia),
            Builders<Order>.Filter.Lt(o => o.CreatedAt, fimDia)
        );

        return await GetCollection().Find(filter).ToListAsync(cancellationToken);
    }

    public async Task<IEnumerable<Order>?> GetAllTodayPaidsCanceled(DateTime dataConsulta, CancellationToken cancellationToken)
    {
        var inicioDia = dataConsulta.Date;
        var fimDia = inicioDia.AddDays(1);

        var filter = Builders<Order>.Filter.And(
            Builders<Order>.Filter.Eq(o => o.Status, "Canceled"),
            Builders<Order>.Filter.Gte(o => o.CreatedAt, inicioDia),
            Builders<Order>.Filter.Lt(o => o.CreatedAt, fimDia)
        );

        return await GetCollection().Find(filter).ToListAsync(cancellationToken);
    }

    public async Task<bool> Delete(string id, CancellationToken cancellationToken)
    {
        var filter = Builders<Order>.Filter.Eq(p => p.id, id);
        var result = await GetCollection().DeleteOneAsync(filter, cancellationToken);
        return result.DeletedCount > 0;
    }

    public async Task<Order?> GetByUserIdOrderAsync(string responsavelId)
    {
        var filter = Builders<Order>.Filter.Eq(p => p.UserId, responsavelId);
        return await GetCollection().Find(filter).FirstOrDefaultAsync();
    }
}