using Data;
using DTOs;
using Interfaces;
using Microsoft.Extensions.Configuration;
using MongoDB.Driver;

namespace Services;

public class AtendenteService : BaseMongoService<Atendente>, IAtendenteService
{
    public AtendenteService(ITenantService tenantService, IConfiguration configuration)
        : base(tenantService, configuration)
    {
    }

    public async Task<List<Atendente>?> GetAllAtendente(CancellationToken cancellationToken)
    {
        var _objts = await GetCollection()
            .Find(Builders<Atendente>.Filter.Empty)
            .ToListAsync(cancellationToken);

        return _objts;
    }

    public async Task<Atendente?> GetObject(string mail, CancellationToken cancellationToken)
    {
        var filter = Builders<Atendente>.Filter.Eq(u => u.email, mail);
        var _Atendente = await GetCollection().Find(filter).FirstOrDefaultAsync(cancellationToken);
        return _Atendente;
    }

    public async Task<Atendente?> GetAtendenteRG(string _rg, CancellationToken cancellationToken)
    {
        var filter = Builders<Atendente>.Filter.Eq(u => u.RG, _rg);
        var _atendente = await GetCollection().Find(filter).FirstOrDefaultAsync(cancellationToken);
        return _atendente;
    }

    public async Task<Atendente?> InsetObject(Atendente _object, CancellationToken cancellationToken)
    {
        await GetCollection().InsertOneAsync(_object, cancellationToken: cancellationToken);
        return _object;
    }

    public async Task<Atendente?> UpdateObject(Atendente _object, CancellationToken cancellationToken)
    {
        var filter = Builders<Atendente>.Filter.Eq(u => u.email, _object.email);

        var update = Builders<Atendente>.Update
            .Set(u => u.email, _object.email)
            .Set(u => u.nome, _object.nome)
            .Set(u => u.LastName, _object.LastName)
            .Set(u => u.CPF, _object.CPF)
            .Set(u => u.RG, _object.RG)
            .Set(u => u.Address, _object.Address)
            .Set(u => u.City, _object.City)
            .Set(u => u.State, _object.State)
            .Set(u => u.ZipCode, _object.ZipCode)
            .Set(u => u.PhoneNumber, _object.PhoneNumber);

        var result = await GetCollection().UpdateOneAsync(filter, update, cancellationToken: cancellationToken);

        if (result.ModifiedCount > 0)
        {
            Console.WriteLine("Atendente updated successfully.");
        }
        else
        {
            return null;
        }

        return await FindByEmailAsync(_object.email!, cancellationToken);
    }

    public async Task<bool> RemoveAsync(string mail, CancellationToken cancellationToken)
    {
        var filter = Builders<Atendente>.Filter.Eq(u => u.email, mail);
        var result = await GetCollection().DeleteOneAsync(filter, cancellationToken);
        return result.DeletedCount > 0;
    }

    public async Task<Atendente?> FindByEmailAsync(string modelCredencial, CancellationToken cancellationToken)
    {
        var filter = Builders<Atendente>.Filter.Eq(u => u.email, modelCredencial);
        var _atendente = await GetCollection().Find(filter).FirstOrDefaultAsync(cancellationToken);
        return _atendente;
    }
}