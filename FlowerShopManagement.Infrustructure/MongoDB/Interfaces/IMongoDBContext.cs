using MongoDB.Driver;

namespace FlowerShopManagement.Infrustructure.MongoDB.Interfaces;

public interface IMongoDBContext
{
    IMongoCollection<T> GetCollection<T>(string collectionName);
}
