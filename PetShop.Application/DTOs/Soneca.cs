using System.ComponentModel.DataAnnotations;

namespace PetShop.Application.DTOs;


public class Soneca
{
    public string Id { get; set; }
    public string AnimalId { get; set; }
    [Required, DataType(DataType.Time)]
    public TimeSpan Inicio { get; set; }

    [Required, DataType(DataType.Time)]
    public TimeSpan Fim { get; set; }
}