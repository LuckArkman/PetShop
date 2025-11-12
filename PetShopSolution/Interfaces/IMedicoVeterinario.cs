namespace Interfaces;

public interface IMedicoVeterinario
{
    Task<bool?> CheckPasswordAsync(string userPassword, string modelPassword);
    Task<IEnumerable<string>> GetRolesAsync<p>(p user);
}