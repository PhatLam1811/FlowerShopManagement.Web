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
    public class ShipmentService : IShipmentService
    {

        private IMongoCollection<Shipment> _shipment;

        public ShipmentService(IDatabaseSettings settings, IMongoClient mongoClient)
        {
            var database = mongoClient.GetDatabase(settings.DatabaseName);
            _shipment = database.GetCollection<Shipment>(CollectionConstant.KEY_SHIPMENT);
        }

        public List<Shipment> Get() => _shipment.Find(_ => true).ToList();

        public Shipment? Get(string id) => _shipment.Find(x => x.OrderID == id).FirstOrDefault();

        public void Create(Shipment customer) => _shipment.InsertOneAsync(customer);

        public void Remove(string id) => _shipment.DeleteOne(x => x.OrderID == id);

        public void Update(string id, Shipment customer) => _shipment.ReplaceOne(x => x.OrderID == id, customer);
    }

}
