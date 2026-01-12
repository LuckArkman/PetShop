using System.Collections.Generic;

namespace DTOs;

public class HistoryVacinacao
{
    public string id { get; set; }
    public string _animalId { get; set; }
    public ICollection<Vacinacao> _Vacinacao { get; set; }

    public HistoryVacinacao(string id, string animalId, Vacinacao vacinacao)
    {
        this.id = id;
        _animalId = animalId;
        _Vacinacao = new List<Vacinacao>(){vacinacao};
    }
}