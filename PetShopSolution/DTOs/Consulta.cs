using System;
using System.Collections.Generic;

namespace DTOs;

public class Consulta
{
    public Guid Id { get; set; }
    public DateTime Data { get; set; }
    public string? Motivo { get; set; }
    public string? AvaliacaoVeterinario { get; set; }
    public string? Recomendacoes { get; set; }
    public DateTime? DataRetorno { get; set; }

    public ICollection<Diagnostico>? Diagnosticos { get; set; }
    public ICollection<Medicacao>? Medicacoes { get; set; }
    public ICollection<Cirurgia>? Cirurgias { get; set; }
}