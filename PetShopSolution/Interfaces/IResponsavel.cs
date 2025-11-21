namespace Interfaces;

public interface IResponsavel
{
    Task<bool?> CheckPasswordAsync(string userPassword, string modelPassword);
    Task<IEnumerable<string>> GetRolesAsync<p>(p user);
    Task<List<object>?> GetAllResponsaveis(ICollection<string> resResponsaveis, CancellationToken none);
}