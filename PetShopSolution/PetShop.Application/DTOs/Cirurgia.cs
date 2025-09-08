using PetShop.Application.Enums;

namespace PetShop.Application.DTOs;

public class Cirurgia
{
    public Guid Id { get; set; }
    public DateTime Data { get; set; }
    public TipoCirurgia Tipo { get; set; } = TipoCirurgia.Outro;
    public string? Motivo { get; set; }
    public string? ProcedimentoRealizado { get; set; }
    public string? PosOperatorioAcompanhamento { get; set; }
    public DateTime? DataAlta { get; set; }
}