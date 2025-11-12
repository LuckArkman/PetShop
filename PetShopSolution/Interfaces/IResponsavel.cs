namespace Interfaces;

public interface IResponsavel
{
    Task<bool?> CheckPasswordAsync(string userPassword, string modelPassword);
    Task<IEnumerable<string>> GetRolesAsync<p>(p user);
}