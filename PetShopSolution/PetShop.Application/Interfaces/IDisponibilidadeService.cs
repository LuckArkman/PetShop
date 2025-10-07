using MongoDB.Driver;
using PetShop.Application.DTOs;

namespace PetShop.Application.Interfaces;

public interface IDisponibilidadeService
{
    IMongoCollection<DiasIndisponiveis> GetCollection();
    Task<List<DiasIndisponiveis>> GetIndisponiveis(CancellationToken cancellationToken);
    Task AddIndisponivel(DiasIndisponiveis dia, CancellationToken cancellationToken);
    Task RemoverIndisponivel(DateTime data, CancellationToken cancellationToken);
}