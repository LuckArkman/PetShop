using Data;
using DTOs;
using Interfaces;
using Microsoft.Extensions.Configuration;
using MongoDB.Driver;

namespace Services;

public class ResponsavelService : BaseMongoService<Responsavel>, IResponsavelService
{
    public ResponsavelService(ITenantService tenantService, IConfiguration configuration)
        : base(tenantService, configuration)
    {
    }

    public async Task<List<Responsavel>?> GetAllResponsavel(CancellationToken cancellationToken)
    {
        var _objts = await GetCollection()
            .Find(Builders<Responsavel>.Filter.Empty)
            .ToListAsync(cancellationToken);

        return _objts;
    }

    public async Task<Responsavel?> GetObject(string mail, CancellationToken cancellationToken)
    {
        var filter = Builders<Responsavel>.Filter.Eq(u => u.Email, mail);
        var _responsavel = await GetCollection().Find(filter).FirstOrDefaultAsync(cancellationToken);
        return _responsavel;
    }

    public async Task<Responsavel?> GetResponsavelId(string _rg, CancellationToken cancellationToken)
    {
        var filter = Builders<Responsavel>.Filter.Eq(u => u.RG, _rg);
        var _responsavel = await GetCollection().Find(filter).FirstOrDefaultAsync(cancellationToken);
        return _responsavel;
    }

    public async Task<Responsavel?> InsetObject(Responsavel _object, CancellationToken cancellationToken)
    {
        await GetCollection().InsertOneAsync(_object, cancellationToken: cancellationToken);
        return _object;
    }

    public async Task<Responsavel?> UpdateObject(Responsavel _object, CancellationToken cancellationToken)
    {
        var filter = Builders<Responsavel>.Filter.Eq(u => u.Email, _object.Email);

        var update = Builders<Responsavel>.Update
            .Set(u => u.Email, _object.Email)
            .Set(u => u.Password, _object.Password);

        var result = await GetCollection().UpdateOneAsync(filter, update, cancellationToken: cancellationToken);

        if (result.ModifiedCount > 0)
        {
            Console.WriteLine("Responsavel updated successfully.");
        }
        else
        {
            return null;
        }

        return await FindByEmailAsync(_object.Email, cancellationToken);
    }

    public async Task<bool> RemoveAsync(string _object, CancellationToken cancellationToken)
    {
        var filter = Builders<Responsavel>.Filter.Eq(u => u.Email, _object);
        var result = await GetCollection().DeleteOneAsync(filter, cancellationToken);
        return result.DeletedCount > 0;
    }

    public async Task<Responsavel?> FindByEmailAsync(string modelCredencial, CancellationToken cancellationToken)
    {
        var filter = Builders<Responsavel>.Filter.Eq(u => u.Email, modelCredencial);
        var _responsavel = await GetCollection().Find(filter).FirstOrDefaultAsync(cancellationToken);
        return _responsavel;
    }

    public async Task<List<Responsavel>?> GetAllResponsaveis(ICollection<string> resResponsaveis, CancellationToken cancellationToken)
    {
        var filter = Builders<Responsavel>.Filter.In(a => a.Id, resResponsaveis);
        var responsaveis = await GetCollection()
            .Find(filter)
            .ToListAsync(cancellationToken);

        return responsaveis;
    }

    public async Task<Responsavel?> GetResponsavelRg(string? rg)
    {
        var filter = Builders<Responsavel>.Filter.Eq(u => u.RG, rg);
        var _responsavel = await GetCollection().Find(filter).FirstOrDefaultAsync();
        return _responsavel;
    }
}