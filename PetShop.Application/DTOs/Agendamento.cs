using System.ComponentModel.DataAnnotations;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using PetShop.Application.Enums;

namespace PetShop.Application.DTOs;

public class Agendamento
{
    public string id { get; set; }
    public string? animalId { get; set; }
    public string? rg { get; set; }

    [Required(ErrorMessage = "O campo veterinarioId é obrigatório.")]
    public string crmv { get; set; } = null!;

    [Required(ErrorMessage = "A data da consulta é obrigatória.")]
    public DateTime? dataConsulta { get; set; }

    [BsonRepresentation(BsonType.String)]
    public Status status { get; set; } = Status.Agendado;

    // Construtor padrão (necessário para o model binding do ASP.NET)
    public Agendamento() { }

    // Construtor auxiliar (opcional)
    public Agendamento(string? animalId, string? rg, string crmv, DateTime dataConsulta, Status status = Status.Agendado)
    {
        this.animalId = animalId;
        this.rg = rg;
        this.crmv = crmv;
        this.dataConsulta = dataConsulta;
        this.status = status;
    }
}