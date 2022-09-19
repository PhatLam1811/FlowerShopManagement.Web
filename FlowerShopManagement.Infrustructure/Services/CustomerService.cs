using FlowerShopManagement.Core.Model;
using FlowerShopManagement.Infrustructure.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MongoDB.Driver;

namespace FlowerShopManagement.Infrustructure.Services
{
    public class CustomerService : ICustomerServices
    {
        private IMongoCollection<Customer> _customers;

        public CustomerService(ICustomerDatabaseSettings settings, IMongoClient mongoClient)
        {
            var database = mongoClient.GetDatabase(settings.DatabaseName);
            _customers = database.GetCollection<Customer>(settings.CustomerCollection);
        }

        public List<Customer> Get() => _customers.Find(_ => true).ToList();

        public Customer? Get(string id) => _customers.Find(x => x.CustomerId == id).FirstOrDefault();

        public void Create(Customer customer) => _customers.InsertOneAsync(customer);

        public void Remove(string id) => _customers.DeleteOne(x => x.CustomerId == id);

        public void Update(string id, Customer customer) => _customers.ReplaceOne(x => x.CustomerId == id, customer);



    }
}
