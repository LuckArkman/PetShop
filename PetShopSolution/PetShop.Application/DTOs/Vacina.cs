using System.ComponentModel.DataAnnotations;
using MongoDB.Bson.Serialization.Attributes;

namespace PetShop.Application.DTOs;

public class Vacina
{
    public string id { get; set; } = Guid.NewGuid().ToString();
    
    public string AnimalId { get; set; }

    [Required, DataType(DataType.Date), Display(Name = "Data da Vacinação")]
    public DateTime _dataVacinacao { get; set; } =  DateTime.UtcNow;

    [Required, Display(Name = "Tipo de Vacina")]
    public string Tipo { get; set; } = string.Empty;

    [Display(Name = "Relatório da Razão")]
    public string? Relatorio { get; set; }
    
    [Display(Name = "veterinario responsavel")]
    public string? _veterinarioId { get; set; }
    
    [BsonElement("responsaveis")]
    public ICollection<string> responsaveis { get; set; }
}