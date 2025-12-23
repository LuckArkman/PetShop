using System.Collections;
using Data;
using DTOs;
using Enums;
using Interfaces;
using Microsoft.Extensions.Configuration;
using MongoDB.Driver;

namespace Services;

public class AgendamentoService : IAgendamentoService
{
    private MongoDataController _db { get; set; }
    
    private readonly IConfiguration _configuration;
    protected IMongoCollection<Agendamento> _collection;
    public string _collectionName { get; set; }
    private IMongoDatabase _mongoDatabase { get; set; }
    
    public void InitializeCollection(string connectionString,
        string databaseName,
        string collectionName)
    {
        _collectionName = collectionName;
        // Verifica se a conexão já foi estabelecida
        if (_collection != null) return;
        
        _db = new MongoDataController(connectionString, databaseName, _collectionName);
        _mongoDatabase = _db.GetDatabase();
        _collection = _mongoDatabase.GetCollection<Agendamento>(_collectionName);
    }

    public async Task<Agendamento?> GetById(string id, CancellationToken cancellationToken)
    {
        var filter = Builders<Agendamento>.Filter.Eq("_id", id);
        return await _collection.Find(filter).FirstOrDefaultAsync(cancellationToken);
    }

    public async Task<IEnumerable<Agendamento>> GetByCliente(string rg, CancellationToken cancellationToken)
    {
        var filter = Builders<Agendamento>.Filter.Eq(a => a.rg, rg);
        return await _collection.Find(filter).ToListAsync(cancellationToken);
    }

    public async Task<Agendamento> Create(Agendamento agendamento, CancellationToken cancellationToken)
    {
        await _collection.InsertOneAsync(agendamento, cancellationToken: cancellationToken);
        return agendamento;
    }

    public async Task<Agendamento?> UpdateStatus(string id, Status status, CancellationToken cancellationToken)
    {
        var filter = Builders<Agendamento>.Filter.Eq(a => a.id, id);
        var update = Builders<Agendamento>.Update.Set<Status>(a => a.status, status);

        var result = await _collection.UpdateOneAsync(filter, update, cancellationToken: cancellationToken);
        return result.ModifiedCount > 0 ? await GetById(id, cancellationToken) : null;
    }

    public async Task<bool> Delete(string id, CancellationToken cancellationToken)
    {
        var collection = _db.GetDatabase().GetCollection<Agendamento>("Agendamento");
        var result = await collection.DeleteOneAsync(a => a.id == id, cancellationToken);
        return result.DeletedCount > 0;
    }
    
    /// <summary>
    /// Remove um agendamento específico com base na data e horário exatos.
    /// </summary>
    /// <param name="dataHora">Data e hora da consulta</param>
    /// <param name="cancellationToken">Token de cancelamento</param>
    /// <returns>True se removido com sucesso, False se não encontrado</returns>
    public async Task<bool> DeleteByDateTime(DateTime dataHora, CancellationToken cancellationToken)
    {
        var collection = _db.GetDatabase().GetCollection<Agendamento>("Agendamento");
        var filter = Builders<Agendamento>.Filter.Eq(a => a.dataConsulta, dataHora);

        var result = await collection.DeleteOneAsync(filter, cancellationToken);
        return result.DeletedCount > 0;
    }
    
    public async Task<IEnumerable<Agendamento>> GetByVeterinario(string crmv, CancellationToken cancellationToken)
    {
        var collection = _db.GetDatabase().GetCollection<Agendamento>("Agendamento");

        var filter = Builders<Agendamento>.Filter.Eq(a => a.crmv, crmv) &
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

        var inicioDoDia = dataConsulta.Date;
        var fimDoDia = inicioDoDia.AddDays(1);

        var filtro = Builders<Agendamento>.Filter.And(
            Builders<Agendamento>.Filter.Gte(a => a.dataConsulta, inicioDoDia),
            Builders<Agendamento>.Filter.Lt(a => a.dataConsulta, fimDoDia)
        );

        return await collection.Find(filtro).ToListAsync(cancellationToken);
    }
}
