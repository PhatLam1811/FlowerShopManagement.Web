using System.ComponentModel.DataAnnotations;
using FlowerShopManagement.Core.Entities;

namespace FlowerShopManagement.Application.Models;
public class CartModel
{
    public string Id { get; private set; }
    [Required]
    public string CustomerId { get; set; } = "";
    public List<CartItemModel> Items { get; set; } = new List<CartItemModel>();
    public long Total { get; set; } = 0;

    public CartModel()
    {
        this.Id = Guid.NewGuid().ToString();

    }

    public CartModel(Cart entity)
    {
        this.Id = entity._id;
        this.CustomerId = entity.customerId;
        if (entity.items != null && entity.items.Count > 0)
        {
            foreach (var item in entity.items)
            {
                this.Items.Add(new CartItemModel(item));
            }
        }
        this.Total = entity.total;
    }

    public Cart ToEntity()
    {
        List<CartItem> items = new List<CartItem>();
        if (Items != null && Items.Count > 0)
        {
            foreach (var item in Items)
            {
                items.Add(item.ToEntity());
            }
        }
            
        return new Cart(Id, CustomerId, items , Total);
    }
}
