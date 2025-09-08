namespace PetShop.Application.DTOs;

public class Medicacao
{
    public Guid Id { get; set; }
    public DateTime Data { get; set; }
    public string Nome { get; set; }
    public string? Dosagem { get; set; }       // Ex: "5 mg/kg"
    public string? Duracao { get; set; }       // Ex: "7 dias"
    public string? Indicacao { get; set; }
    public string? ReacoesObservacoes { get; set; }
}