using System.Collections.Generic;

namespace DTOs;

public class DisponibilidadeDiaDTO
{
    public string Data { get; set; } = string.Empty;
    public List<string> HorariosDisponiveis { get; set; } = new();
}