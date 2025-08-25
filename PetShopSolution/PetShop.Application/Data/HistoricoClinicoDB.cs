using MongoDB.Driver;

namespace PetShop.Application.Data;

public class HistoricoClinicoDB
{
    IMongoClient client;
    string databaseName;
    IMongoDatabase _database;
    public HistoricoClinicoDB()
    {
        
    }

    public HistoricoClinicoDB(string connectionString, string _databaseName)
    {
        this.client = new MongoClient(connectionString);
        this.databaseName = _databaseName;
        GetOrCreateDatabase();
    }

    public IMongoDatabase GetDatabase()
    {
        return _database;
    }

    public void GetOrCreateDatabase()
    {
        _database = client.GetDatabase(databaseName);
        var collectionList = _database.ListCollectionNames().ToList();
        if (collectionList.Count <= 0) _database.CreateCollection(databaseName);
    }
}