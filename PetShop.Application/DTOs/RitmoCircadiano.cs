using System.ComponentModel.DataAnnotations;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace PetShop.Application.DTOs;

public class RitmoCircadiano
{
    public string Id { get; set; } = Guid.NewGuid().ToString();
    public string AnimalId { get; set; }
    public DateTime Data { get; set; }
    public TimeSpan SonoInicio { get; set; }
    public TimeSpan SonoFim { get; set; }
    // Sonecas diurnas
    public ICollection<Soneca> Sonecas { get; set; }
    // Alimentação
    public ICollection<Alimentacao> Refeicoes { get; set; }
    // Atividade percebida (0–10)
    [Range(0,10)]
    public int AtividadeManha { get; set; }

    [Range(0,10)]
    public int AtividadeTarde { get; set; }

    [Range(0,10)]
    public int AtividadeNoite { get; set; }

    // Exposição à luz (minutos sob luz forte/sol)
    [Range(0, 1440)]
    public int LuzManhaMin { get; set; }

    [Range(0, 1440)]
    public int LuzTardeMin { get; set; }

    [Range(0, 1440)]
    public int LuzNoiteMin { get; set; }

    // Ambiente
    public double? TemperaturaAmbienteC { get; set; }

    // Hidratação (mL)
    [Range(0, 10000)]
    public int? AguaMl { get; set; }

    public string? Observacoes { get; set; }

    // --- Derivados (não persistidos) ---
    [BsonIgnore]
    public TimeSpan SonoNoturnoTotal => DuracaoDePara(SonoInicio, SonoFim); // lida com virada de dia

    [BsonIgnore]
    public TimeSpan SonecasTotal => TimeSpan.FromMinutes(Sonecas.Sum(s => DuracaoDePara(s.Inicio, s.Fim).TotalMinutes));

    [BsonIgnore]
    public TimeSpan SonoTotal => SonoNoturnoTotal + SonecasTotal;

    private static TimeSpan DuracaoDePara(TimeSpan inicio, TimeSpan fim)
    {
        // se fim < início, assume que virou o dia
        return (fim >= inicio) ? (fim - inicio) : (new TimeSpan(24,0,0) - inicio + fim);
    }
}