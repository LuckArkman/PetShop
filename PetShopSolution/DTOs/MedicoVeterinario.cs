using System;
using System.ComponentModel.DataAnnotations;

namespace DTOs;

public class MedicoVeterinario
{
    public string Id { get; set; } =  Guid.NewGuid().ToString();

    [Required, Display(Name = "Nome Completo")]
    public string? Nome { get; set; }

    [Required, Display(Name = "CRMV (Registro Profissional)")]
    public string? CRMV { get; set; }
    
    [Required(ErrorMessage = "A senha é obrigatória.")]
    [DataType(DataType.Password)]
    public string Password { get; set; }

    [DataType(DataType.Password)]
    [Compare("Password", ErrorMessage = "A senha e a confirmação de senha não coincidem.")]
    public string ConfirmPassword { get; set; }

    [Required, Display(Name = "Especialidade")]
    public string Especialidade { get; set; }

    [Required, Phone, Display(Name = "Telefone")]
    public string? Telefone { get; set; }

    [Required, EmailAddress, Display(Name = "E-mail")]
    public string? Email { get; set; }
    
    [Display(Name = "Endereço Completo")]
    public string? Endereco { get; set; }

    [Display(Name = "Cidade")]
    public string? Cidade { get; set; }

    [Display(Name = "Estado (UF)")]
    public string? Estado { get; set; }

    [Display(Name = "CEP")]
    public string? CEP { get; set; }
}