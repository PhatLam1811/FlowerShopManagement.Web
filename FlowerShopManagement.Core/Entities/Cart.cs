using System.ComponentModel.DataAnnotations;

namespace FlowerShopManagement.Core.Entities;

public class Cart
{
    public string _id { get; private set; }
    [Required]
    public string customerId { get; set; }
    public List<Product>? items { get; set; }
    public long total { get; set; }

    public Cart(string id = "", string customerId = "", List<Product>? items = null, long total = 0)
    {
        this._id = id;
        this.customerId = customerId;
        if (items != null)
            this.items = items;
        this.total = total;
    }

    public Cart(string customerId)
    {
        this._id = Guid.NewGuid().ToString();
        this.customerId = customerId;
        this.items = new List<Product>();
        this.total = 0;
    }
}
