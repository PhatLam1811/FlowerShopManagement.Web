using FlowerShopManagement.Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace FlowerShopManagement.Core.Interfaces
{
    public interface IOrderService
    {

        public List<Order> Get();

        public Order? Get(string id);

        public void Create(Order customer);

        public void Remove(string id);

        public void Update(string id, Order customer);
    }
}
