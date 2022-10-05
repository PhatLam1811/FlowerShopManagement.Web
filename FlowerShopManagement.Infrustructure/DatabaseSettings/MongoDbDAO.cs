using FlowerShopManagement.Core.Common;
using FlowerShopManagement.Core.Entities;
using FlowerShopManagement.Core.Enums;
using FlowerShopManagement.Infrustructure.Interfaces;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FlowerShopManagement.Infrustructure.DatabaseSettings;

public class MongoDbDAO : IMongoDbDAO
{
    public IMongoClient _mongoClient { get; private set; }
    public IMongoDatabase _mongoDatabase { get; private set; }
    public IMongoCollection<Customer> _customerCollection { get; private set; }
    public IMongoCollection<Staff> _staffCollection { get; private set; }
    public IMongoCollection<Product> _productCollection { get; private set; }
    public IMongoCollection<Cart> _cartCollection { get; private set; }

    public MongoDbDAO(IMongoClient mongoClient)
    {
        _mongoClient = mongoClient;
        _mongoDatabase = _mongoClient.GetDatabase("Test-DB");
        _customerCollection = _mongoDatabase.GetCollection<Customer>(Constants.KEY_CUSTOMER);
        _staffCollection = _mongoDatabase.GetCollection<Staff>(Constants.KEY_STAFFS);
        _productCollection = _mongoDatabase.GetCollection<Product>(Constants.KEY_PRODUCTS);
        _cartCollection = _mongoDatabase.GetCollection<Cart>(Constants.KEY_CARTS);
    }
}
