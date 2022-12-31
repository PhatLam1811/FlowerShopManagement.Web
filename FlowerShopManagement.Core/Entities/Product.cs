using FlowerShopManagement.Core.Enums;

namespace FlowerShopManagement.Core.Entities;

public class Product
{
    public string? _id { get; set; }
    public string _name { get; set; }
    //public string _picture { get; set; }
    public List<string> _pictures { get; set; } 

    //public Category _category { get; set; } 
    public string _category { get; set; } 
    public float _rating { get; set; }
    public int _uniPrice { get; set; }
    public int _amount { get; set; }
    public float _wholesaleDiscount { get; set; }
    public List<Review> _reviews { get; private set; } = new List<Review>();

    // adding attributes
    public Color _color { get; set; }
    public string _description { get; set; }
    //public Material _material { get; set; }
    public string _material { get; set; }
    public string _size { get; set; }
    public string _maintainment { get; set; }

    public bool _isLike { get; set; }

    public Product( string? id = null,
        string name = "", List<string>? picture = null, 
        string category = "Unknown", int amount = 0,
        int uniPrice = 0, float wholesaleDiscount = 0.0f, string description = "", 
        string maintainment  = "", string size = "0cm x 0cm x 0cm", string material = "Unknown", Color color = Color.Sample, bool isLike = false)
    {
        if (id != null)
            _id = id;
        _name = name;
        _pictures = picture ?? new List<string>(); 
        _category = category ;
        _amount = amount;
        _rating = 0.0f;
        _uniPrice = uniPrice;
        _wholesaleDiscount = wholesaleDiscount;
        _reviews = new List<Review>();
        _color = color;
        _description = description;
        _maintainment = maintainment;
        _size = size;
        _material = material; 
        _isLike = isLike;
    }
}
