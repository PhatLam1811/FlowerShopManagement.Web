using FlowerShopManagement.Core.Enums;

namespace FlowerShopManagement.Core.Entities;

public class Product
{
    public string _id { get; set; }
    public string name { get; set; }
    public List<string> pictures { get; set; }
    public List<Size> sizes { get; set; }
    public List<string> colors { get; set; }
    public string material { get; set; }
    public string? description { get; set; } 
    public List<Categories> categories { get; set; }
    public string type { get; set; }
    public float rating { get; set; }
    public int price { get; set; }
    public int amount { get; set; }
    public float discount { get; set; }
    public List<Review> reviews { get; private set; } = new List<Review>();

    public Product(
        string name, List<string> pictures,
        string? details, string? description,
        List<Categories> categories, 
        int price, float discount)
    {
        _id = Guid.NewGuid().ToString();
        this.name = name;
        this.pictures = pictures;
        this.description = description;
        this.categories = categories;
        this.rating = 0.0f;
        this.price = price;
        this.discount = discount;
        reviews = new List<Review>();
    }
}
