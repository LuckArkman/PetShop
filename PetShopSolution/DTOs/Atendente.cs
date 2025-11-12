using System.ComponentModel.DataAnnotations;
using MongoDB.Bson.Serialization.Attributes;

namespace DTOs;

public class Atendente
{
    public string Id { get; set; } = Guid.NewGuid().ToString();
    
    [StringLength(50)]
    public string? nome { get; set; }
    
    [StringLength(50)]
    public string? LastName { get; set; }
    
    [StringLength(50)]
    public string? email { get; set; }
    
    [BsonElement("cpf")]
    public string? CPF { get; set; }
    
    [BsonElement("rg")]
    public string? RG { get; set; }
    
    [Required(ErrorMessage = "A senha é obrigatória.")]
    [DataType(DataType.Password)]
    public string Password { get; set; } = string.Empty;

    [DataType(DataType.Password)]
    [Compare("Password", ErrorMessage = "A senha e a confirmação de senha não coincidem.")]
    public string ConfirmPassword { get; set; } = string.Empty;

    [StringLength(100)]
    public string? Address { get; set; }

    [StringLength(50)]
    public string? City { get; set; }

    [StringLength(50)]
    public string? State { get; set; }

    [StringLength(20)]
    public string? ZipCode { get; set; }

    [Phone]
    public string? PhoneNumber { get; set; }
}