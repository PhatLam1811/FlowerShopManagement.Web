using MongoDB.Bson.Serialization.Attributes;

namespace FlowerShopManagement.Core.Entities
{
    internal class Shipment
    {
        string shipmentId = String.Empty;
        string orderID = String.Empty;
        DateTime shipmentDate = DateTime.MinValue;
        int shipmentPrice = 0;
        string shipmentState = String.Empty;

        [BsonId]
        [BsonRepresentation(MongoDB.Bson.BsonType.ObjectId)]
        public string ShipmentId
        {
            get { return shipmentId; }
            set { shipmentId = value; }
        }
        public string OrderID
        {
            get { return orderID; }
            set { orderID = value; }
        }
        public DateTime ShipmentDate
        {
            get { return shipmentDate; }
            set { shipmentDate = value; }
        }
        public int ShipmentPrice
        {
            get { return shipmentPrice; }
            set { shipmentPrice = value; }
        }
        public string ShipmentState
        {
            get { return shipmentState; }
            set { shipmentState = value; }
        }
    }
}
