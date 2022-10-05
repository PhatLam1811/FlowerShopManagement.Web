using FlowerShopManagement.Core.Enums;

namespace FlowerShopManagement.Core.Entities;

public class Product
{
    public string _id { get; private set; }
    public string _name { get; set; }
    public string _picture { get; set; }
    public List<Categories> _categories { get; set; }
    public float _rating { get; set; }
    public int _uniPrice { get; set; }
    public float _wholesaleDiscount { get; set; }
    public List<Review> _reviews { get; set; } = new List<Review>();

    public Product(
        string name, string picture, 
        List<Categories> categories, 
        int uniPrice, float wholesaleDiscount)
    {
        _id = Guid.NewGuid().ToString();
        _name = name;
        _picture = picture;
        _categories = categories;
        _rating = 0.0f;
        _uniPrice = uniPrice;
        _wholesaleDiscount = wholesaleDiscount;
        _reviews = new List<Review>();
    }
}
