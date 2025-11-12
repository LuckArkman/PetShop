using PetShop.Application.DTOs;

namespace PetShop.Application.Interfaces;

public interface IResponsavel
{
    Task<bool?> CheckPasswordAsync(string userPassword, string modelPassword);
    Task<IEnumerable<string>> GetRolesAsync(MedicoVeterinario user);
}