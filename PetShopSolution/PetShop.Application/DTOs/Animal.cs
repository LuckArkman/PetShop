using System.ComponentModel.DataAnnotations;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace PetShop.Application.DTOs;

public class Animal
{
    public string Id { get; set; } = Guid.NewGuid().ToString();

    [Required, Display(Name = "Nome do Animal")]
    public string Nome { get; set; }

    [Required, Display(Name = "Espécie")]
    public string Especie { get; set; }

    [Display(Name = "Raça")]
    public string? Raca { get; set; }

    [Range(0, 100), Display(Name = "Idade (anos)")]
    public int Idade { get; set; }

    [Range(0, 200), Display(Name = "Peso (Kg)")]
    public double Peso { get; set; }

    [Display(Name = "Porte / Tamanho")]
    public string? Porte { get; set; }
    
    [BsonElement("responsavel_id")]
    public ICollection<string> ResponsavelId { get; set; }
}