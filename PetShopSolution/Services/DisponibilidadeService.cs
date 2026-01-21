using Data;
using DTOs;
using Interfaces;
using Microsoft.Extensions.Configuration;
using MongoDB.Driver;

namespace Services;

public class DisponibilidadeService : BaseMongoService<DiasIndisponiveis>, IDisponibilidadeService
{
    public DisponibilidadeService(ITenantService tenantService, IConfiguration configuration)
        : base(tenantService, configuration)
    {
    }

    public new IMongoCollection<DiasIndisponiveis> GetCollection() => base.GetCollection();

    public async Task<List<DiasIndisponiveis>> GetIndisponiveis(CancellationToken cancellationToken)
    {
        return await GetCollection().Find(_ => true).ToListAsync(cancellationToken);
    }

    public async Task AddIndisponivel(DiasIndisponiveis dia, CancellationToken cancellationToken)
    {
        var existe = await GetCollection()
            .Find(d => d.Data.Date == dia.Data.Date)
            .FirstOrDefaultAsync(cancellationToken);

        if (existe != null)
            throw new Exception($"O dia {dia.Data:dd/MM/yyyy} já está marcado como indisponível.");

        await GetCollection().InsertOneAsync(dia, cancellationToken: cancellationToken);
    }

    public async Task<IEnumerable<Agendamento>> GetByDate(DateTime date, CancellationToken cancellationToken)
    {
        var collection = _mongoDatabase!.GetCollection<Agendamento>("Agendamento");

        var dataInicio = date.Date;
        var dataFim = dataInicio.AddDays(1);

        var filter = Builders<Agendamento>.Filter.Gte(a => a.dataConsulta, dataInicio) &
                     Builders<Agendamento>.Filter.Lt(a => a.dataConsulta, dataFim);

        return await collection.Find(filter).ToListAsync(cancellationToken);
    }

    public async Task RemoverIndisponivel(DateTime data, CancellationToken cancellationToken)
    {
        var filter = Builders<DiasIndisponiveis>.Filter.Eq(d => d.Data, data.Date);
        await GetCollection().DeleteOneAsync(filter, cancellationToken);
    }

    public async Task<List<DiasIndisponiveis>> Getdisponiveis(CancellationToken cancellationToken)
    {
        return await GetCollection().Find(_ => true).ToListAsync(cancellationToken);
    }
}