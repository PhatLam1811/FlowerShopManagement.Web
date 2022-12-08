using FlowerShopManagement.Infrustructure.MongoDB.Interfaces;
using MongoDB.Driver;

namespace FlowerShopManagement.Infrustructure.MongoDB.Implements;

public class MongoDBContext : IMongoDBContext
{
    private IMongoDatabase _database { get; set; }
    private MongoClient _mongoDbClient { get; set; }

    public MongoDBContext(IMongoDBSettings mongoSettings)
    {
        _mongoDbClient = new MongoClient(mongoSettings.ConnectionString);
        _database = _mongoDbClient.GetDatabase(mongoSettings.DatabaseName);
    }

    public IMongoCollection<T> GetCollection<T>(string name) => _database.GetCollection<T>(name);
}
