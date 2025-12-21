namespace Interfaces;

public interface IMedicoVeterinario
{
    void InitializeCollection(string connectionString,
        string databaseName,
        string collectionName);
    Task<bool?> CheckPasswordAsync(string userPassword, string modelPassword);
    Task<IEnumerable<string>> GetRolesAsync<p>(p user);
}