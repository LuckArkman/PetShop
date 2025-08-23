using System.Data;
using MongoDB.Driver;
using PetShop.Application.Data;
using PetShop.Application.DTOs;
using PetShop.Application.Interfaces;

namespace PetShop.Application.Services;

public class ResponsavelService : IResponsavelService
{
    public ResponsavelDBMongo _dbMongo { get; set; }

    public ResponsavelService()
    {
        
    }
    public async Task<object?> GetObject(string _object, CancellationToken cancellationToken)
    {
        var collection = _dbMongo.GetDatabase().GetCollection<Responsavel>("Responsaveis");

        // Create a filter to find the document by Id
        var filter = Builders<Responsavel>.Filter.Eq(u => u.Id, _object);

        // Find the document matching the filter
        var _responsavel = collection.Find(filter).FirstOrDefault();

        return _responsavel as Responsavel;
    }

    public async Task<object?> InsetObject(Responsavel _object, CancellationToken cancellationToken)
    {
        var collection = _dbMongo.GetDatabase().GetCollection<Responsavel>("Responsaveis");
        // Insert the user object into the collection
        collection.InsertOne(_object);
        return _object as Responsavel;
    }

    public async Task<object?> UpdateObject(Responsavel _object, CancellationToken cancellationToken)
    {
        var obj = await GetObject(_object.Id, CancellationToken.None) as Responsavel;
        var collection = _dbMongo.GetDatabase().GetCollection<Responsavel>("Responsaveis");

        // Create a filter to find the document by Id
        var filter = Builders<Responsavel>.Filter.Eq(u => u.Id, _object.Id);

        var update = Builders<Responsavel>.Update
            .Set(u => u.Email, _object.Email)
            .Set(u => u.FirstName, _object.FirstName)
            .Set(u => u.LastName, _object.LastName)
            .Set(u => u.CPF, _object.CPF)
            .Set(u => u.RG, _object.RG)
            .Set(u => u.Address,  _object.Address)
            .Set(u => u.City, _object.City)
            .Set(u => u.State, _object.State)
            .Set(u => u.ZipCode, _object.ZipCode)
            .Set(u => u.PhoneNumber, _object.PhoneNumber)
            .Set(u => u.Animais, _object.Animais);

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
        var ob = await GetObject(_object.Id, CancellationToken.None) as Animal;
        return ob;
    }

    public async Task RemoveObject(object _object, CancellationToken cancellationToken)
        => await Task.FromResult<object?>(null);
}