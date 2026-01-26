using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace DTOs;

public class Animal
{
    public string id { get; set; } = Guid.NewGuid().ToString();

    [Required, Display(Name = "Nome do Animal")]
    public string Nome { get; set; }

    public string? picture { get; set; } = string.Empty;

    [Required, Display(Name = "Espécie")]
    public string Especie { get; set; }

    [Display(Name = "Raça")]
    public string? Raca { get; set; }
    
    public string? Sexo { get; set; }
    
    public bool? Castrado { get; set; }
    public Idade _idade { get; set; }
    public Peso _peso { get; set; }

    [Display(Name = "Porte / Tamanho")]
    public string? Porte { get; set; }
    
    [BsonElement("responsaveis")]
    public ICollection<string> responsaveis { get; set; }
}