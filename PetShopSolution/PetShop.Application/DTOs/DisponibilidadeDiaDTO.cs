namespace PetShop.Application.DTOs;

public class DisponibilidadeDiaDTO
{
    public string Data { get; set; } = string.Empty;
    public List<string> HorariosDisponiveis { get; set; } = new();
}