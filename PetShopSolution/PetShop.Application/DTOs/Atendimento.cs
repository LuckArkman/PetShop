using MongoDB.Bson;

namespace PetShop.Application.DTOs;

public class Atendimento
{
    public string id { get; set; } = ObjectId.GenerateNewId().ToString();

    public string? animalId { get; set; }
    public string? clienteId { get; set; }
    public string veterinarioId { get; set; }
    public string descricao { get; set; }
    public DateTime dataAtendimento { get; set; } = DateTime.UtcNow;
    public string? observacoes { get; set; }
}