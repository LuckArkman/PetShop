using PetShop.Application.DTOs;

namespace PetShop.Application.Interfaces;

public interface IMongoDbService
{
    Task AnimalAsync(IEnumerable<Animal> products);

    Task HistoryVacinacaoAsync(IEnumerable<HistoryVacinacao> orders);

    Task MedicoVeterinarioAsync(IEnumerable<MedicoVeterinario> reviews);
    
    Task QrCodeRegistroAsync(IEnumerable<QrCodeRegistro> products);

    Task RelatorioClinicoVacinacaoAsync(IEnumerable<RelatorioClinico> orders);

    Task ResponsavelAsync(IEnumerable<Responsavel> reviews);
    
    Task VacinacaoAsync(IEnumerable<Vacinacao> products);
}