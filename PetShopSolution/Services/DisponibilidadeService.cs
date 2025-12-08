using Data;
using DTOs;
using Interfaces;
using Microsoft.Extensions.Configuration;
using MongoDB.Driver;

namespace Services;

public class DisponibilidadeService : IDisponibilidadeService
{
    private readonly DisponibilidadeDB _db;
    private readonly IConfiguration _cfg;

    public DisponibilidadeService(IConfiguration configuration)
    {
        _cfg = configuration;
        _db = new DisponibilidadeDB(_cfg["MongoDbSettings:ConnectionString"], "Disponibilidade");
        _db.GetOrCreateDatabase();
    }

    public IMongoCollection<DiasIndisponiveis> GetCollection() =>
        _db.GetDatabase().GetCollection<DiasIndisponiveis>("Disponibilidade");
    
    public async Task<List<DiasIndisponiveis>> GetIndisponiveis(CancellationToken cancellationToken)
    {
        var collection = GetCollection();
        return await collection.Find(_ => true).ToListAsync(cancellationToken);
    }
    public async Task AddIndisponivel(DiasIndisponiveis dia, CancellationToken cancellationToken)
    {
        var collection = GetCollection();

        var existe = await collection
            .Find(d => d.Data.Date == dia.Data.Date)
            .FirstOrDefaultAsync(cancellationToken);

        if (existe != null)
            throw new Exception($"O dia {dia.Data:dd/MM/yyyy} já está marcado como indisponível.");

        await collection.InsertOneAsync(dia, cancellationToken: cancellationToken);
    }
    
    public async Task<IEnumerable<Agendamento>> GetByDate(DateTime date, CancellationToken cancellationToken)
    {
        var collection = _db.GetDatabase().GetCollection<Agendamento>("Agendamento");

        var dataInicio = date.Date;
        var dataFim = dataInicio.AddDays(1);

        var filter = Builders<Agendamento>.Filter.Gte(a => a.dataConsulta, dataInicio) &
                     Builders<Agendamento>.Filter.Lt(a => a.dataConsulta, dataFim);

        return await collection.Find(filter).ToListAsync(cancellationToken);
    }

    // 🔹 Remove um dia da lista de indisponíveis
    public async Task RemoverIndisponivel(DateTime data, CancellationToken cancellationToken)
    {
        var collection = GetCollection();
        var filter = Builders<DiasIndisponiveis>.Filter.Eq(d => d.Data, data.Date);
        await collection.DeleteOneAsync(filter, cancellationToken);
    }

    public async Task<List<DiasIndisponiveis>> Getdisponiveis(CancellationToken cancellationToken)
    {
        var collection = GetCollection();
        return await collection.Find(_ => true).ToListAsync(cancellationToken);
    }
}