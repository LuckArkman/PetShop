using DTOs;

namespace Interfaces;

public interface IMedicacaoService
{
    void InitializeCollection(string connectionString,
        string databaseName,
        string collectionName);
    Task<List<Medicacao>?> GetAllMedicacoes(string _object,CancellationToken cancellationToken);
    Task<Medicacao?> GetMedicacao(string _object, CancellationToken cancellationToken);

    Task<Medicacao?> InsetMedicacao(Medicacao _object, CancellationToken cancellationToken);

    Task<Medicacao?> UpdateMedicacao(Medicacao _object, CancellationToken cancellationToken);

    Task<bool?> RemoveMedicacao(Medicacao _object, CancellationToken cancellationToken);
}