using MongoDB.Driver;

namespace Data;

public class AnimalDBMongo
{
    IMongoClient client;
    string databaseName;
    string collectionName;
    IMongoDatabase _database;
    public AnimalDBMongo()
    {
        
    }
    public AnimalDBMongo(string connectionString, string _databaseName, string _collectionName)
    {
        this.client = new MongoClient(connectionString);
        this.databaseName = _databaseName;
        this.collectionName = _collectionName;
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