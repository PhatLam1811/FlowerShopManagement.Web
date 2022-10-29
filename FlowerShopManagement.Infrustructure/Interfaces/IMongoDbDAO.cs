using FlowerShopManagement.Core.Entities;
using MongoDB.Driver;

namespace FlowerShopManagement.Infrustructure.Interfaces;

public interface IMongoDbDAO
{
    public IMongoClient _mongoClient { get; }
    public IMongoDatabase _mongoDatabase { get; }
    public IMongoCollection<Customer> _customerCollection { get; }
    public IMongoCollection<Profile> _profileCollection { get; }
    public IMongoCollection<Staff> _staffCollection { get; }
    public IMongoCollection<Product> _productCollection { get; }
    public IMongoCollection<Cart> _cartCollection { get; }
    public IMongoCollection<Order> _orderCollection { get; }
    public IMongoCollection<Supplier> _supplierCollection { get; }
}
