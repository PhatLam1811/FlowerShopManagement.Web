using FlowerShopManagement.Core.Entities;

namespace FlowerShopManagement.Application.Models;

public class CartItemModel
{
    public string _productId { get; set; }
    public string _id { get; private set; }
    public ProductDetailModel items { get; set; }// offline data
    public long amount { get; set; }

    public CartItemModel(string id = "", string pid = "", ProductDetailModel? items = null, long total = 0)
    {
        this._id = id;
        this._productId = pid;
        if (items != null)
            this.items = items;
        this.amount = total;
    }

    public CartItemModel(CartItem cartItem)
    {
        this._id = cartItem._id;
        this._productId = cartItem._productId;
        if (items != null)
            this.items = new ProductDetailModel( cartItem.items);
        this.amount = cartItem.amount;
    }

    public CartItemModel(string customerId)
    {
        this._id = Guid.NewGuid().ToString();
        this._productId = "";
        this.items = new ProductDetailModel();
        this.amount = 0;
    }
    public CartItem ToEntity()
    {
        
        return new CartItem(_id,_productId,items.ToEntity(),amount);

    }

}
