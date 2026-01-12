using System.ComponentModel.DataAnnotations;

namespace DTOs;

public class LoginRequest
{
    [Required(ErrorMessage = "O credencial é obrigatório.")]
    public string credencial { get; set; } = string.Empty;

    [Required(ErrorMessage = "A senha é obrigatória.")]
    [DataType(DataType.Password)]
    public string password { get; set; } = string.Empty;
}