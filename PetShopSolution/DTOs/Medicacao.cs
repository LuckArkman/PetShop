using System;

namespace DTOs;

public class Medicacao
{
    public string Id { get; set; } = Guid.NewGuid().ToString();
    public string animalId { get; set; }
    public DateTime data { get; set; }
    public string nome { get; set; }
    public string? dosagem { get; set; }       // Ex: "5 mg/kg"
    public string? duracao { get; set; }       // Ex: "7 dias"
    public string? indicacao { get; set; }
    public string? observacoes { get; set; }
}