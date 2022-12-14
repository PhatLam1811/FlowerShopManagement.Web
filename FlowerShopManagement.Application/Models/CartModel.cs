using System.ComponentModel.DataAnnotations;

namespace FlowerShopManagement.Core.Entities;

public class CartModel
{
    public string Id { get; private set; }
    [Required]
    public string CustomerId { get; set; } = "";
    public List<Product> Items { get; set; } = new List<Product>();
    public long Total { get; set; } = 0;

    public CartModel()
    {
        this.Id = Guid.NewGuid().ToString();

    }

    public CartModel(Cart entity)
    {
        this.Id = entity._id;
        this.CustomerId = entity.customerId;
        this.Items = entity.items;
        this.Total = entity.total;
    }

    public Cart ToEntity()
    {
        return new Cart(Id, CustomerId, Items, Total);
    }
}
