namespace FlowerShopManagement.Infrustructure.MongoDB.Interfaces;

public interface IMongoDBSettings
{
    public string ConnectionString { get; set; }
    public string DatabaseName { get; set; }
}
