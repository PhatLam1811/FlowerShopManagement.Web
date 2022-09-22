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
    public class OrderService : IOrderService
    {

        private IMongoCollection<Order> _order;

        public OrderService(IDatabaseSettings settings, IMongoClient mongoClient)
        {
            var database = mongoClient.GetDatabase(settings.DatabaseName);
            _order = database.GetCollection<Order>(CollectionConstant.KEY_ACCOUNT);
        }

        public List<Order> Get() => _order.Find(_ => true).ToList();

        public Order? Get(string id) => _order.Find(x => x.OrderId == id).FirstOrDefault();

        public void Create(Order customer) => _order.InsertOneAsync(customer);

        public void Remove(string id) => _order.DeleteOne(x => x.OrderId == id);

        public void Update(string id, Order customer) => _order.ReplaceOne(x => x.OrderId == id, customer);
    }

}
