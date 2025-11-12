using System.ComponentModel.DataAnnotations;

namespace PetShop.Application.DTOs;

public class FrequenciaCardiaca
{
    public string Id { get; set; } = Guid.NewGuid().ToString();
    public string AnimalId { get; set; }
    public TimeSpan Horario { get; set; }
    
    [Range(0, 200), Display(Name = "Peso (Kg)")]
    public double Peso { get; set; }
    
    public string Especie { get; set; }

    [Display(Name = "Raça")]
    public string? Raca { get; set; }
    
    [Display(Name = "Porte / Tamanho")]
    public string? Porte { get; set; }

    [Range(20, 400, ErrorMessage = "A frequência cardíaca deve estar entre 20 e 400 bpm.")]
    public int BatimentosPorMinuto { get; set; }

    public string? Observacao { get; set; }
}