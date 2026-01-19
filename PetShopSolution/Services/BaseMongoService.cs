using Data;
using Interfaces;
using Microsoft.Extensions.Configuration;
using MongoDB.Driver;

namespace Services;

public abstract class BaseMongoService<T>
{
    protected readonly ITenantService _tenantService;
    protected readonly IConfiguration _configuration;
    protected IMongoCollection<T>? _collection;
    protected MongoDataController? _db;
    protected IMongoDatabase? _mongoDatabase;
    protected string? _collectionName;

    protected BaseMongoService(ITenantService tenantService, IConfiguration configuration)
    {
        _tenantService = tenantService;
        _configuration = configuration;
    }

    public virtual void InitializeCollection(string? connectionString,
        string? databaseName,
        string? collectionName)
    {
        _collectionName = collectionName ?? typeof(T).Name;
        var conn = connectionString ?? _configuration["MongoDbSettings:ConnectionString"]
                    ?? throw new InvalidOperationException("Connection string not found.");

        var dbName = _tenantService.GetDatabaseName() ?? databaseName ?? _configuration["MongoDbSettings:DataBaseName"]
                    ?? throw new InvalidOperationException("Database name not found.");

        _db = new MongoDataController(conn, dbName, _collectionName);
        _mongoDatabase = _db.GetDatabase();
        _collection = _mongoDatabase.GetCollection<T>(_collectionName);
    }

    protected IMongoCollection<T> GetCollection()
    {
        if (_collection == null)
        {
            // Tenta inicializar com valores padrão se não for chamado manualmente
            InitializeCollection(null, null, null);
        }
        return _collection!;
    }
}
