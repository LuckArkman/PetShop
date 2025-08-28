using System.ComponentModel.DataAnnotations;

namespace PetShop.Application.DTOs;


public class Soneca
{
    [Required, DataType(DataType.Time)]
    public TimeSpan Inicio { get; set; }

    [Required, DataType(DataType.Time)]
    public TimeSpan Fim { get; set; }
}