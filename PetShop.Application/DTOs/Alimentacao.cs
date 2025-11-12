using System.ComponentModel.DataAnnotations;

namespace PetShop.Application.DTOs;

public class Alimentacao
{
    [Required, DataType(DataType.Time)]
    public TimeSpan Horario { get; set; }

    [Range(0, 2000)]
    public int? QuantidadeGramas { get; set; }

    public string? TipoRacao { get; set; }
}