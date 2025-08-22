using System.ComponentModel.DataAnnotations;

namespace PetShop.Application.DTOs;

public class MedicoVeterinario
{
    public int Id { get; set; }

    [Required, Display(Name = "Nome Completo")]
    public string Nome { get; set; } = string.Empty;

    [Required, Display(Name = "CRMV (Registro Profissional)")]
    public string CRMV { get; set; } = string.Empty;

    [Required, Display(Name = "Especialidade")]
    public string Especialidade { get; set; } = string.Empty;

    [Required, Phone, Display(Name = "Telefone")]
    public string Telefone { get; set; } = string.Empty;

    [Required, EmailAddress, Display(Name = "E-mail")]
    public string Email { get; set; } = string.Empty;

    [Display(Name = "Endereço Completo")]
    public string? Endereco { get; set; }

    [Display(Name = "Cidade")]
    public string? Cidade { get; set; }

    [Display(Name = "Estado (UF)")]
    public string? Estado { get; set; }

    [Display(Name = "CEP")]
    public string? CEP { get; set; }
}