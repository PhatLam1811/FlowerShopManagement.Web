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
        public List<Cart> Get();

        public Cart? Get(string id);

        public void Create(Cart customer);

        public void Remove(string id);

        public void Update(string id, Cart customer);
    }
}
