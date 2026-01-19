using DTOs;

namespace Interfaces;

public interface IAtendimentoService
{
    void InitializeCollection(string? connectionString,
        string? databaseName,
        string? collectionName);
    Task<Atendimento?> GetById(string id, CancellationToken cancellationToken);
    Task<IEnumerable<Atendimento>> GetByAnimal(string animalId, CancellationToken cancellationToken);
    Task<Atendimento> Create(Atendimento atendimento, CancellationToken cancellationToken);
    Task<Atendimento?> Update(Atendimento atendimento, CancellationToken cancellationToken);
    Task<bool> Delete(string id, CancellationToken cancellationToken);
}