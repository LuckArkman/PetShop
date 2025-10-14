using System.Collections;
using MongoDB.Driver;
using PetShop.Application.Data;
using PetShop.Application.DTOs;
using PetShop.Application.Enums;
using PetShop.Application.Interfaces;
using PetShop.Application.Singletons;

namespace PetShop.Application.Services;

public class AgendamentoService : IAgendamentoService
{
    private readonly AgendamentoDB _db;

    public AgendamentoService()
    {
        _db = new AgendamentoDB(Singleton.Instance()!.src, "Agendamento");
        _db.GetOrCreateDatabase();
    }

    public async Task<Agendamento?> GetById(string id, CancellationToken cancellationToken)
    {
        var collection = _db.GetDatabase().GetCollection<Agendamento>("Agendamento");
        var filter = Builders<Agendamento>.Filter.Eq(a => a.id, id);
        return await collection.Find(filter).FirstOrDefaultAsync(cancellationToken);
    }

    public async Task<IEnumerable<Agendamento>> GetByCliente(string clienteId, CancellationToken cancellationToken)
    {
        var collection = _db.GetDatabase().GetCollection<Agendamento>("Agendamento");
        var filter = Builders<Agendamento>.Filter.Eq(a => a.clienteId, clienteId);
        return await collection.Find(filter).ToListAsync(cancellationToken);
    }

    public async Task<Agendamento> Create(Agendamento agendamento, CancellationToken cancellationToken)
    {
        var collection = _db.GetDatabase().GetCollection<Agendamento>("Agendamento");
        await collection.InsertOneAsync(agendamento, cancellationToken: cancellationToken);
        return agendamento;
    }

    public async Task<Agendamento?> UpdateStatus(string id, Status status, CancellationToken cancellationToken)
    {
        var collection = _db.GetDatabase().GetCollection<Agendamento>("Agendamento");
        var filter = Builders<Agendamento>.Filter.Eq(a => a.id, id);
        var update = Builders<Agendamento>.Update.Set<Status>(a => a.status, status);

        var result = await collection.UpdateOneAsync(filter, update, cancellationToken: cancellationToken);
        return result.ModifiedCount > 0 ? await GetById(id, cancellationToken) : null;
    }

    public async Task<bool> Delete(string id, CancellationToken cancellationToken)
    {
        var collection = _db.GetDatabase().GetCollection<Agendamento>("Agendamento");
        var result = await collection.DeleteOneAsync(a => a.id == id, cancellationToken);
        return result.DeletedCount > 0;
    }
    
    public async Task<IEnumerable<Agendamento>> GetByVeterinario(string veterinarioId, CancellationToken cancellationToken)
    {
        var collection = _db.GetDatabase().GetCollection<Agendamento>("Agendamento");

        var filter = Builders<Agendamento>.Filter.Eq(a => a.veterinarioId, veterinarioId) &
                     Builders<Agendamento>.Filter.Ne(a => a.status, Status.Cancelado);

        return await collection.Find(filter).ToListAsync(cancellationToken);
    }


    /// <summary>
    /// Retorna todos os agendamentos de um dia específico,
    /// considerando corretamente o fuso horário (UTC) e
    /// excluindo automaticamente os que estiverem Cancelados ou Concluídos.
    /// </summary>
    public async Task<List<Agendamento>> GetByDate(DateTime dataConsulta, CancellationToken cancellationToken)
    {
        var collection = _db.GetDatabase().GetCollection<Agendamento>("Agendamento");

        // normaliza a data local e converte para UTC
        var inicioLocal = DateTime.SpecifyKind(dataConsulta.Date, DateTimeKind.Local);
        var fimLocal = DateTime.SpecifyKind(dataConsulta.Date.AddDays(1), DateTimeKind.Local);

        var inicioUtc = inicioLocal.ToUniversalTime();
        var fimUtc = fimLocal.ToUniversalTime();

        var filtro = Builders<Agendamento>.Filter.And(
            Builders<Agendamento>.Filter.Gte(a => a.dataConsulta, inicioUtc),
            Builders<Agendamento>.Filter.Lt(a => a.dataConsulta, fimUtc),
            Builders<Agendamento>.Filter.Ne(a => a.status, Status.Cancelado),
            Builders<Agendamento>.Filter.Ne(a => a.status, Status.Concluído)
        );

        return await collection.Find(filtro).ToListAsync(cancellationToken);
    }
}
