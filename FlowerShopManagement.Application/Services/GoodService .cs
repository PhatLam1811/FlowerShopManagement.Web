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
    public class GoodService : IGoodService
    {

        private IMongoCollection<Product> _good;

        public GoodService(IDatabaseSettings settings, IMongoClient mongoClient)
        {
            var database = mongoClient.GetDatabase(settings.DatabaseName);
            _good = database.GetCollection<Product>(CollectionConstant.KEY_GOOD);
        }

        List<Product> IServices<Product>.Get() => _good.Find(_ => true).ToList();

        Product IServices<Product>.Get(string id) => _good.Find(x => x.GoodId == id).FirstOrDefault();

        public void Create(Product customer) => _good.InsertOneAsync(customer);

        public void Remove(string id) => _good.DeleteOne(x => x.GoodId == id);

        public void Update(string id, Product customer) => _good.ReplaceOne(x => x.GoodId == id, customer);

    }

}
