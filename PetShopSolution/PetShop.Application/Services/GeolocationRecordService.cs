using MongoDB.Driver;
using PetShop.Application.Data;
using PetShop.Application.DTOs;
using PetShop.Application.Interfaces;
using PetShop.Application.Singletons;

namespace PetShop.Application.Services;

public class GeolocationRecordService : IGeolocationRecordService
{
    public GeolocationRecordDB _db { get; set; }

    public GeolocationRecordService()
    {
        _db = new GeolocationRecordDB(Singleton.Instance().src, "GeolocationRecord");
        _db.GetOrCreateDatabase();
    }
    public async Task<object?> GetObject(string _object, CancellationToken cancellationToken)
    {
        var collection = _db.GetDatabase().GetCollection<GeolocationRecord>("GeolocationRecord");
        
        var filter = MongoDB.Driver.Builders<GeolocationRecord>.Filter.Eq(u => u.Id, _object);
        
        var character = collection.Find(filter).FirstOrDefault();

        return character as GeolocationRecord;
    }

    public async Task<object?> InsetObject(GeolocationRecord _object, CancellationToken cancellationToken)
    {
        var collection = _db.GetDatabase().GetCollection<GeolocationRecord>("GeolocationRecord");
        collection.InsertOne(_object);
        return _object as GeolocationRecord;
    }

    public async Task<object?> UpdateObject(GeolocationRecord _object, CancellationToken cancellationToken)
    {
        var obj = await GetObject(_object.Id, CancellationToken.None) as GeolocationRecord;
        var collection = _db.GetDatabase().GetCollection<GeolocationRecord>("GeolocationRecord");

        // Create a filter to find the document by Id
        var filter = MongoDB.Driver.Builders<GeolocationRecord>.Filter.Eq(u => u.Id, _object.Id);

        var update = MongoDB.Driver.Builders<GeolocationRecord>.Update
            .Set(u => u.Id, _object.Id)
            .Set(u => u.AnimalId, _object.AnimalId)
            .Set(u => u.Latitude, _object.Latitude)
            .Set(u => u.Longitude, _object.Longitude)
            .Set(u => u.DataRegistro, _object.DataRegistro)
            .Set(u => u.Endereco, _object.Endereco)
            .Set(u => u.Observacoes, _object.Observacoes);

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
        var ob = await GetObject(_object.Id, CancellationToken.None) as GeolocationRecord;
        return ob;
    }

    public async Task RemoveObject(object _object, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }
}