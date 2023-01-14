using System.ComponentModel.DataAnnotations;

namespace FlowerShopManagement.Core.Entities;

public class Cart
{
    public string _id { get; private set; }
    [Required]
    public string customerId { get; set; }
    public List<CartItem>? items { get; set; }
    public double total { get; set; }

    public Cart(string id = "", string customerId = "", List<CartItem>? items = null, double total = 0)
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
        this.items = new List<CartItem>();
        this.total = 0;
    }
}
