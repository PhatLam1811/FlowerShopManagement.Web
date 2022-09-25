using FlowerShopManagement.Core.Enums;

namespace FlowerShopManagement.Core.Entities
{
    public class Product : BaseEntity
    {
        protected string _name { get; set; }
        protected string _picture { get; set; }
        protected Categories[] _categories { get; set; } = new Categories[0];
        protected float _rating { get; set; }
        protected int _uniPrice { get; set; }
        protected float _wholesaleDiscount { get; set; }
        protected IList<Review> _reviews { get; set; } = new List<Review>();
    }
}
