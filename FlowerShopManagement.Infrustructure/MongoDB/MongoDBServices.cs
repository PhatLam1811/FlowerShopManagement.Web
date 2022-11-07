using FlowerShopManagement.Core.Entities;
using FlowerShopManagement.Infrustructure.MongoDB.Interfaces;
using MongoDB.Bson;
using MongoDB.Bson.Serialization;
using MongoDB.Bson.Serialization.Serializers;
using MongoDB.Driver;

namespace FlowerShopManagement.Infrustructure.MongoDB;

public class MongoDBServices : IMongoDBServices
{
    private string _connectionString;
    private string _databaseName;

    public MongoDBServices(string connectionString, string databaseName)
    {
        _connectionString = connectionString;
        _databaseName = databaseName;
    }

    public IMongoCollection<T> ConnectToMongo<T>(in string collectionName)
    {
        var client = new MongoClient(_connectionString);
        var database = client.GetDatabase(_databaseName);
        return database.GetCollection<T>(collectionName);
    }

    public void ClassMapping()
    {
        BsonClassMap.RegisterClassMap<User>(cm =>
        {
            cm.MapIdField(c => c._id);
            cm.IdMemberMap.SetSerializer(new StringSerializer(BsonType.ObjectId));
            cm.AutoMap();
        });

        BsonClassMap.RegisterClassMap<Product>(cm =>
        {
            cm.MapIdField(c => c._id);
            cm.IdMemberMap.SetSerializer(new StringSerializer(BsonType.ObjectId));
            cm.AutoMap();
        });

        BsonClassMap.RegisterClassMap<Order>(cm =>
        {
            cm.MapIdField(c => c._id);
            cm.IdMemberMap.SetSerializer(new StringSerializer(BsonType.ObjectId));
            cm.AutoMap();
        });

        BsonClassMap.RegisterClassMap<Review>(cm =>
        {
            cm.MapIdField(c => c._id);
            cm.IdMemberMap.SetSerializer(new StringSerializer(BsonType.ObjectId));
            cm.AutoMap();
        });
    }
}
