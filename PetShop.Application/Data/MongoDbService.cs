using MongoDB.Driver;
using PetShop.Application.DTOs;
using PetShop.Application.Interfaces;
using PetShop.Application.Singletons;

namespace PetShop.Application.Data;

public class MongoDbService: IMongoDbService
{
    private readonly IMongoDatabase _database;
    
    public async Task AnimalAsync(IEnumerable<Animal> products)
    {
        IMongoCollection<Animal> collection = _database.GetCollection<Animal>("Animal", (MongoCollectionSettings)null);
        await collection.InsertManyAsync(products, (InsertManyOptions)null, default(CancellationToken));
    }

    public async Task HistoryVacinacaoAsync(IEnumerable<HistoryVacinacao> historyVacinacaos)
    {
        IMongoCollection<HistoryVacinacao> collection = _database.GetCollection<HistoryVacinacao>("HistoryVacinacao", (MongoCollectionSettings)null);
        await collection.InsertManyAsync(historyVacinacaos, (InsertManyOptions)null, default(CancellationToken));
    }

    public async Task MedicoVeterinarioAsync(IEnumerable<MedicoVeterinario> medicoVeterinarios)
    {
        IMongoCollection<MedicoVeterinario> collection = _database.GetCollection<MedicoVeterinario>("MedicoVeterinario", (MongoCollectionSettings)null);
        await collection.InsertManyAsync(medicoVeterinarios, (InsertManyOptions)null, default(CancellationToken));
    }

    public async Task QrCodeRegistroAsync(IEnumerable<QrCodeRegistro> qrCodeRegistros)
    {
        IMongoCollection<QrCodeRegistro> collection = _database.GetCollection<QrCodeRegistro>("QrCodeRegistro", (MongoCollectionSettings)null);
        await collection.InsertManyAsync(qrCodeRegistros, (InsertManyOptions)null, default(CancellationToken));
    }

    public async Task RelatorioClinicoVacinacaoAsync(IEnumerable<RelatorioClinico> relatorioClinicos)
    {
        IMongoCollection<RelatorioClinico> collection = _database.GetCollection<RelatorioClinico>("RelatorioClinico", (MongoCollectionSettings)null);
        await collection.InsertManyAsync(relatorioClinicos, (InsertManyOptions)null, default(CancellationToken));
    }

    public async Task ResponsavelAsync(IEnumerable<Responsavel> responsavels)
    {
        IMongoCollection<Responsavel> collection = _database.GetCollection<Responsavel>("Responsavel", (MongoCollectionSettings)null);
        await collection.InsertManyAsync(responsavels, (InsertManyOptions)null, default(CancellationToken));
    }

    public async Task VacinacaoAsync(IEnumerable<Vacinacao> vacinacaos)
    {
        IMongoCollection<Vacinacao> collection = _database.GetCollection<Vacinacao>("Vacinacao", (MongoCollectionSettings)null);
        await collection.InsertManyAsync(vacinacaos, (InsertManyOptions)null, default(CancellationToken));
    }
}