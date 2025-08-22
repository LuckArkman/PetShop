using System.ComponentModel.DataAnnotations;

namespace PetShop.Application.DTOs;

public class Vacinacao
{
    public int Id { get; set; }

    [Required, DataType(DataType.Date), Display(Name = "Data da Vacinação")]
    public DateTime _dataVacinacao { get; set; }

    [Required, Display(Name = "Tipo de Vacina")]
    public string Tipo { get; set; } = string.Empty;

    [Display(Name = "Relatório da Razão")]
    public string? Relatorio { get; set; }
    [Display(Name = "veterinario responsavel")]
    public MedicoVeterinario _veterinario { get; set; }

    // Relacionamento (opcional, se quiser vincular ao Animal)
    public int? AnimalId { get; set; }
    public Animal? Animal { get; set; }
}