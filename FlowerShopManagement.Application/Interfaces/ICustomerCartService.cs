using FlowerShopManagement.Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace FlowerShopManagement.Core.Interfaces
{
    public interface ICustomerCartService
    {
        public List<CustomerCart> Get();

        public CustomerCart? Get(string id);

        public void Create(CustomerCart customer);

        public void Remove(string id);

        public void Update(string id, CustomerCart customer);
    }
}
