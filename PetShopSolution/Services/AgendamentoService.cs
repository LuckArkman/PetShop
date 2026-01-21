using System.Collections;
using Data;
using DTOs;
using Enums;
using Interfaces;
using Microsoft.Extensions.Configuration;
using MongoDB.Driver;

namespace Services;

public class AgendamentoService : BaseMongoService<Agendamento>, IAgendamentoService
{
    public AgendamentoService(ITenantService tenantService, IConfiguration configuration)
        : base(tenantService, configuration)
    {
    }

    public async Task<Agendamento?> GetById(string id, CancellationToken cancellationToken)
    {
        var filter = Builders<Agendamento>.Filter.Eq(a => a.id, id);
        return await GetCollection().Find(filter).FirstOrDefaultAsync(cancellationToken);
    }

    public async Task<IEnumerable<Agendamento>> GetByCliente(string rg, CancellationToken cancellationToken)
    {
        var filter = Builders<Agendamento>.Filter.Eq(a => a.rg, rg);
        return await GetCollection().Find(filter).ToListAsync(cancellationToken);
    }

    public async Task<Agendamento> Create(Agendamento agendamento, CancellationToken cancellationToken)
    {
        await GetCollection().InsertOneAsync(agendamento, cancellationToken: cancellationToken);
        return agendamento;
    }

    public async Task<Agendamento?> UpdateStatus(string id, Status status, CancellationToken cancellationToken)
    {
        var filter = Builders<Agendamento>.Filter.Eq(a => a.id, id);
        var update = Builders<Agendamento>.Update.Set<Status>(a => a.status, status);

        var result = await GetCollection().UpdateOneAsync(filter, update, cancellationToken: cancellationToken);
        return result.ModifiedCount > 0 ? await GetById(id, cancellationToken) : null;
    }

    public async Task<bool> Delete(string id, CancellationToken cancellationToken)
    {
        var result = await GetCollection().DeleteOneAsync(a => a.id == id, cancellationToken);
        return result.DeletedCount > 0;
    }

    public async Task<bool> DeleteByDateTime(DateTime dataHora, CancellationToken cancellationToken)
    {
        var filter = Builders<Agendamento>.Filter.Eq(a => a.dataConsulta, dataHora);
        var result = await GetCollection().DeleteOneAsync(filter, cancellationToken);
        return result.DeletedCount > 0;
    }

    public async Task<IEnumerable<Agendamento>> GetByVeterinario(string crmv, CancellationToken cancellationToken)
    {
        var filter = Builders<Agendamento>.Filter.Eq(a => a.crmv, crmv) &
                     Builders<Agendamento>.Filter.Ne(a => a.status, Status.Cancelado);

        return await GetCollection().Find(filter).ToListAsync(cancellationToken);
    }

    public async Task<List<Agendamento>> GetByDate(DateTime dataConsulta, CancellationToken cancellationToken)
    {
        var inicioDoDia = dataConsulta.Date;
        var fimDoDia = inicioDoDia.AddDays(1);

        var filtro = Builders<Agendamento>.Filter.And(
            Builders<Agendamento>.Filter.Gte(a => a.dataConsulta, inicioDoDia),
            Builders<Agendamento>.Filter.Lt(a => a.dataConsulta, fimDoDia)
        );

        return await GetCollection().Find(filtro).ToListAsync(cancellationToken);
    }
}
