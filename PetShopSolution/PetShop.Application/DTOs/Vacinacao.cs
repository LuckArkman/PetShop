using System.ComponentModel.DataAnnotations;

namespace PetShop.Application.DTOs;

public class Vacinacao
{
    public string Id { get; set; }
    public string AnimalId { get; set; }

    [Required, DataType(DataType.Date), Display(Name = "Data da Vacinação")]
    public DateTime _dataVacinacao { get; set; }

    [Required, Display(Name = "Tipo de Vacina")]
    public string Tipo { get; set; } = string.Empty;

    [Display(Name = "Relatório da Razão")]
    public string? Relatorio { get; set; }
    
    [Display(Name = "veterinario responsavel")]
    public string? _veterinarioId { get; set; }

    public Vacinacao()
    {
        Id = Guid.NewGuid().ToString();
        _dataVacinacao =  DateTime.UtcNow;
    }
    
}