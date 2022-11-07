using FlowerShopManagement.Core.Common;
using FlowerShopManagement.Core.Entities;
using FlowerShopManagement.Core.Enums;
using FlowerShopManagement.Infrustructure.Interfaces;
using MongoDB.Bson;
using MongoDB.Bson.Serialization;
using MongoDB.Bson.Serialization.Serializers;
using MongoDB.Driver;


namespace FlowerShopManagement.Infrustructure.DatabaseSettings;

public class MongoDbContext : IMongoDbContext
{
    public IMongoClient _mongoClient { get; private set; }
    public IMongoDatabase _mongoDatabase { get; private set; }
    public IMongoCollection<Customer> _customerCollection { get; private set; }
    public IMongoCollection<Staff> _staffCollection { get; private set; }
    public IMongoCollection<Product> _productCollection { get; private set; }
    public IMongoCollection<Cart> _cartCollection { get; private set; }
    public IMongoCollection<Supplier> _supplierCollection { get; private set; }


    public MongoDbContext(IMongoClient mongoClient)
    {
        ClassMapping();

        _mongoClient = mongoClient;
        _mongoDatabase = _mongoClient.GetDatabase("Test-DB");
        _customerCollection = _mongoDatabase.GetCollection<Customer>(Constants.KEY_CUSTOMER);
        _staffCollection = _mongoDatabase.GetCollection<Staff>(Constants.KEY_STAFFS);
        _productCollection = _mongoDatabase.GetCollection<Product>(Constants.KEY_PRODUCTS);
        _cartCollection = _mongoDatabase.GetCollection<Cart>(Constants.KEY_CARTS);
        _supplierCollection = _mongoDatabase.GetCollection<Supplier>(Constants.KEY_SUPPLIERS);
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
