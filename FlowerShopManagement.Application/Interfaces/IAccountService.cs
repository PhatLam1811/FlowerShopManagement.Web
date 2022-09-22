using FlowerShopManagement.Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace FlowerShopManagement.Core.Interfaces
{
    public interface IShipmentService
    {
        public List<Shipment> Get();

        public Shipment? Get(string id);

        public void Create(Shipment customer);

        public void Remove(string id);

        public void Update(string id, Shipment customer);
    }
}
