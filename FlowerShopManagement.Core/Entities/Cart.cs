using System.ComponentModel.DataAnnotations;

namespace FlowerShopManagement.Core.Entities;

public class Cart 
{
    public string _id { get; private set; }
    [Required]
    public string customerId { get; set; }
    public List<Product> items { get; set; }
    public long total { get; set; }

    public Cart() 
    {
        this._id = string.Empty;
        this.customerId = string.Empty;
        this.items = new List<Product>();
        this.total = 0;
    }

    public Cart(string customerId)
    {
        this._id = Guid.NewGuid().ToString();
        this.customerId = customerId;
        this.items = new List<Product>();
        this.total = 0;
    }
}
