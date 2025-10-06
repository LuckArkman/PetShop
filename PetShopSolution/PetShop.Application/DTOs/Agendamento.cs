using MongoDB.Bson;
using PetShop.Application.Enums;

namespace PetShop.Application.DTOs;

public class Agendamento
{
    public string id { get; set; } = ObjectId.GenerateNewId().ToString();

    public string? animalId { get; set; }
    public string? clienteId { get; set; }
    public string veterinarioId { get; set; }
    public DateTime? dataConsulta { get; set; } = DateTime.UtcNow;
    public Status status { get; set; }

    public Agendamento(string? animalId, string? clienteId, string veterinarioId, Status status)
    {
        this.animalId = animalId;
        this.clienteId = clienteId;
        this.veterinarioId = veterinarioId;
        this.status = status;
    }
}