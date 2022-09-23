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
    public class GoodCategoryService : IGoodCategoryService
    {

        private IMongoCollection<GoodCategory> _goodCatergory;

        public GoodCategoryService(IDatabaseSettings settings, IMongoClient mongoClient)
        {
            var database = mongoClient.GetDatabase(settings.DatabaseName);
            _goodCatergory = database.GetCollection<GoodCategory>(CollectionConstant.KEY_GOOD_CART);
        }

        public List<GoodCategory> Get() => _goodCatergory.Find(_ => true).ToList();

        public GoodCategory? Get(string id) => _goodCatergory.Find(x => x.GoodCategoryId == id).FirstOrDefault();

        public void Create(GoodCategory customer) => _goodCatergory.InsertOneAsync(customer);

        public void Remove(string id) => _goodCatergory.DeleteOne(x => x.GoodCategoryId == id);

        public void Update(string id, GoodCategory customer) => _goodCatergory.ReplaceOne(x => x.GoodCategoryId == id, customer);
    }

}
