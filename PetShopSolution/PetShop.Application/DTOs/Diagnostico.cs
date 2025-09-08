using PetShop.Application.Enums;

namespace PetShop.Application.DTOs;

public class Diagnostico
{
    public Guid Id { get; set; }
    public DateTime Data { get; set; }
    public string DoencaOuCondicao { get; set; }
    public string? SintomasObservados { get; set; }
    public string? ExamesSolicitados { get; set; }
    public string? CondutaTratamento { get; set; }
    public GravidadeDiagnostico? Gravidade { get; set; }
}