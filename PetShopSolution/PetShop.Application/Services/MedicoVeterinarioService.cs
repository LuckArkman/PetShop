using MongoDB.Driver;
using PetShop.Application.Data;
using PetShop.Application.DTOs;
using PetShop.Application.Interfaces;
using PetShop.Application.Singletons;

namespace PetShop.Application.Services;

public class MedicoVeterinarioService : IMedicoVeterinarioService
{
    public MedicoVeterinarioDB _db { get; set; }
    private readonly IMongoCollection<MedicoVeterinario> _collection;
    public MedicoVeterinarioService()
    {
        _db = new MedicoVeterinarioDB(Singleton.Instance().src, "MedicoVeterinario");
        _db.GetOrCreateDatabase();
        _collection = _db.GetDatabase().GetCollection<MedicoVeterinario>("MedicoVeterinario");
    }

    public async Task<List<MedicoVeterinario>?> GetAllMedicoVeterinario(CancellationToken cancellationToken)
    {
        var collection = _db.GetDatabase().GetCollection<MedicoVeterinario>("MedicoVeterinario");

        // Busca todos os animais
        var _objts = await collection
            .Find(Builders<MedicoVeterinario>.Filter.Empty)
            .ToListAsync(cancellationToken);

        return _objts;
    }

    public async Task<object?> GetObject(string _object, CancellationToken cancellationToken)
    {
        var collection = _db.GetDatabase().GetCollection<MedicoVeterinario>("MedicoVeterinario");

        // Create a filter to find the document by Id
        var filter = Builders<MedicoVeterinario>.Filter.Eq(u => u.Id, _object);

        // Find the document matching the filter
        var character = collection.Find(filter).FirstOrDefault();

        return character as MedicoVeterinario;
    }

    public async Task<object?> InsetObject(MedicoVeterinario _object, CancellationToken cancellationToken)
    {
        var collection = _db.GetDatabase().GetCollection<MedicoVeterinario>("MedicoVeterinario");
        // Insert the user object into the collection
        collection.InsertOne(_object);
        return _object as MedicoVeterinario;
    }

    public async Task<object?> UpdateObject(MedicoVeterinario _object, CancellationToken cancellationToken)
    {
        var obj = await GetObject(_object.Id, CancellationToken.None) as MedicoVeterinario;
        var collection = _db.GetDatabase().GetCollection<MedicoVeterinario>("MedicoVeterinario");

        // Create a filter to find the document by Id
        var filter = Builders<MedicoVeterinario>.Filter.Eq(u => u.Id, _object.Id);

        var update = Builders<MedicoVeterinario>.Update
            .Set(u => u.Nome, _object.Nome)
            .Set(u => u.CRMV, _object.CRMV)
            .Set(u => u.Especialidade, _object.Especialidade)
            .Set(u => u.Telefone, _object.Telefone)
            .Set(u => u.Email, _object.Email)
            .Set(u => u.Endereco,  _object.Endereco)
            .Set(u => u.Cidade, _object.Cidade)
            .Set(u => u.Estado, _object.Estado)
            .Set(u => u.CEP, _object.CEP);

        // Perform the update
        var result = collection.UpdateOne(filter, update);

        if (result.ModifiedCount > 0)
        {
            Console.WriteLine("User updated successfully.");
        }
        else
        {
            return null;
        }
        var ob = await GetObject(_object.Id, CancellationToken.None) as MedicoVeterinario;
        return ob;
    }

    public async Task<bool> RemoveAsync(object _object, CancellationToken cancellationToken)
    {
        var filter = Builders<MedicoVeterinario>.Filter.Eq(u => u.CRMV, _object);
        var result = await _collection.DeleteOneAsync(filter, cancellationToken);

        return result.DeletedCount > 0;  
    }

    public async Task<object?> FindByCRMVAsync(string modelCredencial, CancellationToken cancellationToken)
    {
        var collection = _db.GetDatabase().GetCollection<MedicoVeterinario>("MedicoVeterinario");

        // Create a filter to find the document by Id
        var filter = Builders<MedicoVeterinario>.Filter.Eq(u => u.CRMV, modelCredencial);

        // Find the document matching the filter
        var character = collection.Find(filter).FirstOrDefault();

        return character as MedicoVeterinario;
    }
}