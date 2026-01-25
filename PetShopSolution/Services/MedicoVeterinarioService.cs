using Data;
using DTOs;
using Interfaces;
using Microsoft.Extensions.Configuration;
using MongoDB.Driver;

namespace Services;

public class MedicoVeterinarioService : BaseMongoService<MedicoVeterinario>, IMedicoVeterinarioService
{
    public MedicoVeterinarioService(ITenantService tenantService, IConfiguration configuration)
        : base(tenantService, configuration)
    {
    }

    public async Task<List<MedicoVeterinario>?> GetAllMedicoVeterinario(CancellationToken cancellationToken)
    {
        var _objts = await GetCollection()
            .Find(Builders<MedicoVeterinario>.Filter.Empty)
            .ToListAsync(cancellationToken);

        return _objts;
    }

    public async Task<MedicoVeterinario?> GetObject(string _object, CancellationToken cancellationToken)
    {
        var filter = Builders<MedicoVeterinario>.Filter.Eq(u => u.CRMV, _object);
        var character = await GetCollection().Find(filter).FirstOrDefaultAsync(cancellationToken);
        return character;
    }

    public async Task<MedicoVeterinario?> InsetObject(MedicoVeterinario _object, CancellationToken cancellationToken)
    {
        await GetCollection().InsertOneAsync(_object, cancellationToken: cancellationToken);
        return _object;
    }

    public async Task<MedicoVeterinario?> UpdateObject(MedicoVeterinario _object, CancellationToken cancellationToken)
    {
        var filter = Builders<MedicoVeterinario>.Filter.Eq(u => u.Id, _object.Id);

        var update = Builders<MedicoVeterinario>.Update
            .Set(u => u.Nome, _object.Nome)
            .Set(u => u.CRMV, _object.CRMV)
            .Set(u => u.Especialidade, _object.Especialidade)
            .Set(u => u.Telefone, _object.Telefone)
            .Set(u => u.Email, _object.Email)
            .Set(u => u.Endereco, _object.Endereco)
            .Set(u => u.Cidade, _object.Cidade)
            .Set(u => u.Estado, _object.Estado)
            .Set(u => u.CEP, _object.CEP);

        var result = await GetCollection().UpdateOneAsync(filter, update, cancellationToken: cancellationToken);

        if (result.ModifiedCount > 0)
        {
            Console.WriteLine("MedicoVeterinario updated successfully.");
        }
        else
        {
            return null;
        }

        return await GetObject(_object.CRMV!, cancellationToken);
    }

    public async Task<bool> RemoveAsync(MedicoVeterinario _object, CancellationToken cancellationToken)
    {
        var filter = Builders<MedicoVeterinario>.Filter.Eq(u => u.CRMV, _object.CRMV);
        var result = await GetCollection().DeleteOneAsync(filter, cancellationToken);
        return result.DeletedCount > 0;
    }

    public async Task<MedicoVeterinario?> FindByCRMVAsync(string modelCredencial, CancellationToken cancellationToken)
    {
        var filter = Builders<MedicoVeterinario>.Filter.Eq(u => u.CRMV, modelCredencial);
        var character = await GetCollection().Find(filter).FirstOrDefaultAsync(cancellationToken);
        return character;
    }
}