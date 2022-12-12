using FlowerShopManagement.Core.Entities;

namespace FlowerShopManagement.Application.Models;

public class LowOnStockProductModel : ProductModel
{
    public int inStockAmount { get; set; }

    public LowOnStockProductModel(Product entity) : base(entity)
    {
        inStockAmount = entity.amount;
    }
}

public class ProductModel
{
    protected string _id { get; set; }
    public string Name { get; set; }
    public string Picture { get; set; }
    public int UnitPrice { get; set; }

    public ProductModel(Product entity)
    {
        _id = entity._id;
        Name = entity.name;
        Picture = entity.pictures[0];
        UnitPrice = entity.price;
    }
}
