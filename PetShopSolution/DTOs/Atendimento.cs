using System;
using MongoDB.Bson;

namespace DTOs;

public class Atendimento
{
    public string id { get; set; } = ObjectId.GenerateNewId().ToString();

    public string? animalId { get; set; }
    public string? rg { get; set; }
    public string crmv { get; set; }
    public string descricao { get; set; }
    public DateTime dataAtendimento { get; set; } = DateTime.UtcNow;
    public string? observacoes { get; set; }
}