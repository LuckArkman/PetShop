using DTOs;
using MongoDB.Driver;

namespace Interfaces;

public interface IDisponibilidadeService
{
    void InitializeCollection(string? connectionString,
        string? databaseName,
        string? collectionName);
    IMongoCollection<DiasIndisponiveis> GetCollection();
    Task<List<DiasIndisponiveis>> GetIndisponiveis(CancellationToken cancellationToken);
    Task AddIndisponivel(DiasIndisponiveis dia, CancellationToken cancellationToken);
    Task<IEnumerable<Agendamento>> GetByDate(DateTime date, CancellationToken cancellationToken);
    Task RemoverIndisponivel(DateTime data, CancellationToken cancellationToken);
    Task<List<DiasIndisponiveis>> Getdisponiveis(CancellationToken cancellationToken);
}