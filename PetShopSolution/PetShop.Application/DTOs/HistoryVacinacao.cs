namespace PetShop.Application.DTOs;

public class HistoryVacinacao
{
    public string id { get; set; }
    public string _animalId { get; set; }
    public ICollection<Vacinacao> _Vacinacao { get; set; }
}