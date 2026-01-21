using DTOs;
using Interfaces;
using Microsoft.Extensions.Configuration;
using MongoDB.Driver;

namespace Services;

public class AdminService : BaseMongoService<RegisterViewModel>, IAdminService
{
    public AdminService(ITenantService tenantService, IConfiguration configuration)
        : base(tenantService, configuration)
    {
    }

    public async Task<RegisterViewModel?> GetObject(string mail, CancellationToken cancellationToken)
    {
        var filter = Builders<RegisterViewModel>.Filter.Eq(u => u.Email, mail);
        var character = await GetCollection().Find(filter).FirstOrDefaultAsync(cancellationToken);
        return character;
    }

    public async Task<RegisterViewModel?> InsetObject(RegisterViewModel _object, CancellationToken cancellationToken)
    {
        await GetCollection().InsertOneAsync(_object, cancellationToken: cancellationToken);
        return _object;
    }

    public async Task<RegisterViewModel?> UpdateObject(RegisterViewModel _object, CancellationToken cancellationToken)
    {
        var filter = Builders<RegisterViewModel>.Filter.Eq(u => u.id, _object.id);
        var update = Builders<RegisterViewModel>.Update
            .Set(u => u.id, _object.id)
            .Set(u => u.Nome, _object.Nome)
            .Set(u => u.Telefone, _object.Telefone)
            .Set(u => u.Email, _object.Email)
            .Set(u => u.Senha, _object.Senha)
            .Set(u => u.ConfirmarSenha, _object.ConfirmarSenha);

        var result = await GetCollection().UpdateOneAsync(filter, update, cancellationToken: cancellationToken);

        if (result.ModifiedCount > 0)
        {
            Console.WriteLine("Vacinacao updated successfully.");
        }
        else
        {
            return null;
        }

        return await GetObject(_object.Email!, cancellationToken);
    }

    public async Task<bool> RemoveAsync(string _object, CancellationToken cancellationToken)
    {
        var filter = Builders<RegisterViewModel>.Filter.Eq(u => u.Email, _object);
        var result = await GetCollection().DeleteOneAsync(filter, cancellationToken);
        return result.DeletedCount > 0;
    }

    public async Task<RegisterViewModel?> FindByEmailAsync(string modelCredencial, CancellationToken cancellationToken)
    {
        var filter = Builders<RegisterViewModel>.Filter.Eq(u => u.Email, modelCredencial);
        var character = await GetCollection().Find(filter).FirstOrDefaultAsync(cancellationToken);
        return character;
    }
}