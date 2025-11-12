using Enums;

namespace DTOs;

public class Cirurgia
{
    public string Id { get; set; } = Guid.NewGuid().ToString();
    public string animalId { get; set; }
    public DateTime data { get; set; }
    public TipoCirurgia tipo { get; set; } = TipoCirurgia.Outro;
    public string? motivo { get; set; }
    public string? procedimentoRealizado { get; set; }
    public Relatorio? relatorio { get; set; }
    public string? posOperatorioAcompanhamento { get; set; }
    public DateTime? dataAlta { get; set; }
    
}