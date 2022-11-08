using FlowerShopManagement.Infrustructure.MongoDB.Interfaces;

namespace FlowerShopManagement.Infrustructure.MongoDB.Implements;

public class MongoDBSettings : IMongoDBSettings
{
    public string ConnectionString { get; set; } = string.Empty;
    public string DatabaseName { get; set; } = string.Empty;
}
