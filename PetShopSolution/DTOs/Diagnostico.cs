using System;
using Enums;

namespace DTOs;

public class Diagnostico
{
    public string Id { get; set; } = Guid.NewGuid().ToString();
    public string animalId { get; set; }
    public DateTime _data { get; set; }
    public string doencaOuCondicao { get; set; }
    public string? sintomasObservados { get; set; }
    public string? examesSolicitados { get; set; }
    public string? condutaTratamento { get; set; }
    public GravidadeDiagnostico? gravidade { get; set; }
}