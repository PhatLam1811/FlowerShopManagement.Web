using MongoDB.Driver;

namespace FlowerShopManagement.Infrustructure.MongoDB.Interfaces;

public interface IMongoDBServices
{
    public IMongoCollection<T> ConnectToMongo<T>(in string collectionName);
}
