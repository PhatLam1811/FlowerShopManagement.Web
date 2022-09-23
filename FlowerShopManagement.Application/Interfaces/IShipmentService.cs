using FlowerShopManagement.Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace FlowerShopManagement.Core.Interfaces
{
    public interface IAccountService
    {

        public List<Account> Get();

        public Account? Get(string id);

        public void Create(Account customer);

        public void Remove(string id);

        public void Update(string id, Account customer);
    }
}
