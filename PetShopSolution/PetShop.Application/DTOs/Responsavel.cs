using System.ComponentModel.DataAnnotations;
using MongoDB.Bson.Serialization.Attributes;

namespace PetShop.Application.DTOs;

public class Responsavel
{
    [Key]
    public string Id { get; set; } = Guid.NewGuid().ToString();

    [Required]
    [EmailAddress]
    public string Email { get; set; } = string.Empty;

    [Required]
    [StringLength(50)]
    public string UserName { get; set; } = string.Empty;

    [StringLength(50)]
    public string? FirstName { get; set; }

    [StringLength(50)]
    public string? LastName { get; set; }
    
    [BsonElement("cpf")]
    public string? CPF { get; set; }

    [BsonElement("rg")]
    public string? RG { get; set; }

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

    // Relacionamento: um responsável pode ter vários animais
    public ICollection<Animal> Animais { get; set; } = new List<Animal>();
}
