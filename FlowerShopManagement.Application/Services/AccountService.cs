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
    public class AccountService : IAccountService
    {

        private IMongoCollection<Account> _account;

        public AccountService(IDatabaseSettings settings, IMongoClient mongoClient)
        {
            var database = mongoClient.GetDatabase(settings.DatabaseName);
            _account = database.GetCollection<Account>(CollectionConstant.KEY_ACCOUNT);
        }

        public List<Account> Get() => _account.Find(_ => true).ToList();

        public Account? Get(string id) => _account.Find(x => x.AccountId == id).FirstOrDefault();

        public void Create(Account customer) => _account.InsertOneAsync(customer);

        public void Remove(string id) => _account.DeleteOne(x => x.AccountId == id);

        public void Update(string id, Account customer) => _account.ReplaceOne(x => x.AccountId == id, customer);
    }

}
