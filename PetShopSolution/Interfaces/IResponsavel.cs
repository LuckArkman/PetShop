namespace Interfaces;

public interface IResponsavel
{
    void InitializeCollection(string connectionString,
        string databaseName,
        string collectionName);
    Task<bool?> CheckPasswordAsync(string userPassword, string modelPassword);
    Task<IEnumerable<string>> GetRolesAsync<p>(p user);
    Task<List<object>?> GetAllResponsaveis(ICollection<string> resResponsaveis, CancellationToken none);
}