using MongoDB.Driver;
using FlowerShopManagement.Core.Enums;
using FlowerShopManagement.Core.Interfaces;
using FlowerShopManagement.Core.Entities;
using FlowerShopManagement.Infrustructure.Interfaces;

namespace FlowerShopManagement.Core.Services
{
    public class CustomerService : ICustomerService
    {
        private IMongoCollection<Customer> _customers;

        public CustomerService(IDatabaseSettings settings, IMongoClient mongoClient)
        {
            var database = mongoClient.GetDatabase(settings.DatabaseName);
            _customers = database.GetCollection<Customer>(CollectionConstant.KEY_CUSTOMER);
        }

        public List<Customer> Get() => _customers.Find(_ => true).ToList();

        public Customer? Get(string id) => _customers.Find(x => x.CustomerId == id).FirstOrDefault();

        public void Create(Customer customer) => _customers.InsertOneAsync(customer);

        public void Remove(string id) => _customers.DeleteOne(x => x.CustomerId == id);

        public void Update(string id, Customer customer) => _customers.ReplaceOne(x => x.CustomerId == id, customer);


    }
}
