using MongoDB.Bson.Serialization.Attributes;

namespace FlowerShopManagement.Core.Model
{
    internal class Good
    {
        string goodId = String.Empty;
        string goodName = String.Empty;
        string categoryId = String.Empty;
        int goodUnitPrice = 0;
        float goodWholeSaleDiscount = 0;

        [BsonId]
        [BsonRepresentation(MongoDB.Bson.BsonType.ObjectId)]
        public string GoodId
        {
            get { return goodId; }
            set { goodId = value; }
        }
        public string GoodName
        {
            get { return goodName; }
            set { goodName = value; }
        }
        public string CategoryId
        {
            get { return categoryId; }
            set { categoryId = value; }
        }
        public int GoodUnitPrice
        {
            get { return goodUnitPrice; }
            set { goodUnitPrice = value; }
        }
        public float GoodWholeSaleDiscount
        {
            get { return goodWholeSaleDiscount; }
            set { goodWholeSaleDiscount = value; }
        }
    }
}
