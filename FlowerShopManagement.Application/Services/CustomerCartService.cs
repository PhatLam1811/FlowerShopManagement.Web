using FlowerShopManagement.Application.Interfaces;
using FlowerShopManagement.Core.Entities;
using FlowerShopManagement.Core.Enums;
using FlowerShopManagement.Core.Interfaces;
using FlowerShopManagement.Infrustructure.Interfaces;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FlowerShopManagement.Application.Services
{
    public class CustomerCartService : ICustomerCartService
    {

        private IMongoCollection<CustomerCart> _customerCart;

        public CustomerCartService(IDatabaseSettings settings, IMongoClient mongoClient)
        {
            var database = mongoClient.GetDatabase(settings.DatabaseName);
            _customerCart = database.GetCollection<CustomerCart>(CollectionConstant.KEY_CUSTOMER_CART);
        }

        public List<CustomerCart> Get() => _customerCart.Find(_ => true).ToList();

        public CustomerCart? Get(string id) => _customerCart.Find(x => x.CartId == id).FirstOrDefault();

        public void Create(CustomerCart customer) => _customerCart.InsertOneAsync(customer);

        public void Remove(string id) => _customerCart.DeleteOne(x => x.CartId == id);

        public void Update(string id, CustomerCart customer) => _customerCart.ReplaceOne(x => x.CartId == id, customer);

       
    }

}
