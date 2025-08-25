using PetShop.Application.DTOs;

namespace PetShop.Application.Interfaces;

public interface IMedicoVeterinario
{
    Task<bool?> CheckPasswordAsync(string userPassword, string modelPassword);
    Task<IEnumerable<string>> GetRolesAsync(MedicoVeterinario user);
}