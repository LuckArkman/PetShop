using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace PetShop.Application.DTOs;

public class HistoricoClinico
{
    public string Id { get; set; }= Guid.NewGuid().ToString();
    public string AnimalId { get; set; }

    // Lista de relatórios clínicos relacionados ao animal
    public ICollection<RelatorioClinico> Relatorios { get; set; }

    // Data da última atualização no histórico
    public DateTime UltimaAtualizacao { get; set; }

    public HistoricoClinico(string animalId, RelatorioClinico relatorio)
    {
        AnimalId = animalId;
        Relatorios = new List<RelatorioClinico>(){relatorio};
        UltimaAtualizacao = DateTime.UtcNow;
    }
}