using FlowerShopManagement.Core.Enums;

namespace FlowerShopManagement.Core.Entities;

public class Product
{
    public string? _id { get; set; }
    public string _name { get; set; }
    public string _picture { get; set; }
    public List<Categories>? _categories { get; set; }
    public float _rating { get; set; }
    public int _uniPrice { get; set; }
    public int _amount { get; set; }
    public float _wholesaleDiscount { get; set; }
    public List<Review> _reviews { get; private set; } = new List<Review>();

    public Product( string? id = null,
        string name = "", string picture = "", 
        List<Categories>? categories = null, int amount = 0,
        int uniPrice = 0, float wholesaleDiscount = 0.0f)
    {
        if (id != null)
            _id = id;
        _name = name;
        _picture = picture;
        _categories = categories;
        _amount = amount;
        _rating = 0.0f;
        _uniPrice = uniPrice;
        _wholesaleDiscount = wholesaleDiscount;
        _reviews = new List<Review>();
    }
}
